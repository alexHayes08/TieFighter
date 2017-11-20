//onFirebaseInit.push(function (user, db) {
//    $("#userPicture").attr("src", user.photoURL);
//    $("#email").val(user.email);
//    $("#defaultDisplayName").val(user.displayName);

//    var displayNameInputEl = $("#displayName");

//    // Check to see if the user has entered a different display name
//    db.ref(`users/${user.uid}`).once("value").then(function (snapshot) {
//        var userDisplayName = snapshot.val() || user.displayName;
//        displayNameInputEl.val(userDisplayName);
//        displayNameInputEl.removeAttr("readonly");
//    });

//    $("#updateDisplayName").on("change", function () {
//        console.log("Updating display name...");
//    });
//});

$(function () {
    var displayNameField = $("#displayName");
    var displayNameUpdateBttn = $("#updateDisplayName");
    var updatedMessage = $("#attemptToUpdateDisplayNameResult");

    displayNameUpdateBttn.on("click", function () {
        updatedMessage.text("");
        updatedMessage.removeClass("text-success");
        updatedMessage.removeClass("text-danger");
        displayNameField.attr("disabled", "");
        displayNameUpdateBttn.attr("disabled", "");
        addLoading(displayNameField[0]);
        XMLHttpRequestPromise("POST", "/Account/UpdateDisplayName",
            JSON.stringify({ displayName: displayNameField.val() }))
            .then(function (result) {
                var response = JSON.parse(result);
                if (response.error != "") {
                    throw response.Error
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
});