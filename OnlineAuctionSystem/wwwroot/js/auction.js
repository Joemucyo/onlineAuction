/* ═══════════════════════════════════════════
   GAVELPRO AUCTIONS — MAIN JAVASCRIPT
═══════════════════════════════════════════ */

document.addEventListener('DOMContentLoaded', () => {

    /* ──────────────────────────────────────
       1. STICKY NAV — add class on scroll
    ────────────────────────────────────── */
    const nav = document.getElementById('mainNav');
    const onScroll = () => {
        nav?.classList.toggle('scrolled', window.scrollY > 40);
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();


    /* ──────────────────────────────────────
       2. MOBILE NAV TOGGLE
    ────────────────────────────────────── */
    const navToggle = document.getElementById('navToggle');
    const mobileMenu = document.getElementById('mobileMenu');
    navToggle?.addEventListener('click', () => {
        mobileMenu?.classList.toggle('open');
    });
    // Close on link click
    mobileMenu?.querySelectorAll('a').forEach(a => {
        a.addEventListener('click', () => mobileMenu.classList.remove('open'));
    });


    /* ──────────────────────────────────────
       3. COUNTDOWN TIMERS
       Data attribute: data-ends="ISO date"
    ────────────────────────────────────── */
    function formatTime(ms) {
        if (ms <= 0) return 'Ended';
        const s = Math.floor(ms / 1000);
        const h = Math.floor(s / 3600);
        const m = Math.floor((s % 3600) / 60);
        const sc = s % 60;
        const pad = n => String(n).padStart(2, '0');
        return h > 0
            ? `${pad(h)}h ${pad(m)}m ${pad(sc)}s`
            : `${pad(m)}m ${pad(sc)}s`;
    }

    function updateTimers() {
        document.querySelectorAll('[data-ends]').forEach(el => {
            const ends = new Date(el.dataset.ends).getTime();
            const diff = ends - Date.now();
            el.textContent = formatTime(diff);
            if (diff <= 0) el.style.color = 'var(--text-ghost)';
        });
    }
    updateTimers();
    setInterval(updateTimers, 1000);


    /* ──────────────────────────────────────
       4. COUNTING STAT NUMBERS
    ────────────────────────────────────── */
    function animateCount(el) {
        const target = parseInt(el.dataset.count, 10);
        const prefix = el.dataset.prefix || '';
        const suffix = el.dataset.suffix || '';
        const dur = 1600;   // ms
        const start = Date.now();

        (function tick() {
            const progress = Math.min((Date.now() - start) / dur, 1);
            // Ease out cubic
            const eased = 1 - Math.pow(1 - progress, 3);
            const val = Math.round(eased * target);
            el.textContent = prefix + val.toLocaleString() + suffix;
            if (progress < 1) requestAnimationFrame(tick);
        })();
    }

    const statNums = document.querySelectorAll('.stat-card__num[data-count]');
    if ('IntersectionObserver' in window) {
        const io = new IntersectionObserver((entries, obs) => {
            entries.forEach(e => {
                if (e.isIntersecting) {
                    animateCount(e.target);
                    obs.unobserve(e.target);
                }
            });
        }, { threshold: 0.5 });
        statNums.forEach(n => io.observe(n));
    } else {
        statNums.forEach(animateCount);
    }


    /* ──────────────────────────────────────
       5. FILTER TABS (All / Live / Upcoming)
    ────────────────────────────────────── */
    const tabs = document.querySelectorAll('.filter-tab');
    const cards = document.querySelectorAll('.auction-card');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            const filter = tab.dataset.filter;
            cards.forEach(card => {
                const show = filter === 'all' || card.dataset.status === filter;
                card.style.display = show ? 'flex' : 'none';
            });
        });
    });


    /* ──────────────────────────────────────
       6. SMOOTH SCROLL for anchor links
    ────────────────────────────────────── */
    document.querySelectorAll('a[href^="#"]').forEach(a => {
        a.addEventListener('click', e => {
            const id = a.getAttribute('href').slice(1);
            const el = document.getElementById(id);
            if (el) {
                e.preventDefault();
                el.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    });


    /* ──────────────────────────────────────
       7. FADE-IN ON SCROLL (auction cards)
    ────────────────────────────────────── */
    const fadeEls = document.querySelectorAll(
        '.auction-card, .featured-card, .step, .cat-card'
    );
    if ('IntersectionObserver' in window) {
        const fadeOpts = { threshold: 0.1, rootMargin: '0px 0px -40px 0px' };
        const fadeObs = new IntersectionObserver((entries) => {
            entries.forEach(e => {
                if (e.isIntersecting) {
                    e.target.style.animation = 'fadeUp 0.5s ease both';
                    fadeObs.unobserve(e.target);
                }
            });
        }, fadeOpts);

        fadeEls.forEach((el, i) => {
            el.style.opacity = '0';
            // Stagger by column position
            setTimeout(() => fadeObs.observe(el), i * 40);
        });
    }

});
