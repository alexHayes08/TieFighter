// var webClientId = "1086585271464-5kjfr6pbsm7omriab1b7v4tgc3d9130o.apps.googleusercontent.com";
// var webClientSecret = "1HbqJ0gjfPByh0ma_70Fgk2g";

// Initialize Firebase
var config = {
  apiKey: "AIzaSyCgeNBTA_rTNn0NG2nCAYj401Kf5rXRgx0",
  authDomain: "tiefighter-imperialremnant.firebaseapp.com",
  databaseURL: "https://tiefighter-imperialremnant.firebaseio.com",
  projectId: "tiefighter-imperialremnant",
  storageBucket: "tiefighter-imperialremnant.appspot.com",
  messagingSenderId: "1086585271464"
};
firebase.initializeApp(config);

(function () {

  // Call firebase redirect login function on elements
  // $("#enterGame").on("click", function () {
  //   $(this).popover({
  //     container: 'body',
  //     content: "Must be logged in to enter the game. <a href=\"#\">Login</a>",
  //     html: true,
  //     position: "bottom",
  //     // title: "Test",
  //     trigger: 'click'
  //   });
  // });

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

    $("#login").on("click", login);
  }
})();

// Firebase testing

function addToDo (e) {
  // e.preventDefault();
  var test = {
    names: [
      "alex",
      "bob",
      "charlie"
    ],
    highscore: 24
  };

  // Add to database
  var testKey = dbRef.child("tests").push().key;
  var updates = {};
  updates["/tests" + testKey] = test;

  return dbRef.update(updates);
}

function addToDoList (toDoList) {

}