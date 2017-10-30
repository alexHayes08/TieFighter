$(function () {
    // Init poppers
    // $("#enterGame").tooltip({
    //     content: "Must be logged in to enter the game.",
    //     html: true,
    //     position: "bottom",
    //     trigger: 'hover focus'
    // });
    $("#enterGame").popover({
        content: "Must be logged in to enter the game.",
        position: "bottom",
        trigger: 'focus'
    });

    function login () {
        // Using a redirect.
        firebase.auth().getRedirectResult().then(function(result) {
            if (result.credential) {
            // This gives you a Google Access Token.
            var token = result.credential.accessToken;
            }
            var user = result.user;
        });
    
        // Start a sign in process for an unauthenticated user.
        var provider = new firebase.auth.GoogleAuthProvider();
        provider.addScope('profile');
        provider.addScope('email');
        firebase.auth().signInWithRedirect(provider);
    }

    $("#login").on("click", login);
});