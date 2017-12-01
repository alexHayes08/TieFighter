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