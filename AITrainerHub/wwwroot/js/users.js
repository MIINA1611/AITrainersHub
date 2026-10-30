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
        }, 2600);
    }

    window.usersToast = showToast;

    /* Mobile navigation drawer */
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

    /* Placeholder actions for features that are not migrated yet */
    document.addEventListener("click", function (event) {
        var trigger = event.target.closest("[data-placeholder]");

        if (!trigger) {
            return;
        }

        event.preventDefault();
        showToast(trigger.getAttribute("data-placeholder") || "Coming soon.");
    });

    /* Password visibility toggles */
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

    /* Landing category tabs */
    document.querySelectorAll("[data-users-tab]").forEach(function (tab) {
        tab.addEventListener("click", function () {
            var group = tab.closest("[data-users-tabs]");

            if (group) {
                group.querySelectorAll("[data-users-tab]").forEach(function (item) {
                    item.setAttribute("aria-selected", "false");
                    item.classList.remove("bg-white", "text-black", "shadow");
                    item.classList.add("text-gray-400");
                });
            }

            tab.setAttribute("aria-selected", "true");
            tab.classList.add("bg-white", "text-black", "shadow");
            tab.classList.remove("text-gray-400");

            showToast("Explore filters are coming soon.");
        });
    });
})();
