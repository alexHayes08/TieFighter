function setAllInputsTo(boolean) {
    var inputs = $("form input, form textarea, form button");
    if (boolean) {
        // Enable all inputs
        inputs.removeAttr("disabled");
    } else {
        // Disable all inputs
        inputs.attr("disabled", "disabled");
    }
}

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

    $("#updateEntity").on("click", function () {

        function show() {
            setAllInputsTo(true);
            isLoading($("form")[0], false);
            $("#updateEntity").removeAttr("disabled");
        }

        isLoading($("form")[0], true);
        var formData = new FormData($("form")[0]);
        $("#updateEntity").attr("disabled", "");
        setAllInputsTo(false);
        setLoadingScreenVisible(true);

        updateMedal($("#medalId").text(), formData)
            .then(function (response) {
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw new Error(res.Error);
                }
                show();
            }).catch(function (error) {
                show();
                showErrorMsg(error);
            });
    });
});