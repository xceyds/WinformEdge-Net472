(function (window) {
    const WINDOW_COMMAND_ATTR_NAME = `app-command`;
    const FORMEDGE_MESSAGE_PASSCODE = `{{FORMEDGE_MESSAGE_PASSCODE}}`;
    const WINFORMEDGE_VERSION = `{{WINFORMEDGE_VERSION}}`;
    const WINFORMEDGE_VERSION_INFO = `{{WINFORMEDGE_VERSION_INFO}}`;
    const HAS_TITLE_BAR = (`{{HAS_TITLE_BAR}}` === "true");

    function postMessage(message) {

        if (window.chrome?.webview?.postMessage && message.message) {
            window.chrome.webview.postMessage(message);
        }
        else {
            console.error("[window.chrome.webview.postMessage] is not supported in this environment.");
        }
    }

    function raiseHostWindowEvent(eventName, detail) {
        window.dispatchEvent(new Event(eventName, {
            detail: detail,
        }));
    }

    window["formedgeVersion"] = WINFORMEDGE_VERSION;

    window.moveTo = (x, y) => {
        postMessage({
            passcode: FORMEDGE_MESSAGE_PASSCODE,
            message: "FormedgeWindowMoveTo",
            x,
            y
        });
    }

    window.resizeTo = (width, height) => {
        postMessage({
            passcode: FORMEDGE_MESSAGE_PASSCODE,
            message: "FormedgeWindowResizeTo",
            width,
            height
        });
    }

    window.moveBy = (dx, dy) => {
        postMessage({
            passcode: FORMEDGE_MESSAGE_PASSCODE,
            message: "FormedgeWindowMoveBy",
            dx,
            dy
        });
    }

    window.resizeBy = (dx, dy) => {
        postMessage({
            passcode: FORMEDGE_MESSAGE_PASSCODE,
            message: "FormedgeWindowResizeBy",
            dx,
            dy
        });
    }

    window.addEventListener("load", () => {

        const htmlEl = document.querySelector("html");

        window.addEventListener("click", (e) => {
            const button = e.button;

            if (button === 0) {
                let srcElement = e.target;

                while (srcElement && !srcElement.hasAttribute(WINDOW_COMMAND_ATTR_NAME)) {
                    srcElement = srcElement.parentElement;
                }

                if (srcElement) {
                    const command = srcElement.getAttribute(WINDOW_COMMAND_ATTR_NAME)?.toLowerCase();
                    postMessage({
                        passcode: FORMEDGE_MESSAGE_PASSCODE,
                        message: "FormedgeWindowCommand",
                        command: command,
                    });
                }
            }
        });

        //if (IS_SNAP_LAYOUTS_ENABLED) {

        //    let isMaximizeAreaMouseOver = false;

        //    window.addEventListener("mousemove", (e) => {
        //        let srcElement = e.target;

        //        while (srcElement && !srcElement.hasAttribute(WINDOW_COMMAND_ATTR_NAME)) {
        //            srcElement = srcElement.parentElement;
        //        }

        //        if (srcElement && srcElement.getAttribute(WINDOW_COMMAND_ATTR_NAME)?.toLowerCase() === "maximize") {
        //            isMaximizeAreaMouseOver = true;
        //            postMessage({
        //                passcode: FORMEDGE_MESSAGE_PASSCODE,
        //                message: "FormedgeWindowSnapLayoutsRequired",
        //                status: isMaximizeAreaMouseOver
        //            });

        //        }
        //        else {
        //            if (isMaximizeAreaMouseOver) {
        //                isMaximizeAreaMouseOver = false;
        //                postMessage({
        //                    passcode: FORMEDGE_MESSAGE_PASSCODE,
        //                    message: "FormedgeWindowSnapLayoutsRequired",
        //                    status: isMaximizeAreaMouseOver
        //                });
        //            }

        //        }

        //    });
        //}

        if (window.chrome?.webview?.addEventListener) {
            window.chrome.webview.addEventListener("message", (e) => {
                const { passcode, message } = e.data;
                if (passcode !== FORMEDGE_MESSAGE_PASSCODE) {
                    return;
                }
                switch (message) {
                    case "FormedgeNotifyWindowStateChange":
                        onFormedgeNotifyWindowStateChange(e.data);
                        break;
                    case "FormedgeNotifyWindowResize":
                        onFormedgeNotifyWindowResize(e.data);
                        break;
                    case "FormedgeNotifyWindowMove":
                        onFormedgeNotifyWindowMove(e.data);
                        break;
                    case "FormedgeNotifyWindowActivated":
                        onFormedgeNotifyWindowActivated(e.data);
                        break;
                }

            });
        }


        onFormedgeNotifyWindowActivated({
            passcode: FORMEDGE_MESSAGE_PASSCODE,
            message: "FormedgeNotifyWindowActivated",
            state: true
        });


        showVersionLog();

        htmlEl?.classList.toggle("window__titlbar--shown", HAS_TITLE_BAR);
        htmlEl?.classList.toggle("window__titlbar--hidden", !HAS_TITLE_BAR);


    });

    function showVersionLog() {
        console.log("==== Welcome to The%c WinFormege %cProject ====", "color: #fff;font-weight:bold;font-size:1.1em;text-shadow: #152173 0 0 3px;", "");
        const badgeLeftCss = "font-size:0.8em;background: #35495e; padding: 4px; border-radius: 5px 0 0 5px; color: #fff";
        const badgeRightCss = "font-size:0.8em;background: #83b841; padding: 4px; border-radius: 0 5px 5px 0; color: #fff";
        console.log(WINFORMEDGE_VERSION_INFO, badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "");
        console.log(`Copyrights (C) ${new Date().getFullYear()} Xuanchen Lin all rights reserved.`);
    }



    function onFormedgeNotifyWindowActivated(data) {
        if (data.state === undefined) return;

        const { state } = data;
        const htmlEl = document.querySelector("html");

        if (state) {
            raiseHostWindowEvent("windowactivated", {});
        }
        else {
            raiseHostWindowEvent("windowdeactivate", {});
        }

        htmlEl?.classList.toggle("window--activated", state);
        htmlEl?.classList.toggle("window--deactivated", !state);
    }

    function onFormedgeNotifyWindowStateChange(data) {
        const { state } = data;
        if (!state) return;

        raiseHostWindowEvent("windowstatechange", { state });

        const htmlEl = document.querySelector("html");
        htmlEl?.classList.toggle("window--maximized", state === "maximized");
        htmlEl?.classList.toggle("window--minimized", state === "minimized");
        htmlEl?.classList.toggle("window--fullscreen", state === "fullscreen");
    }

    function onFormedgeNotifyWindowResize(data) {
        const { x, y, width, height } = data;
        raiseHostWindowEvent("windowresize", { x, y, width, height });
    }

    function onFormedgeNotifyWindowMove(data) {
        const { x, y, screenX, screenY } = data;
        raiseHostWindowEvent("windowmove", { x, y, screenX, screenY });
    }

    const hostWindow = {
        minimize: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Minimize();
        },
        maximize: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Maximize();
        },
        restore: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Restore();
        },
        fullscreen: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Fullscreen();
        },
        toggleFullscreen: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.ToggleFullscreen();
        },
        close: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Close();
        },
        activate: async () => {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            await win.Activate();
        }
    };

    Object.defineProperty(hostWindow, "activated", {
        async get() {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            return await win.Activated;
        }
    });

    Object.defineProperty(hostWindow, "hasTitleBar", {
        async get() {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            return await win.HasTitleBar;
        }
    });

    Object.defineProperty(hostWindow, "windowState", {
        async get() {
            if (!window.chrome?.webview?.hostObjects?.hostWindow) return;
            var win = window.chrome?.webview?.hostObjects?.hostWindow;
            return await win.WindowState;
        }
    });

    window["hostWindow"] = hostWindow;

})(window)