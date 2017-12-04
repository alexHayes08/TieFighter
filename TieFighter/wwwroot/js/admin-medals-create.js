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
    var newKeyPress = false;
    var activeNetworkRequest = false;
    $("#Id").on("keyup", function () {
        if (activeNetworkRequest) {
            return;
        } else {
            setTimeout(function () {

            }, 1000);
            console.log($(this));
            XMLHttpRequestPromise("GET", window.location.origin + "/Medals/IsIdAvailable/" + $(this).value)
                .then(function (response) {
                    var res = JSON.parse(response);
                    if (!res.succeeded) {
                        throw JSON.stringify(new Error("Error occurred while looking up Id."));;
                    } else {
                        console.log(res);
                    }
                }).catch(function (error) {
                    var res = JSON.parse(error);
                    console.log(res);
                });
        }
    });
});