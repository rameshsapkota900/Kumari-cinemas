// KumariCinemas Management System — Modern UI JavaScript
// CC6012NT Data and Web Development | Ramesh Sapkota (23049378)

document.addEventListener('DOMContentLoaded', function () {

    // ─── Auto-dismiss alerts after 4 seconds ───
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            bsAlert.close();
        }, 4000);
    });

    // ─── Sidebar Toggle ───
    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('sidebarToggle');
    const mainContent = document.querySelector('.main-content');

    // Create overlay for mobile
    const overlay = document.createElement('div');
    overlay.className = 'sidebar-overlay';
    document.body.appendChild(overlay);

    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            const isDesktop = window.innerWidth > 992;
            if (isDesktop) {
                sidebar.classList.toggle('collapsed');
                mainContent.classList.toggle('expanded');
            } else {
                sidebar.classList.toggle('open');
                overlay.classList.toggle('active');
            }
        });
    }

    overlay.addEventListener('click', function () {
        sidebar.classList.remove('open');
        overlay.classList.remove('active');
    });

    // ─── Active Sidebar Link ───
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.sidebar-link').forEach(function (link) {
        const href = link.getAttribute('href');
        if (href && currentPath === href.toLowerCase()) {
            link.classList.add('active');
        } else if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            link.classList.add('active');
        }
    });

    // ─── Fade-up animation on scroll (IntersectionObserver) ───
    const fadeEls = document.querySelectorAll('.card, .form-section, .filter-section, .page-header');
    fadeEls.forEach(function (el) {
        el.classList.add('fade-up');
    });

    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.style.animationPlayState = 'running';
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.05 });

        fadeEls.forEach(function (el) {
            el.style.animationPlayState = 'paused';
            observer.observe(el);
        });
    }

    // ─── Smooth number counter for stat values ───
    document.querySelectorAll('.stat-value').forEach(function (el) {
        const text = el.textContent.trim();
        const num = parseFloat(text.replace(/[^0-9.]/g, ''));
        if (!isNaN(num) && num > 0) {
            const prefix = text.replace(/[0-9.,]+.*/, '');
            const suffix = text.replace(/.*[0-9]/, '').replace(/^[.,]+/, '');
            const isDecimal = text.includes('.');
            const duration = 1200;
            const steps = 40;
            const stepTime = duration / steps;
            let current = 0;
            const increment = num / steps;
            el.textContent = prefix + '0' + suffix;
            const timer = setInterval(function () {
                current += increment;
                if (current >= num) {
                    current = num;
                    clearInterval(timer);
                }
                el.textContent = prefix + (isDecimal ? current.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',') : Math.round(current).toLocaleString()) + suffix;
            }, stepTime);
        }
    });
});
