class MessengerWidget {
    constructor() {
        this.isOpen = false;
        this.openChats = new Map();
        this.connection = null;
        this.currentUserId = null;
        this.conversations = [];
        this.init();
    }

    init() {
        this.getCurrentUserId();
        this.setupEventListeners();
        this.initSignalR();
        this.loadConversations();
        this.startAutoRefresh();
    }

    async getCurrentUserId() {
        try {
            const response = await fetch('/api/messaging/current-user');
            const data = await response.json();
            this.currentUserId = data.userId;
        } catch (error) {
            console.error('Error getting current user:', error);
        }
    }

    setupEventListeners() {
        // Toggle chat list
        document.getElementById('messengerButton').addEventListener('click', () => {
            this.toggleChatList();
        });

        // Close chat list when clicking outside
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.messenger-widget')) {
                this.closeChatList();
            }
        });

        // Tab switching
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.switchTab(e.target.dataset.tab);
            });
        });

        // Search functionality
        document.querySelector('.search-input').addEventListener('input', (e) => {
            this.searchConversations(e.target.value);
        });
    }

    toggleChatList() {
        const popup = document.getElementById('chatListPopup');
        this.isOpen = !this.isOpen;
        
        if (this.isOpen) {
            popup.style.display = 'flex';
            this.loadConversations();
        } else {
            popup.style.display = 'none';
        }
    }

    closeChatList() {
        document.getElementById('chatListPopup').style.display = 'none';
        this.isOpen = false;
    }

    switchTab(tab) {
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.classList.remove('active');
        });
        document.querySelector(`[data-tab="${tab}"]`).classList.add('active');
        
        this.filterConversations(tab);
    }

    async loadConversations() {
        try {
            const response = await fetch('/api/messaging/conversations');
            const conversations = await response.json();
            this.conversations = conversations;
            this.renderConversations(conversations);
            this.updateUnreadCount();
        } catch (error) {
            console.error('Error loading conversations:', error);
        }
    }

    renderConversations(conversations) {
        const container = document.getElementById('popupConversations');
        
        if (conversations.length === 0) {
            container.innerHTML = `
                <div class="text-center p-4">
                    <i class="fab fa-facebook-messenger" style="font-size: 48px; color: #ccc; margin-bottom: 16px;"></i>
                    <p style="color: #65676b; font-size: 14px;">Chưa có cuộc trò chuyện nào</p>
                    <button class="btn btn-primary btn-sm" onclick="messengerWidget.showStaffList()">
                        Bắt đầu trò chuyện
                    </button>
                </div>
            `;
            return;
        }

        container.innerHTML = conversations.map(conv => `
            <div class="popup-conversation-item" onclick="messengerWidget.openChatWindow('${conv.userId}', '${conv.userName}')">
                <div class="popup-conversation-avatar">
                    <div class="popup-avatar-circle">
                        ${conv.userName.charAt(0).toUpperCase()}
                    </div>
                    ${conv.isOnline ? '<div class="popup-online-indicator"></div>' : ''}
                </div>
                <div class="popup-conversation-content">
                    <div class="popup-conversation-header">
                        <div class="popup-conversation-name">${conv.userName}</div>
                        <div class="popup-conversation-time">${this.formatTime(conv.lastMessageTime)}</div>
                    </div>
                    <div class="popup-conversation-preview">
                        ${conv.lastMessage || 'Bắt đầu cuộc trò chuyện...'}
                        ${conv.unreadCount > 0 ? '<div class="popup-unread-indicator"></div>' : ''}
                    </div>
                </div>
            </div>
        `).join('');
    }

    openChatWindow(userId, userName) {
        if (this.openChats.has(userId)) {
            // Focus existing chat window
            const chatWindow = document.getElementById(`chat-${userId}`);
            chatWindow.scrollIntoView({ behavior: 'smooth' });
            return;
        }

        this.createChatWindow(userId, userName);
        this.openChats.set(userId, { userId, userName });
        this.loadChatMessages(userId);
        this.closeChatList();
    }

    createChatWindow(userId, userName) {
        const chatWindows = document.getElementById('chatWindows');
        const chatWindow = document.createElement('div');
        chatWindow.className = 'chat-window';
        chatWindow.id = `chat-${userId}`;
        
        chatWindow.innerHTML = `
            <div class="chat-window-header">
                <div class="chat-window-user">
                    <div class="chat-window-avatar">
                        ${userName.charAt(0).toUpperCase()}
                    </div>
                    <div>
                        <div class="chat-window-name">${userName}</div>
                        <div style="font-size: 12px; opacity: 0.8;">Đang hoạt động</div>
                    </div>
                </div>
                <div class="chat-window-actions">
                    <button class="chat-window-btn" onclick="messengerWidget.closeChatWindow('${userId}')" title="Đóng">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            <div class="chat-window-messages" id="messages-${userId}">
                <!-- Messages will be loaded here -->
            </div>
            <div class="chat-window-input">
                <div class="chat-input-wrapper">
                    <input type="text" class="chat-input" placeholder="Aa" 
                           onkeypress="messengerWidget.handleKeyPress(event, '${userId}')"
                           id="input-${userId}">
                    <button class="chat-send-btn" onclick="messengerWidget.sendMessage('${userId}')">
                        <i class="fas fa-paper-plane"></i>
                    </button>
                </div>
            </div>
        `;
        
        chatWindows.appendChild(chatWindow);
    }

    closeChatWindow(userId) {
        const chatWindow = document.getElementById(`chat-${userId}`);
        if (chatWindow) {
            chatWindow.remove();
            this.openChats.delete(userId);
        }
    }









    async loadChatMessages(userId) {
        try {
            const response = await fetch(`/api/messaging/messages/${userId}`);
            const messages = await response.json();
            this.renderMessages(userId, messages);
        } catch (error) {
            console.error('Error loading messages:', error);
        }
    }

    renderMessages(userId, messages) {
        const container = document.getElementById(`messages-${userId}`);
        
        container.innerHTML = messages.map(msg => `
            <div class="message-bubble ${msg.senderId == this.currentUserId ? 'message-sent' : 'message-received'}">
                ${msg.content}
            </div>
        `).join('');
        
        container.scrollTop = container.scrollHeight;
    }

    handleKeyPress(event, userId) {
        if (event.key === 'Enter') {
            this.sendMessage(userId);
        }
    }

    async sendMessage(userId) {
        const input = document.getElementById(`input-${userId}`);
        const message = input.value.trim();
        
        if (!message) return;

        try {
            const response = await fetch('/api/messaging/send', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    receiverId: userId,
                    content: message
                })
            });

            if (response.ok) {
                input.value = '';
                this.loadChatMessages(userId);
                // Refresh conversations to move this conversation to top
                this.loadConversations();
            }
        } catch (error) {
            console.error('Error sending message:', error);
        }
    }

    async showStaffList() {
        try {
            const response = await fetch('/api/messaging/staff');
            const staff = await response.json();
            
            // Create staff list modal
            const modal = document.createElement('div');
            modal.className = 'modal fade';
            modal.innerHTML = `
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Chọn nhân viên để trò chuyện</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            ${staff.map(s => `
                                <div class="d-flex align-items-center p-2 border-bottom cursor-pointer" 
                                     onclick="messengerWidget.startNewChat('${s.id}', '${s.fullName}')">
                                    <div class="avatar-circle me-3">
                                        ${s.fullName.charAt(0).toUpperCase()}
                                    </div>
                                    <div>
                                        <div class="fw-bold">${s.fullName}</div>
                                        <small class="text-muted">${s.email}</small>
                                    </div>
                                </div>
                            `).join('')}
                        </div>
                    </div>
                </div>
            `;
            
            document.body.appendChild(modal);
            const bsModal = new bootstrap.Modal(modal);
            bsModal.show();
            
            modal.addEventListener('hidden.bs.modal', () => {
                modal.remove();
            });
        } catch (error) {
            console.error('Error loading staff:', error);
        }
    }

    startNewChat(userId, userName) {
        const modal = bootstrap.Modal.getInstance(document.querySelector('.modal'));
        modal.hide();
        this.openChatWindow(userId, userName);
    }

    filterConversations(filter) {
        let filtered = this.conversations;
        
        switch (filter) {
            case 'unread':
                filtered = this.conversations.filter(c => c.unreadCount > 0);
                break;
            case 'groups':
                filtered = []; // No groups for now
                break;
        }
        
        this.renderConversations(filtered);
    }

    searchConversations(query) {
        if (!query) {
            this.renderConversations(this.conversations);
            return;
        }
        
        const filtered = this.conversations.filter(c => 
            c.userName.toLowerCase().includes(query.toLowerCase()) ||
            (c.lastMessage && c.lastMessage.toLowerCase().includes(query.toLowerCase()))
        );
        
        this.renderConversations(filtered);
    }

    updateUnreadCount() {
        const totalUnread = this.conversations.reduce((sum, conv) => sum + conv.unreadCount, 0);
        const badge = document.getElementById('widgetUnreadCount');
        
        if (totalUnread > 0) {
            badge.textContent = totalUnread > 99 ? '99+' : totalUnread;
            badge.style.display = 'flex';
        } else {
            badge.style.display = 'none';
        }
    }

    formatTime(dateString) {
        if (!dateString) return '';
        
        const date = new Date(dateString);
        const now = new Date();
        const diff = now - date;
        
        if (diff < 60000) return 'Vừa xong';
        if (diff < 3600000) return Math.floor(diff / 60000) + ' phút';
        if (diff < 86400000) return Math.floor(diff / 3600000) + ' giờ';
        
        return date.toLocaleDateString('vi-VN');
    }

    async initSignalR() {
        try {
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/messagingHub")
                .build();

            this.connection.start().then(() => {
                console.log('SignalR Connected for widget');
            });

            this.connection.on("ReceiveMessage", (senderId, senderName, message) => {
                this.handleNewMessage(senderId, senderName, message);
            });



        } catch (error) {
            console.error('SignalR connection error:', error);
        }
    }

    handleNewMessage(senderId, senderName, message) {
        // Update chat window if open
        if (this.openChats.has(senderId)) {
            this.loadChatMessages(senderId);
        }

        // Update conversations list immediately to move conversation to top
        this.loadConversations();

        // Show notification if chat window is not open
        if (!this.openChats.has(senderId)) {
            this.showNotification(senderName, message);
        }
    }

    showNotification(senderName, message) {
        if (Notification.permission === 'granted') {
            new Notification(`${senderName}`, {
                body: message,
                icon: '/favicon.ico'
            });
        }
    }

    startAutoRefresh() {
        setInterval(() => {
            this.loadConversations();
        }, 30000); // Refresh every 30 seconds
    }


}

// Initialize widget when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.messengerWidget = new MessengerWidget();
    
    // Request notification permission
    if ('Notification' in window && Notification.permission === 'default') {
        Notification.requestPermission();
    }
});
