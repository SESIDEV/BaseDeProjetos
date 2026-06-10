'use strict';

/* ===== Enable Bootstrap helpers ====== */
if (window.bootstrap) {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new window.bootstrap.Popover(popoverTriggerEl);
    });

    var alertList = document.querySelectorAll('.alert');
    alertList.forEach(function (alert) {
        new window.bootstrap.Alert(alert);
    });
}

/* ===== Responsive Sidepanel ====== */
const sidePanelToggler = document.getElementById('sidepanel-toggler');
const sidePanel = document.getElementById('app-sidepanel');
const sidePanelDrop = document.getElementById('sidepanel-drop');
const sidePanelClose = document.getElementById('sidepanel-close');
const sidePanelDesktopToggle = document.getElementById('sidepanel-desktop-toggle');
const sidebarCollapsedStorageKey = 'sgi-sidebar-collapsed';

window.addEventListener('load', function () {
    responsiveSidePanel();
});

window.addEventListener('resize', function () {
    responsiveSidePanel();
});

function responsiveSidePanel() {
    if (!sidePanel) {
        return;
    }

    let w = window.innerWidth;
    if (w >= 1200) {
        // if larger
        //console.log('larger');
        sidePanel.classList.remove('sidepanel-hidden');
        sidePanel.classList.add('sidepanel-visible');
        applyDesktopSidebarState();
    } else {
        // if smaller
        //console.log('smaller');
        document.body.classList.remove('sidebar-collapsed');
        sidePanel.classList.remove('sidepanel-visible');
        sidePanel.classList.add('sidepanel-hidden');
    }
};

function applyDesktopSidebarState() {
    if (!sidePanelDesktopToggle) {
        return;
    }

    const collapsed = localStorage.getItem(sidebarCollapsedStorageKey) !== 'false';
    document.body.classList.toggle('sidebar-collapsed', collapsed);
    sidePanelDesktopToggle.setAttribute('aria-pressed', collapsed ? 'true' : 'false');
}

if (sidePanelToggler && sidePanel) {
    sidePanelToggler.addEventListener('click', () => {
        if (sidePanel.classList.contains('sidepanel-visible')) {
            sidePanel.classList.remove('sidepanel-visible');
            sidePanel.classList.add('sidepanel-hidden');
        } else {
            sidePanel.classList.remove('sidepanel-hidden');
            sidePanel.classList.add('sidepanel-visible');
        }
    });
}

if (sidePanelClose && sidePanelToggler) {
    sidePanelClose.addEventListener('click', (e) => {
        e.preventDefault();
        sidePanelToggler.click();
    });
}

if (sidePanelDrop && sidePanelToggler) {
    sidePanelDrop.addEventListener('click', (e) => {
        sidePanelToggler.click();
    });
}

if (sidePanelDesktopToggle) {
    sidePanelDesktopToggle.addEventListener('click', () => {
        const collapsed = !document.body.classList.contains('sidebar-collapsed');
        document.body.classList.toggle('sidebar-collapsed', collapsed);
        sidePanelDesktopToggle.setAttribute('aria-pressed', collapsed ? 'true' : 'false');
        localStorage.setItem(sidebarCollapsedStorageKey, collapsed ? 'true' : 'false');
    });
}

if (sidePanel) {
    sidePanel.addEventListener('click', (event) => {
        if (window.innerWidth < 1200 || !document.body.classList.contains('sidebar-collapsed')) {
            return;
        }

        if (event.target.closest('#sidepanel-desktop-toggle')) {
            return;
        }

        const clickedInsidePanel = event.target.closest('.sidepanel-inner');
        if (!clickedInsidePanel) {
            return;
        }

        event.preventDefault();
        document.body.classList.remove('sidebar-collapsed');
        sidePanelDesktopToggle?.setAttribute('aria-pressed', 'false');
        localStorage.setItem(sidebarCollapsedStorageKey, 'false');
    });
}

/* ====== Mobile search ======= */
const searchMobileTrigger = document.querySelector('.search-mobile-trigger');
const searchBox = document.querySelector('.app-search-box');

if (searchMobileTrigger && searchBox) {
    searchMobileTrigger.addEventListener('click', () => {
        searchBox.classList.toggle('is-visible');

        let searchMobileTriggerIcon = document.querySelector('.search-mobile-trigger-icon');

        if (!searchMobileTriggerIcon) {
            return;
        }

        if (searchMobileTriggerIcon.classList.contains('fa-search')) {
            searchMobileTriggerIcon.classList.remove('fa-search');
            searchMobileTriggerIcon.classList.add('fa-times');
        } else {
            searchMobileTriggerIcon.classList.remove('fa-times');
            searchMobileTriggerIcon.classList.add('fa-search');
        }
    });
}
