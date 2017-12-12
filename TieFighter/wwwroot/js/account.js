$(function () {
    var displayNameField = $("#displayName");
    var displayNameUpdateBttn = $("#updateDisplayName");
    var updatedMessage = $("#attemptToUpdateDisplayNameResult");

    function resetUpdateMessage() {
        updatedMessage.text("");
        updatedMessage.removeClass("text-success");
        updatedMessage.removeClass("text-danger");
    }

    displayNameUpdateBttn.on("click", function () {
        resetUpdateMessage();
        displayNameField.attr("disabled", "");
        displayNameUpdateBttn.attr("disabled", "");
        addLoading(displayNameField[0]);
        XMLHttpRequestPromise("POST", "/Account/UpdateDisplayName",
            JSON.stringify({ displayName: displayNameField.val() }))
            .then(function (result) {
                var response = JSON.parse(result);
                if (response.error != "") {
                    throw response.error;
                } else {
                    console.log("updated display name");
                }

                updatedMessage.addClass("text-success");
                updatedMessage.text("Successfully updated display name :)");
                displayNameField.removeAttr("disabled");
                removeLoading(displayNameField[0]);
                displayNameUpdateBttn.removeAttr("disabled");
            }).catch(function (error) {
                updatedMessage.addClass("text-danger");
                updatedMessage.text("Failed to updated display name :(. " + error);
                removeLoading(displayNameField[0]);
                displayNameField.removeAttr("disabled");
                displayNameUpdateBttn.removeAttr("disabled");
            })
    });

    // Upload new thumbnail
    $("#updateThumbnail").on("click", function () {
        resetUpdateMessage();
        var inputEl = $("#newThumbnail")[0];
        if (inputEl.value == "") {
            updatedMessage.addClass("text-danger");
            updatedMessage.text("Need to pick an image first");
            return;
        }
        var formData = new FormData($("#thumbnailForm")[0]);
        isLoading(inputEl, true);
        XMLHttpRequestPromise(
            "POST",
            "/Account/UpdateUserThumbnail",
            formData,
            false)
            .then(function (response) {
                updatedMessage.addClass("text-success");
                updatedMessage.text("Updated thumbnail :)");
                var message = JSON.parse(response).message;
                $("#userPicture").attr("src", message);
                isLoading(inputEl, false);
            }).catch(function (error) {
                updatedMessage.addClass("text-danger");
                updatedMessage.text("Failed to updated thumbnail :( - " + error);
                isLoading(inputEl, false);
            });
    })

    // Remove thumbnail
    $("#useGoogleThumbnail").on("click", function () {
        var inputEl = this;
        isLoading(inputEl, true);
        XMLHttpRequestPromise("POST", `/Account/RemovePreferredThumbnail/${$(this).attr("data-bind")}`)
            .then(function (response) {
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw new Error(res.error);
                }

                var message = res.message;
                updatedMessage.addClass("text-success");
                updatedMessage.text("Updated thumbnail :)");
                $("#userPicture").attr("src", message);
                isLoading(inputEl, false);
            }).catch(function (error) {
                updatedMessage.addClass("text-danger");
                updatedMessage.text("Failed to remove thumbnail :( - " + error);
                isLoading(inputEl, false);
            })
    });
});