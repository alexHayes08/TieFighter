onFirebaseInit.push(function (user, db) {
    $("#userPicture").attr("src", user.photoURL);
    $("#email").val(user.email);
    $("#defaultDisplayName").val(user.displayName);

    var displayNameInputEl = $("#displayName");

    // Check to see if the user has entered a different display name
    db.ref(`users/${user.uid}`).once("value").then(function (snapshot) {
        var userDisplayName = snapshot.val() || user.displayName;
        displayNameInputEl.val(userDisplayName);
        displayNameInputEl.removeAttr("readonly");
    });

    $("#updateDisplayName").on("change", function () {
        console.log("Updating display name...");
    });
});