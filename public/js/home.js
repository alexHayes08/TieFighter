$(function () {
    $("#goToSecondSequence").on("click", function () {
        var bbox = $("#secondSequence")[0].getBoundingClientRect();
        window.scroll({
            top: bbox.top,
            left: 0,
            behaviour: "smooth"
        });
    });
});