﻿function setAllInputsTo(boolean) {
    var inputs = $("form input, form textarea, form button");
    if (boolean) {
        // Enable all inputs
        inputs.removeAttr("disabled");
    } else {
        // Disable all inputs
        inputs.attr("disabled", "disabled");
    }
}

function addNewCondition() {

}

$(function () {
    // Preview the new medal image
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

    // Update the medal
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
            }).catch(function (error) {
                show();
                showErrorMsg(error);
                new jBox('Notice', {
                    content: "Failed to update!",
                    color: "red"
                }).open();
            });
    });

    // Add new condition
    $("#addNewCondition").on("click", function () {
        var newEl = $("#conditionalRowTemplate")[0].cloneNode(true);
        $("#conditionsTable > tbody").append($(newEl));
        $(newEl)
            .find("select")
            .on("change", function () {
                var valEl = $(this)
                    .parents()[1]
                    .find("td:last-child");

                var newValHmtl = null;
                switch ($(this).val()) {
                    case "TimeSpan":
                        newValHmtl = "<input type=\"datetime\"/>";
                    case "KillCount":
                    case "TotalTravelDistance":
                    case "WithoutDying":
                    case "StatAt":
                        break;
                }

                valEl.html(newValHtml);
            });
    });
});