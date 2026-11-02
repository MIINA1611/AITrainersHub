(function () {
    "use strict";

    var toast = document.getElementById("usersToast");
    var toastTimer = null;

    function showToast(message) {
        if (!toast || !message) {
            return;
        }

        toast.textContent = message;
        toast.classList.remove("hidden");

        if (toastTimer) {
            window.clearTimeout(toastTimer);
        }

        toastTimer = window.setTimeout(function () {
            toast.classList.add("hidden");
        }, 3000);
    }

    window.usersToast = showToast;

    /* ----------------------------------------------------------------------
       Mobile navigation drawer
       ---------------------------------------------------------------------- */

    var drawer = document.getElementById("usersDrawer");
    var drawerToggle = document.getElementById("usersDrawerToggle");
    var menuIcon = document.getElementById("usersMenuIcon");
    var closeIcon = document.getElementById("usersCloseIcon");

    function setDrawer(open) {
        if (!drawer) {
            return;
        }

        drawer.classList.toggle("hidden", !open);
        document.body.style.overflow = open ? "hidden" : "";

        if (menuIcon) {
            menuIcon.classList.toggle("hidden", open);
        }

        if (closeIcon) {
            closeIcon.classList.toggle("hidden", !open);
        }

        if (drawerToggle) {
            drawerToggle.setAttribute("aria-expanded", open ? "true" : "false");
        }
    }

    if (drawerToggle) {
        drawerToggle.addEventListener("click", function () {
            setDrawer(drawer ? drawer.classList.contains("hidden") : false);
        });
    }

    if (drawer) {
        drawer.querySelectorAll("[data-users-drawer-close]").forEach(function (element) {
            element.addEventListener("click", function () {
                setDrawer(false);
            });
        });

        drawer.querySelectorAll("a").forEach(function (link) {
            link.addEventListener("click", function () {
                setDrawer(false);
            });
        });
    }

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape") {
            setDrawer(false);
        }
    });

    /* ----------------------------------------------------------------------
       Placeholder actions left in legacy screens
       ---------------------------------------------------------------------- */

    document.addEventListener("click", function (event) {
        var trigger = event.target.closest("[data-placeholder]");

        if (!trigger) {
            return;
        }

        event.preventDefault();
        showToast(trigger.getAttribute("data-placeholder") || "Coming soon.");
    });

    /* ----------------------------------------------------------------------
       Password visibility toggles
       ---------------------------------------------------------------------- */

    document.querySelectorAll("[data-password-toggle]").forEach(function (button) {
        button.addEventListener("click", function () {
            var input = document.getElementById(button.getAttribute("data-password-toggle"));

            if (!input) {
                return;
            }

            var show = input.type === "password";
            input.type = show ? "text" : "password";
            button.setAttribute("aria-label", show ? "Hide password" : "Show password");

            var eye = button.querySelector("[data-eye-open]");
            var eyeOff = button.querySelector("[data-eye-off]");

            if (eye) {
                eye.classList.toggle("hidden", show);
            }

            if (eyeOff) {
                eyeOff.classList.toggle("hidden", !show);
            }
        });
    });

    /* ----------------------------------------------------------------------
       Confirmation for destructive forms
       ---------------------------------------------------------------------- */

    document.querySelectorAll("form[data-confirm]").forEach(function (form) {
        form.addEventListener("submit", function (event) {
            var message = form.getAttribute("data-confirm") || "Are you sure?";

            if (!window.confirm(message)) {
                event.preventDefault();
            }
        });
    });

    /* ----------------------------------------------------------------------
       Auto-submit selects and toggles
       ---------------------------------------------------------------------- */

    document.querySelectorAll("[data-auto-submit]").forEach(function (element) {
        element.addEventListener("change", function () {
            var form = element.closest("form");

            if (form) {
                form.submit();
            }
        });
    });

    /* ----------------------------------------------------------------------
       Debounced live search
       ---------------------------------------------------------------------- */

    var liveSearchTimer = null;

    document.querySelectorAll("[data-live-search]").forEach(function (input) {
        input.addEventListener("input", function () {
            var form = input.closest("form");

            if (!form) {
                return;
            }

            if (liveSearchTimer) {
                window.clearTimeout(liveSearchTimer);
            }

            liveSearchTimer = window.setTimeout(function () {
                form.submit();
            }, 450);
        });
    });

    /* ----------------------------------------------------------------------
       Tabs (Overview / Guides / Reviews / Opportunities)
       ---------------------------------------------------------------------- */

    function activateTab(group, name, updateHash) {
        if (!group) {
            return;
        }

        var buttons = Array.prototype.slice.call(group.querySelectorAll("[data-users-tab]"));
        var panels = Array.prototype.slice.call(group.querySelectorAll("[data-users-tab-panel]"));

        if (buttons.length === 0) {
            return;
        }

        var exists = buttons.some(function (button) {
            return button.getAttribute("data-users-tab") === name;
        });

        if (!exists) {
            name = buttons[0].getAttribute("data-users-tab");
        }

        buttons.forEach(function (button) {
            var active = button.getAttribute("data-users-tab") === name;
            button.classList.toggle("is-active", active);
            button.setAttribute("aria-selected", active ? "true" : "false");
        });

        panels.forEach(function (panel) {
            panel.classList.toggle("hidden", panel.getAttribute("data-users-tab-panel") !== name);
        });

        if (updateHash) {
            var nextUrl = name
                ? "#" + name
                : window.location.pathname + window.location.search;

            if (window.history && window.history.replaceState) {
                window.history.replaceState(null, "", nextUrl);
            }
        }
    }

    var tabGroups = document.querySelectorAll("[data-users-tab-group]");

    tabGroups.forEach(function (group) {
        var buttons = group.querySelectorAll("[data-users-tab]");

        buttons.forEach(function (button) {
            button.addEventListener("click", function () {
                activateTab(group, button.getAttribute("data-users-tab"), true);
            });
        });

        var initial = window.location.hash.replace("#", "");

        if (initial) {
            activateTab(group, initial, false);
        }
    });

    window.addEventListener("hashchange", function () {
        var name = window.location.hash.replace("#", "");

        if (!name) {
            return;
        }

        tabGroups.forEach(function (group) {
            activateTab(group, name, false);
        });
    });

    /* ----------------------------------------------------------------------
       Server-side toast
       ---------------------------------------------------------------------- */

    if (toast) {
        var serverMessage = toast.getAttribute("data-message");

        if (serverMessage) {
            showToast(serverMessage);
        }
    }
})();
