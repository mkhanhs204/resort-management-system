/**
 * Phúc Nguyên Resort — In-App Notification System
 * Connects to SignalR hub and manages the notification bell UI.
 */
(function () {
    'use strict';

    // ─── State ───────────────────────────────────
    let connection = null;
    let isDropdownOpen = false;
    const MAX_DISPLAY = 10;

    // ─── DOM References ──────────────────────────
    const bellBtn = document.getElementById('notification-bell');
    const bellBadge = document.getElementById('notification-badge');
    const dropdown = document.getElementById('notification-dropdown');
    const listContainer = document.getElementById('notification-list');
    const emptyState = document.getElementById('notification-empty');
    const markAllBtn = document.getElementById('notification-mark-all');

    if (!bellBtn) return; // User not authenticated or bell not rendered

    // ─── Initialize ──────────────────────────────
    fetchNotifications();
    initSignalR();

    // ─── Event Listeners ─────────────────────────
    bellBtn.addEventListener('click', function (e) {
        e.stopPropagation();
        toggleDropdown();
    });

    document.addEventListener('click', function (e) {
        if (isDropdownOpen && dropdown && !dropdown.contains(e.target) && !bellBtn.contains(e.target)) {
            closeDropdown();
        }
    });

    if (markAllBtn) {
        markAllBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            markAllAsRead();
        });
    }

    // ─── SignalR Connection ──────────────────────
    function initSignalR() {
        if (typeof signalR === 'undefined') {
            console.warn('[Notifications] SignalR library not loaded.');
            return;
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/notifications')
            .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
            .build();

        connection.on('ReceiveNotification', function (data) {
            // Prepend new notification to dropdown
            prependNotification(data);
            updateBadge(data.unreadCount);
            // Brief visual pulse on bell
            bellBtn.classList.add('animate-bounce');
            setTimeout(function () { bellBtn.classList.remove('animate-bounce'); }, 1000);
        });

        connection.start().catch(function (err) {
            console.warn('[Notifications] SignalR connection failed:', err);
        });
    }

    // ─── API Calls ───────────────────────────────
    function fetchNotifications() {
        fetch('/api/notifications?take=' + MAX_DISPLAY, {
            credentials: 'same-origin'
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                renderNotifications(data.notifications || []);
                updateBadge(data.unreadCount || 0);
            })
            .catch(function (err) {
                console.warn('[Notifications] Fetch error:', err);
            });
    }

    function markAsRead(notificationId, el) {
        fetch('/api/notifications/' + notificationId + '/read', {
            method: 'POST',
            credentials: 'same-origin'
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                if (el) el.classList.remove('bg-primary-fixed/10');
                if (el) el.classList.add('bg-white');
                updateBadge(data.unreadCount);
            })
            .catch(function () { });
    }

    function markAllAsRead() {
        fetch('/api/notifications/read-all', {
            method: 'POST',
            credentials: 'same-origin'
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                updateBadge(0);
                // Update all items visually
                if (listContainer) {
                    listContainer.querySelectorAll('.notification-item').forEach(function (el) {
                        el.classList.remove('bg-primary-fixed/10');
                        el.classList.add('bg-white');
                    });
                }
            })
            .catch(function () { });
    }

    // ─── Rendering ───────────────────────────────
    function renderNotifications(notifications) {
        if (!listContainer) return;
        listContainer.innerHTML = '';

        if (!notifications.length) {
            if (emptyState) emptyState.classList.remove('hidden');
            return;
        }

        if (emptyState) emptyState.classList.add('hidden');

        notifications.forEach(function (n) {
            listContainer.appendChild(createNotificationElement(n));
        });
    }

    function prependNotification(n) {
        if (!listContainer) return;
        if (emptyState) emptyState.classList.add('hidden');

        var el = createNotificationElement(n);
        // Fade-in animation
        el.style.opacity = '0';
        el.style.transform = 'translateY(-10px)';
        listContainer.insertBefore(el, listContainer.firstChild);
        requestAnimationFrame(function () {
            el.style.transition = 'all 0.3s ease';
            el.style.opacity = '1';
            el.style.transform = 'translateY(0)';
        });

        // Limit to MAX_DISPLAY
        while (listContainer.children.length > MAX_DISPLAY) {
            listContainer.removeChild(listContainer.lastChild);
        }
    }

    function createNotificationElement(n) {
        var div = document.createElement('div');
        div.className = 'notification-item px-4 py-3 cursor-pointer transition-colors duration-200 hover:bg-surface-container border-b border-outline-variant/10 last:border-b-0 ' +
            (n.isRead ? 'bg-white' : 'bg-primary-fixed/10');
        div.setAttribute('data-id', n.notificationId);

        div.innerHTML =
            '<div class="flex gap-3 items-start">' +
            '<div class="w-8 h-8 rounded-full bg-primary-container/30 flex items-center justify-center flex-shrink-0 mt-0.5">' +
            '<span class="material-symbols-outlined text-primary text-[16px]">notifications</span>' +
            '</div>' +
            '<div class="flex-1 min-w-0">' +
            '<p class="text-[13px] font-semibold text-on-surface leading-tight truncate">' + escapeHtml(n.title || '') + '</p>' +
            '<p class="text-[12px] text-on-surface-variant mt-0.5 line-clamp-2">' + escapeHtml(n.message || '') + '</p>' +
            '<p class="text-[11px] text-on-surface-variant/60 mt-1">' + escapeHtml(n.sentAtRelative || n.sentAt || '') + '</p>' +
            '</div>' +
            (!n.isRead ? '<div class="w-2 h-2 rounded-full bg-primary flex-shrink-0 mt-2"></div>' : '') +
            '</div>';

        div.addEventListener('click', function () {
            if (!n.isRead) {
                markAsRead(n.notificationId, div);
                n.isRead = true;
            }
        });

        return div;
    }

    // ─── UI Helpers ──────────────────────────────
    function updateBadge(count) {
        if (!bellBadge) return;
        if (count > 0) {
            bellBadge.textContent = count > 99 ? '99+' : count;
            bellBadge.classList.remove('hidden');
        } else {
            bellBadge.classList.add('hidden');
        }
    }

    function toggleDropdown() {
        if (isDropdownOpen) {
            closeDropdown();
        } else {
            openDropdown();
        }
    }

    function openDropdown() {
        if (!dropdown) return;
        dropdown.classList.remove('hidden', 'opacity-0', 'scale-95');
        dropdown.classList.add('opacity-100', 'scale-100');
        isDropdownOpen = true;

        // Auto mark all as read and hide badge when dropdown is opened
        if (bellBadge && !bellBadge.classList.contains('hidden')) {
            markAllAsRead();
        }
    }

    function closeDropdown() {
        if (!dropdown) return;
        dropdown.classList.add('opacity-0', 'scale-95');
        setTimeout(function () {
            dropdown.classList.add('hidden');
        }, 200);
        isDropdownOpen = false;
    }

    function escapeHtml(text) {
        var div = document.createElement('div');
        div.appendChild(document.createTextNode(text));
        return div.innerHTML;
    }
})();
