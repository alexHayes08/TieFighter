$(function () {
    var viewer = $("#imageViewer");
    var viewerCarouselImages = viewer.find("#imageViewControls .carousel-inner");
    function showImageModalStartingAt(images, index) {
        viewerCarouselImages.empty();

        var newHTMLContent = [];
        for (var img of images) {
            var container = document.createElement("div");
            var image = document.createElement("img");
            container.classList.add("carousel-item");
            image.classList.add("d-block-100", "w-100");
            image.setAttribute("src", img.getAttribute("src"));
            container.appendChild(image);
            viewerCarouselImages.append(container);
        }

        viewerCarouselImages[0].children[index].classList.add("active");
        $("#imageViewer").modal("show");
    }

    $(".sprintImageGroup").each(function () {
        var images = $(this).find("img");
        for (let i = 0; i < images.length; i++) {
            images[i].addEventListener("click", showImageModalStartingAt.bind(window, images, i));
        }

        var links = $(this).find("a");
        links.each(function () {
            this.addEventListener("click", showImageModalStartingAt.bind(null, images, 0));
        });
    });
});