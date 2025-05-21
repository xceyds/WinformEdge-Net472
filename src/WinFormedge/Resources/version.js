(function () {
    const WINFORMEDGE_VERSION_INFO = `{{WINFORMEDGE_VERSION_INFO}}`;
    function showVersionLog() {
        console.log("==== Welcome to The%c WinFormege %cProject ====", "color: #fff;font-weight:bold;font-size:1.1em;text-shadow: #152173 0 0 3px;", "");
        const badgeLeftCss = "font-size:0.8em;background: #35495e; padding: 4px; border-radius: 5px 0 0 5px; color: #fff";
        const badgeRightCss = "font-size:0.8em;background: #83b841; padding: 4px; border-radius: 0 5px 5px 0; color: #fff";
        console.log(WINFORMEDGE_VERSION_INFO, badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "");
        console.log(`Copyrights (C) ${new Date().getFullYear()} Xuanchen Lin all rights reserved.`);
    }

    if (document.readyState === "loading") {
        window.addEventListener("DOMContentLoaded", () => {
            showVersionLog();
        });
    }
    else {
        showVersionLog();
    }
})();