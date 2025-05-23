﻿(function (window) {
    const WINDOW_COMMAND_ATTR_NAME = `app-command`;
    const FORMEDGE_MESSAGE_PASSCODE = `{{FORMEDGE_MESSAGE_PASSCODE}}`;
    const WINFORMEDGE_VERSION = `{{WINFORMEDGE_VERSION}}`;
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



        htmlEl?.classList.toggle("window__titlbar--shown", HAS_TITLE_BAR);
        htmlEl?.classList.toggle("window__titlbar--hidden", !HAS_TITLE_BAR);


    });



    function getHostWindow() {
        if (!window.chrome?.webview?.hostObjects?.sync?.hostWindow) return;
        return window.chrome?.webview?.hostObjects?.sync?.hostWindow
    }

    function hostWindowMinimize() {
        const win = getHostWindow();
        if (!win) return;
        
        win.Minimize();
    }

    function hostWindowMaximize() {
        const win = getHostWindow();
        if (!win) return;
        win.Maximize();
    }

    function hostWindowRestore() {
        const win = getHostWindow();
        if (!win) return;
        win.Restore();
    }
    function hostWindowFullscreen() {
        const win = getHostWindow();
        if (!win) return;
        win.Fullscreen();
    }
    function hostWindowToggleFullscreen() {
        const win = getHostWindow();
        if (!win) return;
        win.ToggleFullscreen();
    }
    function hostWindowClose() {
        const win = getHostWindow();
        if (!win) return;
        win.Close();
    }
    function hostWindowActivate() {
        const win = getHostWindow();
        if (!win) return;
        win.Activate();
    }


    function getHostWindowActivated() {
        const win = getHostWindow();
        if (!win) return;
        return win.Activated;
    }

    function getHostWindowState() {
        const win = getHostWindow();
        if (!win) return;
        return win.WindowState;
    }

    function getHostWindowHasTitleBar() {
        const win = getHostWindow();
        if (!win) return;
        return win.HasTitleBar;
    }



    class HostWindow {
        get activated() {
            return getHostWindowActivated();
        }

        get hasTitleBar() {
            return getHostWindowHasTitleBar();
        }

        get windowState() {
            return getHostWindowState();
        }

        activate() {
            hostWindowActivate();
        }

        minimize() {
            hostWindowMinimize();
        }

        maximize() {
            hostWindowMaximize();
        }

        restore() {
            hostWindowRestore();
        }

        fullscreen() {
            hostWindowFullscreen();
        }

        toggleFullscreen() {
            hostWindowToggleFullscreen();
        }

        close() {
            hostWindowClose();
        }
    }

    function getWinFormedgeVersion() {
        const win = getHostWindow();
        if (!win) return;
        return win.FormedgeVersion;
    }

    function getChromiumVersion() {
        const win = getHostWindow();
        if (!win) return;
        return win.ChromiumVersion;
    }

    class Formedge {

        version = {
            get Formedge() {
                return getWinFormedgeVersion();
            },
            get Chromium() {
                return getChromiumVersion();
            }
        }
    }

    window["formedge"] = new Formedge();
    window["hostWindow"] = new HostWindow();

})(window)