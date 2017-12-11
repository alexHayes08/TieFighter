$(function () {
    $(".openingCrawl").on("animationend", function () {
        $("img.title,img.gameTitle").css("animation", "1s 1 forwards fadeInAnim");
    });
});