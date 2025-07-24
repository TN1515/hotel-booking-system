using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using HotelBooking.Hubs;

namespace HotelBooking.Controllers
{
    [Authorize(Roles = "Admin,Staff,Customer")]
    public class MessagingController : BaseController
    {
        private readonly HotelBookingContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagingController(HotelBookingContext context, UserManager<CustomUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet("api/messaging/current-user")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Json(new { userId = userId });
        }

        // GET: Messaging
        public async Task<IActionResult> Index(int? chatUserId = null)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Forbid();
            }
            var currentUserId = int.Parse(userIdStr);
            
            // Get conversations (unique users the current user has messaged with)
            var conversations = await _context.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => new
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(m => m.SentAt).First(),
                    UnreadCount = g.Count(m => m.ReceiverId == currentUserId && !m.IsRead)
                })
                .ToListAsync();

            var conversationList = new List<dynamic>();
            foreach (var conv in conversations)
            {
                var otherUser = await _userManager.FindByIdAsync(conv.UserId.ToString());
                if (otherUser != null)
                {
                    conversationList.Add(new
                    {
                        UserId = conv.UserId,
                        UserName = otherUser.UserName,
                        Email = otherUser.Email,
                        LastMessage = conv.LastMessage.Content,
                        LastMessageTime = conv.LastMessage.SentAt,
                        UnreadCount = conv.UnreadCount,
                        IsLastMessageFromMe = conv.LastMessage.SenderId == currentUserId
                    });
                }
            }

            ViewBag.Conversations = conversationList.OrderByDescending(c => c.LastMessageTime).ToList();
            ViewBag.ChatUserId = chatUserId;
            return View();
        }

        [HttpGet("api/messaging/conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Forbid();
            }
            var currentUserId = int.Parse(userIdStr);

            var conversations = await _context.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => new
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(m => m.SentAt).First(),
                    UnreadCount = g.Count(m => m.ReceiverId == currentUserId && !m.IsRead)
                })
                .ToListAsync();

            var result = conversations.Select(c => new
            {
                userId = c.UserId,
                userName = c.LastMessage.SenderId == currentUserId ? c.LastMessage.Receiver.UserName : c.LastMessage.Sender.UserName,
                lastMessage = c.LastMessage.Content,
                lastMessageTime = c.LastMessage.SentAt,
                unreadCount = c.UnreadCount,
                isOnline = true // You can implement online status logic here
            })
            .OrderByDescending(c => c.lastMessageTime); // Sort by last message time, newest first

            return Json(result);
        }

        [HttpGet("api/messaging/messages/{userId}")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Forbid();
            }
            var currentUserId = int.Parse(userIdStr);

            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                           (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    senderId = m.SenderId,
                    content = m.Content,
                    sentAt = m.SentAt
                })
                .ToListAsync();

            // Mark messages as read
            var unreadMessages = await _context.Messages
                .Where(m => m.SenderId == userId && m.ReceiverId == currentUserId && !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
            }
            await _context.SaveChangesAsync();

            return Json(messages);
        }

        [HttpPost("api/messaging/send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sender = await _userManager.FindByIdAsync(currentUserId.ToString());
            var senderRoles = await _userManager.GetRolesAsync(sender);
            var isAdmin = senderRoles.Contains("Admin");

            var receiver = await _userManager.FindByIdAsync(request.ReceiverId.ToString());
            var receiverRoles = await _userManager.GetRolesAsync(receiver);
            var isReceiverCustomer = receiverRoles.Contains("Customer");

            if (isAdmin && isReceiverCustomer)
            {
                return BadRequest(new { success = false, message = "Admin không được phép chat với Customer." });
            }

            var message = new Message
            {
                SenderId = currentUserId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                SentAt = DateTime.Now,
                IsRead = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Send via SignalR
            var senderUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            await _hubContext.Clients.User(request.ReceiverId.ToString())
                .SendAsync("ReceiveMessage", currentUserId, senderUser.UserName, request.Content);

            return Ok();
        }

        [HttpGet("api/messaging/staff")]
        public async Task<IActionResult> GetStaff()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var staff = await _userManager.Users
                .Where(u => u.Id != currentUserId)
                .Select(u => new
                {
                    id = u.Id,
                    fullName = u.UserName,
                    email = u.Email
                })
                .ToListAsync();

            return Json(staff);
        }

        // GET: Messaging/Chat/5 - Redirect to main messaging page
        public IActionResult Chat(int id)
        {
            return RedirectToAction("Index", new { chatUserId = id });
        }

        // POST: Messaging/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage(int receiverId, string content)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var sender = await _userManager.FindByIdAsync(currentUserId.ToString());
                var senderRoles = await _userManager.GetRolesAsync(sender);
                var isAdmin = senderRoles.Contains("Admin");

                var receiver = await _userManager.FindByIdAsync(receiverId.ToString());
                var receiverRoles = await _userManager.GetRolesAsync(receiver);
                var isReceiverCustomer = receiverRoles.Contains("Customer");

                if (isAdmin && isReceiverCustomer)
                {
                    return Json(new { success = false, message = "Admin không được phép chat với Customer." });
                }
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Json(new { success = false, message = "Message content cannot be empty." });
                }

                var message = new Message
                {
                    SenderId = currentUserId,
                    ReceiverId = receiverId,
                    Content = content.Trim(),
                    SentAt = DateTime.Now,
                    IsRead = false,
                    CreatedBy = User.Identity!.Name,
                    CreatedDate = DateTime.Now
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                // Get sender info for response
                var senderUser = await _userManager.FindByIdAsync(currentUserId.ToString());

                return Json(new
                {
                    success = true,
                    message = new
                    {
                        messageId = message.MessageId,
                        content = message.Content,
                        sentAt = message.SentAt,
                        senderName = senderUser?.UserName,
                        senderId = currentUserId,
                        isRead = false,
                        isFromCurrentUser = true
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Messaging/GetMessages/5
        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, DateTime? lastMessageTime = null)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                IQueryable<Message> query = _context.Messages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                               (m.SenderId == userId && m.ReceiverId == currentUserId))
                    .Include(m => m.Sender);

                if (lastMessageTime.HasValue)
                {
                    query = query.Where(m => m.SentAt > lastMessageTime.Value);
                }

                var messages = await query
                    .OrderBy(m => m.SentAt)
                    .Select(m => new
                    {
                        messageId = m.MessageId,
                        content = m.Content,
                        sentAt = m.SentAt,
                        senderName = m.Sender!.UserName,
                        senderId = m.SenderId,
                        isRead = m.IsRead,
                        isFromCurrentUser = m.SenderId == currentUserId
                    })
                    .ToListAsync();

                return Json(new { success = true, messages });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Messaging/StaffList
        public async Task<IActionResult> StaffList()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var isAdmin = userRoles.Contains("Admin");
            var isStaff = userRoles.Contains("Staff");
            var isCustomer = userRoles.Contains("Customer");

            List<CustomUser> users = new List<CustomUser>();
            if (isStaff) {
                // Staff: chat với admin, staff, customer (trừ chính mình)
                var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                var customerUsers = await _userManager.GetUsersInRoleAsync("Customer");
                users = staffUsers.Concat(adminUsers).Concat(customerUsers)
                    .Where(u => u.Id != currentUserId && u.IsActive)
                    .Distinct().ToList();
            } else if (isAdmin) {
                // Admin: chỉ chat với staff
                var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
                users = staffUsers.Where(u => u.Id != currentUserId && u.IsActive).ToList();
            } else if (isCustomer) {
                // Customer: chỉ chat với staff
                var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
                users = staffUsers.Where(u => u.Id != currentUserId && u.IsActive).ToList();
            }

            var allUsers = new List<object>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                allUsers.Add(new {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = roles.FirstOrDefault() ?? "Unknown"
                });
            }

            return Json(new { success = true, users = allUsers });
        }

        // POST: Messaging/MarkAsRead
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int senderId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                var unreadMessages = await _context.Messages
                    .Where(m => m.SenderId == senderId && m.ReceiverId == currentUserId && !m.IsRead)
                    .ToListAsync();

                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, markedCount = unreadMessages.Count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Messaging/GetUnreadCount
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                
                var unreadCount = await _context.Messages
                    .CountAsync(m => m.ReceiverId == currentUserId && !m.IsRead);

                return Json(new { success = true, count = unreadCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class SendMessageRequest
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
