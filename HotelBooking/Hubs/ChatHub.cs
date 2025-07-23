using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HotelBooking.Models;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelBooking.Hubs
{
    [Authorize(Roles = "Staff,Customer")]
    public class ChatHub : Hub
    {
        private readonly HotelBookingContext _context;
        private static readonly Dictionary<string, UserConnection> _connections = new Dictionary<string, UserConnection>();

        public ChatHub(HotelBookingContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(userId))
            {
                _connections[Context.ConnectionId] = new UserConnection
                {
                    UserId = int.Parse(userId),
                    UserName = userName,
                    ConnectionId = Context.ConnectionId,
                    ConnectedAt = DateTime.Now,
                    IsOnline = true
                };

                // Notify others that user is online
                await Clients.Others.SendAsync("UserOnline", int.Parse(userId));
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var connection))
            {
                _connections.Remove(Context.ConnectionId);

                // Notify others that user is offline
                await Clients.Others.SendAsync("UserOffline", connection.UserId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(int receiverId, string message)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = Context.User.Identity.Name;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(message))
                return;

            var senderId = int.Parse(userId);

            // Save message to database
            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                SentAt = DateTime.Now,
                IsRead = false,
                CreatedBy = userName,
                CreatedDate = DateTime.Now
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Find receiver's connection
            var receiverConnections = _connections.Values
                .Where(c => c.UserId == receiverId)
                .Select(c => c.ConnectionId)
                .ToList();

            // Send to specific user if online
            if (receiverConnections.Any())
            {
                await Clients.Clients(receiverConnections).SendAsync("ReceiveMessage", new
                {
                    messageId = newMessage.MessageId,
                    content = newMessage.Content,
                    sentAt = newMessage.SentAt,
                    senderName = userName,
                    senderId = senderId,
                    isRead = false,
                    isFromCurrentUser = false
                });
            }

            // Send back to sender for confirmation
            await Clients.Caller.SendAsync("MessageSent", new
            {
                messageId = newMessage.MessageId,
                content = newMessage.Content,
                sentAt = newMessage.SentAt,
                receiverId = receiverId,
                isRead = false
            });
        }

        public async Task MarkAsRead(int senderId)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
                return;

            var currentUserId = int.Parse(userId);

            // Mark messages as read in database
            var unreadMessages = await _context.Messages
                .Where(m => m.SenderId == senderId && m.ReceiverId == currentUserId && !m.IsRead)
                .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();

                // Find sender's connection
                var senderConnections = _connections.Values
                    .Where(c => c.UserId == senderId)
                    .Select(c => c.ConnectionId)
                    .ToList();

                // Notify sender that messages were read
                if (senderConnections.Any())
                {
                    await Clients.Clients(senderConnections).SendAsync("MessagesRead", currentUserId);
                }
            }
        }

        public async Task SendTypingIndicator(int receiverId, bool isTyping)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = Context.User.Identity.Name;

            if (string.IsNullOrEmpty(userId))
                return;

            var senderId = int.Parse(userId);

            // Find receiver's connection
            var receiverConnections = _connections.Values
                .Where(c => c.UserId == receiverId)
                .Select(c => c.ConnectionId)
                .ToList();

            // Send typing indicator to receiver if online
            if (receiverConnections.Any())
            {
                await Clients.Clients(receiverConnections).SendAsync("TypingIndicator", new
                {
                    userId = senderId,
                    userName = userName,
                    isTyping = isTyping
                });
            }
        }


    }

    public class UserConnection
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; }
        public bool IsOnline { get; set; }

        public UserConnection()
        {
            UserName = string.Empty;
            ConnectionId = string.Empty;
        }
    }
}
