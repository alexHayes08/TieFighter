$(function () {
    $("#FileLocation").on("change", function (evt) {
        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#newImagePreview").attr('src', e.target.result);
                $("#newImagePreviewContainer")
                    .removeClass("d-none")
                    .addClass("d-inline");
            }

            reader.readAsDataURL(this.files[0]);
        }
    });
    $("Id").on("keyup", function () {
        XMLHttpRequestPromise("GET", window.location.origin + "/Medals/IsIdAvailable/" + $(this).value)
            .then(function (response) {
                var res = JSON.parse(response);
            }).catch(function (error) {
                var res = JSON.parse(error);
            });
    });
});