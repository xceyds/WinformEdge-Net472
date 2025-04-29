var formedge;

(function () {
    const WINDOW_COMMAND_ATTR_NAME = "app-command";
    const FORMEDGE_MESSAGE_PASSCODE = "{{FORMEDGE_MESSAGE_PASSCODE}}";
    const IS_SNAP_LAYOUTS_ENABLED = ("{{IS_SNAP_LAYOUTS_ENABLED}}"==="true");
    class HostWindow {

    }

    function postMessage(message) {

        if (window.chrome?.webview?.postMessage) {
            window.chrome.webview.postMessage(message);
        }
        else {
            console.error("[window.chrome.webview.postMessage] is not supported in this environment.");
        }
    }


    window.addEventListener("load", () => {

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








    });

})()