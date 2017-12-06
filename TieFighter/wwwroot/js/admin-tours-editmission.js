$(function () {
    // Update the mission
    var $inputs = $("body main input,body main textarea,body main button");
    $("#updateEntity").on("click", function () {

        function show() {
            setInputsTo($inputs, true);
            isLoading($("form")[0], false);
            $("#updateEntity").removeAttr("disabled");
        }

        isLoading($("form")[0], true);
        var formData = new FormData($("form")[0]);

        // Get table form data

        $("#updateEntity").attr("disabled", "");
        setInputsTo($inputs, false);

        updateMedal($("#medalId").text(), formData)
            .then(function (response) {
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw new Error(res.Error);
                }
                show();
                new jBox('Notice', {
                    content: "Successfully updated.",
                    color: "green"
                }).open();
                var newSrc = $("#FileLocation").val();
                if (newSrc != null && newSrc != "") {
                    $("#currentImage").attr("src", newSrc);
                }
            }).catch(function (error) {
                show();
                showErrorMsg(error);
                new jBox('Notice', {
                    content: "Failed to update!",
                    color: "red"
                }).open();
            });
    });
});