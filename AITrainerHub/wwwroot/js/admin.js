(function () {
    "use strict";

    var toast = document.getElementById("adminToast");
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
        }, 2800);
    }

    window.adminToast = showToast;

    /* ----------------------------------------------------------------------
       Mobile drawer
       ---------------------------------------------------------------------- */

    var drawer = document.getElementById("adminDrawer");
    var drawerToggle = document.getElementById("adminDrawerToggle");
    var menuIcon = document.getElementById("adminMenuIcon");
    var closeIcon = document.getElementById("adminCloseIcon");

    function setDrawer(open) {
        if (!drawer) {
            return;
        }

        drawer.classList.toggle("hidden", !open);

        if (menuIcon) {
            menuIcon.classList.toggle("hidden", open);
        }

        if (closeIcon) {
            closeIcon.classList.toggle("hidden", !open);
        }

        if (drawerToggle) {
            drawerToggle.setAttribute("aria-expanded", open ? "true" : "false");
        }

        document.body.classList.toggle("overflow-hidden", open);
    }

    if (drawerToggle) {
        drawerToggle.addEventListener("click", function () {
            setDrawer(drawer.classList.contains("hidden"));
        });
    }

    if (drawer) {
        drawer.querySelectorAll("[data-admin-drawer-close]").forEach(function (element) {
            element.addEventListener("click", function () {
                setDrawer(false);
            });
        });
    }

    /* ----------------------------------------------------------------------
       Categories delete modal (kept for the existing Categories page)
       ---------------------------------------------------------------------- */

    var deleteModal = document.getElementById("deleteCategoryModal");
    var deleteId = document.getElementById("deleteCategoryId");
    var deleteName = document.getElementById("deleteCategoryName");
    var deleteForm = document.getElementById("deleteCategoryForm");

    function setDeleteModal(open) {
        if (!deleteModal) {
            return;
        }

        deleteModal.classList.toggle("hidden", !open);
        document.body.classList.toggle("overflow-hidden", open);

        if (!open && deleteId) {
            deleteId.value = "";
        }
    }

    document.querySelectorAll("[data-delete-id]").forEach(function (button) {
        button.addEventListener("click", function () {
            if (deleteId) {
                deleteId.value = button.getAttribute("data-delete-id") || "";
            }

            if (deleteName) {
                deleteName.textContent = button.getAttribute("data-delete-name") || "";
            }

            setDeleteModal(true);
        });
    });

    if (deleteModal) {
        deleteModal.addEventListener("click", function (event) {
            if (event.target === deleteModal) {
                setDeleteModal(false);
            }
        });

        deleteModal.querySelectorAll("[data-delete-cancel]").forEach(function (element) {
            element.addEventListener("click", function () {
                setDeleteModal(false);
            });
        });
    }

    if (deleteForm) {
        deleteForm.addEventListener("submit", function () {
            var submitButton = deleteForm.querySelector('button[type="submit"]');

            if (submitButton) {
                submitButton.disabled = true;
            }
        });
    }

    /* ----------------------------------------------------------------------
       Generic confirmation for destructive forms
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
       Auto-submit selects / toggles
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
            }, 420);
        });
    });

    /* ----------------------------------------------------------------------
       Moderation tabs
       ---------------------------------------------------------------------- */

    function activateTab(name) {
        document.querySelectorAll("[data-admin-tab]").forEach(function (button) {
            var active = button.getAttribute("data-admin-tab") === name;
            button.classList.toggle("is-active", active);
            button.setAttribute("aria-selected", active ? "true" : "false");
        });

        document.querySelectorAll("[data-admin-panel]").forEach(function (panel) {
            panel.classList.toggle("hidden", panel.getAttribute("data-admin-panel") !== name);
        });
    }

    var tabButtons = document.querySelectorAll("[data-admin-tab]");

    if (tabButtons.length > 0) {
        tabButtons.forEach(function (button) {
            button.addEventListener("click", function () {
                activateTab(button.getAttribute("data-admin-tab"));
            });
        });

        var params = new URLSearchParams(window.location.search);
        var requestedTab = params.get("tab");
        var activeButton = document.querySelector("[data-admin-tab].is-active");
        var initialTab = requestedTab || (activeButton ? activeButton.getAttribute("data-admin-tab") : null);

        if (initialTab) {
            activateTab(initialTab);
        }
    }

    /* ----------------------------------------------------------------------
       Generic modals
       ---------------------------------------------------------------------- */

    function openModal(modal) {
        if (!modal) {
            return;
        }

        modal.classList.add("is-open");
        document.body.classList.add("overflow-hidden");

        var focusable = modal.querySelector("input:not([type='hidden']), select, textarea, button");

        if (focusable) {
            focusable.focus();
        }
    }

    function closeModal(modal) {
        if (!modal) {
            return;
        }

        modal.classList.remove("is-open");
        document.body.classList.remove("overflow-hidden");
    }

    document.querySelectorAll("[data-admin-modal-open]").forEach(function (button) {
        button.addEventListener("click", function () {
            var selector = button.getAttribute("data-admin-modal-open");
            openModal(selector ? document.querySelector(selector) : null);
        });
    });

    document.querySelectorAll("[data-admin-modal-close]").forEach(function (button) {
        button.addEventListener("click", function () {
            var selector = button.getAttribute("data-admin-modal-close");
            closeModal(selector ? document.querySelector(selector) : button.closest(".admin-modal"));
        });
    });

    document.querySelectorAll(".admin-modal").forEach(function (modal) {
        modal.addEventListener("click", function (event) {
            if (event.target === modal) {
                closeModal(modal);
            }
        });
    });

    /* ----------------------------------------------------------------------
       Countries edit modal
       ---------------------------------------------------------------------- */

    var countryModal = document.getElementById("countryEditModal");

    function setFieldValue(id, value) {
        var element = document.getElementById(id);

        if (element) {
            element.value = value || "";
        }
    }

    if (countryModal) {
        document.querySelectorAll("[data-country-edit]").forEach(function (button) {
            button.addEventListener("click", function () {
                setFieldValue("CountryEdit_Id", button.getAttribute("data-id"));
                setFieldValue("CountryEdit_NameAr", button.getAttribute("data-name-ar"));
                setFieldValue("CountryEdit_NameEn", button.getAttribute("data-name-en"));
                setFieldValue("CountryEdit_IsoCode", button.getAttribute("data-iso-code"));

                openModal(countryModal);
            });
        });
    }

    /* ----------------------------------------------------------------------
       Prevent double submits
       ---------------------------------------------------------------------- */

    document.querySelectorAll("form[data-submit-once]").forEach(function (form) {
        form.addEventListener("submit", function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var submitButton = form.querySelector('button[type="submit"]');

            if (submitButton) {
                submitButton.disabled = true;
            }
        });
    });

    /* ----------------------------------------------------------------------
       Escape key
       ---------------------------------------------------------------------- */

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape") {
            setDrawer(false);
            setDeleteModal(false);
            document.querySelectorAll(".admin-modal.is-open").forEach(closeModal);
        }
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
