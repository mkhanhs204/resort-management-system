/* ================================================================
   Room Service — Client-side JavaScript
   ================================================================ */

(function () {
    'use strict';

    // -- Menu Search Filter (client-side) -------------------------
    function initSearch() {
        var searchInput = document.getElementById('rs-search-input');
        if (!searchInput) return;

        searchInput.addEventListener('input', function () {
            var query = this.value.toLowerCase().trim();
            var cards = document.querySelectorAll('.rs-menu-card[data-item-name]');

            cards.forEach(function (card) {
                var name = (card.getAttribute('data-item-name') || '').toLowerCase();
                var category = (card.getAttribute('data-category') || '').toLowerCase();
                var matchesSearch = !query || name.indexOf(query) !== -1 || category.indexOf(query) !== -1;

                // Also check active tab filter
                var activeTab = document.querySelector('.rs-category-tab.active');
                var activeCategory = activeTab ? activeTab.getAttribute('data-category') : 'all';
                var matchesCategory = activeCategory === 'all' || category === activeCategory.toLowerCase();

                card.style.display = (matchesSearch && matchesCategory) ? '' : 'none';
            });

            updateSectionVisibility();
        });
    }

    // -- Category Tab Filter (client-side) ------------------------
    function initCategoryTabs() {
        var tabs = document.querySelectorAll('.rs-category-tab');
        if (!tabs.length) return;

        tabs.forEach(function (tab) {
            tab.addEventListener('click', function (e) {
                e.preventDefault();

                // Update active tab
                tabs.forEach(function (t) { t.classList.remove('active'); });
                this.classList.add('active');

                var category = this.getAttribute('data-category');
                var cards = document.querySelectorAll('.rs-menu-card[data-item-name]');
                var searchInput = document.getElementById('rs-search-input');
                var query = searchInput ? searchInput.value.toLowerCase().trim() : '';

                cards.forEach(function (card) {
                    var cardCategory = (card.getAttribute('data-category') || '').toLowerCase();
                    var name = (card.getAttribute('data-item-name') || '').toLowerCase();
                    var matchesCategory = category === 'all' || cardCategory === category.toLowerCase();
                    var matchesSearch = !query || name.indexOf(query) !== -1 || cardCategory.indexOf(query) !== -1;

                    card.style.display = (matchesCategory && matchesSearch) ? '' : 'none';
                });

                updateSectionVisibility();
            });
        });
    }

    // -- Update section titles visibility -------------------------
    function updateSectionVisibility() {
        var sections = document.querySelectorAll('.rs-section-title');
        sections.forEach(function (title) {
            var category = title.getAttribute('data-section-category');
            if (!category) return;
            var cards = document.querySelectorAll('.rs-menu-card[data-category="' + category + '"]');
            var anyVisible = false;
            cards.forEach(function (c) {
                if (c.style.display !== 'none') anyVisible = true;
            });
            title.style.display = anyVisible ? '' : 'none';
        });
    }

    // -- Quantity +/- Buttons ------------------------------------
    function initQuantityButtons() {
        document.addEventListener('click', function (e) {
            var btn = e.target.closest('.rs-qty-btn');
            if (!btn) return;

            var container = btn.closest('.rs-qty-selector');
            var input = container.querySelector('.rs-qty-value');
            var card = btn.closest('.rs-menu-card');
            var currentVal = parseInt(input.value) || 0;

            if (btn.classList.contains('qty-minus')) {
                if (currentVal > 0) input.value = currentVal - 1;
            } else if (btn.classList.contains('qty-plus')) {
                if (currentVal < 1000) input.value = currentVal + 1;
            }

            // Trigger input event for change detection
            input.dispatchEvent(new Event('input', { bubbles: true }));

            // Toggle card highlight
            if (card) {
                if (parseInt(input.value) > 0) {
                    card.classList.add('has-quantity');
                } else {
                    card.classList.remove('has-quantity');
                }
            }

            updateCartBar();
        });

        // Also listen for manual input changes
        document.addEventListener('input', function (e) {
            if (!e.target.classList.contains('rs-qty-value')) return;
            var val = parseInt(e.target.value) || 0;
            if (val < 0) e.target.value = 0;
            if (val > 1000) e.target.value = 1000;

            var card = e.target.closest('.rs-menu-card');
            if (card) {
                if (val > 0) {
                    card.classList.add('has-quantity');
                } else {
                    card.classList.remove('has-quantity');
                }
            }

            updateCartBar();
        });
    }

    // -- Sticky Cart Bar -----------------------------------------
    function updateCartBar() {
        var cartBar = document.getElementById('rs-cart-bar');
        if (!cartBar) return;

        var inputs = document.querySelectorAll('.rs-qty-value');
        var totalItems = 0;
        var subtotal = 0;

        inputs.forEach(function (input) {
            var qty = parseInt(input.value) || 0;
            if (qty > 0) {
                totalItems += qty;
                var price = parseFloat(input.getAttribute('data-price')) || 0;
                subtotal += qty * price;
            }
        });

        var countEl = cartBar.querySelector('.cart-count');
        var subtotalEl = cartBar.querySelector('.cart-subtotal');

        if (countEl) {
            countEl.innerHTML = '<strong>' + totalItems + '</strong> item' + (totalItems !== 1 ? 's' : '') + ' selected';
        }
        if (subtotalEl) {
            subtotalEl.textContent = formatCurrency(subtotal) + ' VND';
        }

        if (totalItems > 0) {
            cartBar.classList.add('visible');
        } else {
            cartBar.classList.remove('visible');
        }
    }

    // -- Format Currency -----------------------------------------
    function formatCurrency(value) {
        return value.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
    }

    // -- Cancel Order Modal --------------------------------------
    function initCancelModal() {
        document.addEventListener('click', function (e) {
            var cancelBtn = e.target.closest('[data-cancel-order]');
            if (!cancelBtn) return;
            e.preventDefault();

            var modal = document.getElementById('rs-cancel-modal');
            if (!modal) return;

            var formAction = cancelBtn.getAttribute('data-cancel-action');
            var form = modal.querySelector('form');
            if (form && formAction) {
                form.setAttribute('action', formAction);
            }

            modal.classList.add('show');
        });

        document.addEventListener('click', function (e) {
            if (e.target.closest('[data-dismiss-modal]')) {
                var modal = e.target.closest('.rs-modal-backdrop');
                if (modal) modal.classList.remove('show');
            }
            if (e.target.classList.contains('rs-modal-backdrop')) {
                e.target.classList.remove('show');
            }
        });
    }

    // -- Filter Tabs (My Orders) ---------------------------------
    function initFilterTabs() {
        var tabs = document.querySelectorAll('.rs-filter-tab');
        if (!tabs.length) return;

        tabs.forEach(function (tab) {
            tab.addEventListener('click', function () {
                tabs.forEach(function (t) { t.classList.remove('active'); });
                this.classList.add('active');

                var filter = this.getAttribute('data-filter');
                var cards = document.querySelectorAll('.rs-history-card');

                cards.forEach(function (card) {
                    var status = card.getAttribute('data-status');
                    if (filter === 'all' || status === filter) {
                        card.style.display = '';
                    } else {
                        card.style.display = 'none';
                    }
                });

                // Update empty state
                var visibleCards = document.querySelectorAll('.rs-history-card:not([style*="display: none"])');
                var emptyState = document.getElementById('rs-empty-filtered');
                if (emptyState) {
                    emptyState.style.display = visibleCards.length === 0 ? '' : 'none';
                }
            });
        });
    }

    // -- Init on page load ----------------------------------------
    document.addEventListener('DOMContentLoaded', function () {
        initSearch();
        initCategoryTabs();
        initQuantityButtons();
        initCancelModal();
        initFilterTabs();
        updateCartBar();

        // Initialize card highlights for pre-populated quantities
        document.querySelectorAll('.rs-qty-value').forEach(function (input) {
            var val = parseInt(input.value) || 0;
            var card = input.closest('.rs-menu-card');
            if (card && val > 0) {
                card.classList.add('has-quantity');
            }
        });
    });
})();
