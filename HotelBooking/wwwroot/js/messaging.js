// Messaging System JavaScript

class MessagingSystem {
    constructor() {
        this.currentUserId = null;
        this.receiverId = null;
        this.lastMessageTime = null;
        this.refreshInterval = null;
        this.typingTimeout = null;
        this.isTyping = false;
        
        this.init();
    }
    
    init() {
        this.bindEvents();
        this.startAutoRefresh();
        this.loadUnreadCount();
    }
    
    bindEvents() {
        // Message form submission
        $(document).on('submit', '#messageForm', (e) => {
            e.preventDefault();
            this.sendMessage();
        });
        
        // Enter key handling
        $(document).on('keypress', '#messageInput', (e) => {
            if (e.which === 13 && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
        
        // Typing indicator
        $(document).on('input', '#messageInput', () => {
            this.handleTyping();
        });
        
        // Conversation selection
        $(document).on('click', '.conversation-item', (e) => {
            const userId = $(e.currentTarget).data('user-id');
            this.openChat(userId);
        });
        
        // Staff selection
        $(document).on('click', '.staff-item', (e) => {
            const userId = $(e.currentTarget).data('user-id');
            const userName = $(e.currentTarget).data('user-name');
            this.startChat(userId, userName);
        });
        
        // Page visibility change
        $(document).on('visibilitychange', () => {
            if (!document.hidden) {
                this.markAsRead();
            }
        });
        
        // Window focus
        $(window).on('focus', () => {
            this.markAsRead();
        });
    }
    
    startAutoRefresh() {
        // Refresh conversations every 30 seconds
        this.refreshInterval = setInterval(() => {
            if (window.location.pathname === '/Messaging') {
                this.refreshConversations();
                this.loadUnreadCount();
            }
        }, 3000);
    }
    
    sendMessage() {
        const content = $('#messageInput').val().trim();
        if (!content) return;
        
        const sendButton = $('#sendButton');
        const messageInput = $('#messageInput');
        
        // Show sending state
        this.setSendingState(true);
        
        $.post('/Messaging/SendMessage', {
            receiverId: this.receiverId,
            content: content
        })
        .done((response) => {
            if (response.success) {
                messageInput.val('');
                this.addMessageToChat(response.message, true);
                this.updateLastMessageTime();
                this.scrollToBottom();
                this.showMessageStatus('sent');
            } else {
                this.showError('Failed to send message: ' + response.message);
            }
        })
        .fail(() => {
            this.showError('Error sending message. Please try again.');
        })
        .always(() => {
            this.setSendingState(false);
        });
    }
    
    checkForNewMessages() {
        if (!this.lastMessageTime || !this.receiverId) return;
        
        $.get('/Messaging/GetMessages', {
            userId: this.receiverId,
            lastMessageTime: this.lastMessageTime.toISOString()
        })
        .done((response) => {
            if (response.success && response.messages.length > 0) {
                response.messages.forEach((message) => {
                    this.addMessageToChat(message, message.senderId === this.currentUserId);
                });
                
                this.updateLastMessageTime();
                this.scrollToBottom();
                this.markAsRead();
                this.playNotificationSound();
            }
        })
        .fail(() => {
            console.error('Failed to check for new messages');
        });
    }
    
    addMessageToChat(message, isSent) {
        const messageHtml = this.createMessageHtml(message, isSent);
        $('#messagesContainer').append(messageHtml);
        
        // Animate message appearance
        $('.message-item:last-child').hide().fadeIn(300);
    }
    
    createMessageHtml(message, isSent) {
        const statusIcon = isSent ? this.getStatusIcon(message.isRead) : '';
        
        return `
            <div class="message-item ${isSent ? 'message-sent' : 'message-received'}">
                <div class="message-bubble ${isSent ? 'bg-primary text-white' : 'bg-light'}">
                    <div class="message-content">${this.escapeHtml(message.content)}</div>
                    <div class="message-time">
                        <small class="${isSent ? 'text-white-50' : 'text-muted'}">
                            ${message.sentAt}
                            ${statusIcon}
                        </small>
                    </div>
                </div>
            </div>
        `;
    }
    
    getStatusIcon(isRead) {
        if (isRead) {
            return '<i class="fas fa-check-double message-status read ms-1" title="Read"></i>';
        } else {
            return '<i class="fas fa-check message-status delivered ms-1" title="Delivered"></i>';
        }
    }
    
    scrollToBottom() {
        const container = $('#messagesContainer');
        if (container.length) {
            container.animate({
                scrollTop: container[0].scrollHeight
            }, 300);
        }
    }
    
    markAsRead() {
        if (!this.receiverId) return;
        
        $.post('/Messaging/MarkAsRead', {
            senderId: this.receiverId
        })
        .done((response) => {
            if (response.success && response.markedCount > 0) {
                this.updateReadStatus();
            }
        });
    }
    
    updateReadStatus() {
        // Update message status icons
        $('.message-sent .message-status.delivered').each(function() {
            $(this).removeClass('delivered').addClass('read')
                   .removeClass('fa-check').addClass('fa-check-double')
                   .attr('title', 'Read');
        });
    }
    
    loadUnreadCount() {
        $.get('/Messaging/GetUnreadCount')
        .done((response) => {
            if (response.success) {
                this.updateUnreadBadge(response.count);
            }
        });
    }
    
    updateUnreadBadge(count) {
        const badge = $('.messages-unread-badge');
        if (count > 0) {
            badge.text(count).show();
        } else {
            badge.hide();
        }
    }
    
    refreshConversations() {
        // Reload conversations list
        $.get('/Messaging')
        .done((html) => {
            const newConversations = $(html).find('#conversationsList').html();
            $('#conversationsList').html(newConversations);
        });
    }
    
    openChat(userId) {
        // Stay on the same page and load chat via AJAX
        if (typeof loadChat === 'function') {
            // Find user name from conversations or staff list
            var conversation = $(`.conversation-item[data-user-id="${userId}"]`);
            if (conversation.length > 0) {
                var userName = conversation.find('.conversation-name').text();
                loadChat(userId, userName);
            } else {
                // Load from staff list if not in conversations
                this.loadStaffList().then(() => {
                    var staffMember = this.staffList.find(s => s.id == userId);
                    if (staffMember) {
                        loadChat(userId, staffMember.fullName);
                    }
                });
            }
        }
    }
    
    startChat(userId, userName) {
        $('#staffListModal').modal('hide');
        this.openChat(userId);
    }
    
    showStaffList() {
        $('#staffListModal').modal('show');
        this.loadStaffList();
    }
    
    loadStaffList() {
        $('#staffListContainer').html(this.getLoadingHtml());
        
        $.get('/Messaging/StaffList')
        .done((response) => {
            if (response.success) {
                const html = this.createStaffListHtml(response.users);
                $('#staffListContainer').html(html);
            } else {
                $('#staffListContainer').html('<div class="alert alert-danger">Failed to load staff list</div>');
            }
        })
        .fail(() => {
            $('#staffListContainer').html('<div class="alert alert-danger">Error loading staff list</div>');
        });
    }
    
    createStaffListHtml(users) {
        let html = '';
        users.forEach((user) => {
            // Ẩn customer nếu là admin, ẩn admin nếu là customer
            if ((window.isAdmin === 'true' && user.Role === 'Customer') || (window.isCustomer === 'true' && user.Role === 'Admin')) return;
            html += `
                <div class="staff-item" data-user-id="${user.Id}" data-user-name="${user.UserName}">
                    <div class="d-flex align-items-center">
                        <div class="avatar-circle bg-secondary text-white me-3">
                            <i class="fas fa-user"></i>
                        </div>
                        <div class="flex-grow-1">
                            <h6 class="mb-1">${this.escapeHtml(user.UserName)}</h6>
                            <div class="d-flex align-items-center gap-2">
                                <small class="text-muted">${this.escapeHtml(user.Email)}</small>
                                <span class="role-badge bg-${user.Role === 'Admin' ? 'danger' : 'primary'} text-white">
                                    ${user.Role}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        });
        return html;
    }
    
    getLoadingHtml() {
        return `
            <div class="text-center py-3">
                <div class="loading-spinner mx-auto mb-2"></div>
                <p class="text-muted">Loading staff members...</p>
            </div>
        `;
    }
    
    setSendingState(isSending) {
        const sendButton = $('#sendButton');
        const messageInput = $('#messageInput');
        
        if (isSending) {
            sendButton.prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i>');
            messageInput.prop('disabled', true);
        } else {
            sendButton.prop('disabled', false).html('<i class="fas fa-paper-plane"></i>');
            messageInput.prop('disabled', false).focus();
        }
    }
    
    handleTyping() {
        if (!this.isTyping) {
            this.isTyping = true;
            // Send typing indicator to server (future enhancement)
        }
        
        clearTimeout(this.typingTimeout);
        this.typingTimeout = setTimeout(() => {
            this.isTyping = false;
            // Stop typing indicator (future enhancement)
        }, 1000);
    }
    
    showError(message) {
        // Create toast notification
        const toast = $(`
            <div class="toast align-items-center text-white bg-danger border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `);
        
        $('.toast-container').append(toast);
        const bsToast = new bootstrap.Toast(toast[0]);
        bsToast.show();
        
        // Remove toast after it's hidden
        toast.on('hidden.bs.toast', () => {
            toast.remove();
        });
    }
    
    showMessageStatus(status) {
        // Show temporary status message
        const statusText = status === 'sent' ? 'Message sent' : 'Message delivered';
        // Implementation for status display (future enhancement)
    }
    
    playNotificationSound() {
        // Play notification sound (future enhancement)
        try {
            const audio = new Audio('/sounds/notification.mp3');
            audio.volume = 0.3;
            audio.play().catch(() => {
                // Ignore audio play errors
            });
        } catch (e) {
            // Ignore audio errors
        }
    }
    
    updateLastMessageTime() {
        this.lastMessageTime = new Date();
    }
    
    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
    
    destroy() {
        if (this.refreshInterval) {
            clearInterval(this.refreshInterval);
        }
        if (this.typingTimeout) {
            clearTimeout(this.typingTimeout);
        }
    }
}

// Initialize messaging system when document is ready
$(document).ready(() => {
    window.messagingSystem = new MessagingSystem();
    
    // Set current user ID and receiver ID from page data
    if (typeof currentUserId !== 'undefined') {
        window.messagingSystem.currentUserId = currentUserId;
    }
    if (typeof receiverId !== 'undefined') {
        window.messagingSystem.receiverId = receiverId;
    }
    
    // Set last message time
    window.messagingSystem.updateLastMessageTime();
});

// Global functions for backward compatibility
function showStaffList() {
    window.messagingSystem.showStaffList();
}

function openChat(userId) {
    window.messagingSystem.openChat(userId);
}

function startChat(userId, userName) {
    window.messagingSystem.startChat(userId, userName);
}
