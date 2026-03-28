/* GavelPro — scripts used with Bootstrap */

document.addEventListener('DOMContentLoaded', () => {

    const nav = document.getElementById('mainNav');
    const onScroll = () => {
        nav?.classList.toggle('shadow', window.scrollY > 8);
    };
    window.addEventListener('scroll', onScroll, { passive: true });
    onScroll();

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
            if (diff <= 0) {
                el.classList.add('text-muted');
                el.classList.remove('text-warning', 'text-danger');
            } else {
                el.classList.remove('text-muted');
                if (el.classList.contains('timer')) {
                    el.classList.add('text-warning');
                }
            }
        });
    }
    updateTimers();
    setInterval(updateTimers, 1000);

    function animateCount(el) {
        const target = parseInt(el.dataset.count, 10);
        if (Number.isNaN(target)) return;
        const prefix = el.dataset.prefix || '';
        const suffix = el.dataset.suffix || '';
        const dur = 1600;
        const start = Date.now();

        (function tick() {
            const progress = Math.min((Date.now() - start) / dur, 1);
            const eased = 1 - Math.pow(1 - progress, 3);
            const val = Math.round(eased * target);
            el.textContent = prefix + val.toLocaleString() + suffix;
            if (progress < 1) requestAnimationFrame(tick);
        })();
    }

    const statNums = document.querySelectorAll('.js-stat-num[data-count]');
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

    const tabs = document.querySelectorAll('.filter-tab');
    const gridItems = document.querySelectorAll('#auctionGrid .auction-grid-item');

    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            const filter = tab.dataset.filter;
            gridItems.forEach(item => {
                const status = item.dataset.status;
                const show = filter === 'all' || status === filter || status === 'all';
                item.classList.toggle('d-none', !show);
            });
        });
    });

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

    const fadeEls = document.querySelectorAll(
        '.featured-card, .step, .cat-card, #auctionGrid .auction-grid-item'
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
            setTimeout(() => fadeObs.observe(el), i * 40);
        });
    }
});
