$(function () {
    $("#goToSecondSequence").on("click", function () {
        var bbox = $("#secondSequence")[0].getBoundingClientRect();
        window.scroll({
            top: bbox.top - 62 + window.scrollY,
            left: 0,
            behavior: "smooth"
        });
    });

    $(".openingCrawl").on("animationend", function () {
        $("img.title,img.gameTitle").css("animation", "1s 1 forwards fadeInAnim");
    });
});