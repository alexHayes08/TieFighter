$(function () {
    $('[data-toggle="tooltip"]').tooltip({
        delay: 500,
        trigger: "hover"
    });
})

function isLoading(element, boolean) {
    if (boolean) {
        element.setAttribute("loading", "");
    } else {
        element.addEventListener("animationend", function () {
            element.removeAttribute("finishedLoading");
            element.removeAttribute("loading");
        }, { once: true });
        element.setAttribute("finishedLoading", "");
    }
}

function pageLoading(boolean) {
    const pageLoadingTemplate = $.parseHTML(`
        <template>
            <div class="horizontalLoadingIndicator">
                <div class="colorA"></div>
                <div class="colorB"></div>
                <div class="colorC"></div>
                <div class="colorD"></div>
            </div>
        </template>`);
    var $header = $("header");
    if (boolean) {
        var content = pageLoadingTemplate[1].content.cloneNode(true);
        $header.append(content);
    } else {
        $header.find(".horizontalLoadingIndicator").remove()
    }
}