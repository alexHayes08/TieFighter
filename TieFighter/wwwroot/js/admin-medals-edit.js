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

        isLoading($("body main form")[0], true);
        var formData = new FormData($("#medalForm")[0]);

        // TODO: Get table form data
        //$("#conditionsTable ")

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

    // Add new condition
    $("#addNewCondition").on("click", function () {
        var newEl = $("#conditionalRowTemplate")[0].cloneNode(true).content;
        $(newEl)
            .find("select")
            .on("change", function () {
                var valEl = $($(this)
                    .parents()[1])
                    .find("td:nth-child(3)");

                var newValHtml = null;
                switch ($(this).val()) {
                    case "TimeSpan":
                        newValHtml = "<input type=\"datetime\" class=\"form-control\"/>";
                        break;
                    case "KillCount":
                    case "TotalTravelDistance":
                        newValHtml = "<input type=\"number\" class=\"form-control\"/>";
                        break;
                    case "WithoutDying":
                        newValHtml = "<label><input type=\"checkbox\" checked disabled class=\"form-control custom-checkbox\"/></label>";
                        break;
                    case "StatAt":
                        newValHtml = $("#conditionalStatType")[0].cloneNode(true).content;
                        break;
                }

                valEl.html(newValHtml);
            });

        newEl = $("#conditionsTable > tbody")[0].appendChild(newEl);
    });
});