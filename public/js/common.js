var onFirebaseInit = [];

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
firebase.auth().setPersistence(firebase.auth.Auth.Persistence.LOCAL).then(function () {
  console.log("Set persistence to local.");
  firebase.auth().onAuthStateChanged(function(user) {
    if (user) {
      var db = firebase.database();

      // User is signed in.
      $("#account").removeClass("hidden");
      
      $("#account").text(user.displayName);
      
      $("#logout").on("click", function () {
          firebase.auth().signOut()
              .then(function (result) {
                  window.location.href = "/";
              }).catch(error => console.error(error));
      });

      for (var f of onFirebaseInit) {
        if (typeof(f) == "function")
          f(user, db);
      }
    } else {
      // No user is signed in.
      $("#login").removeClass("hidden");
      
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
    }
  });
  var defaultAuth = firebase.auth();
  var currentUser = defaultAuth.currentUser;

}).catch (function (error) {
  console.error(error);
});

// Firebase testing

// function addToDo (e) {
//   // e.preventDefault();
//   var test = {
//     names: [
//       "alex",
//       "bob",
//       "charlie"
//     ],
//     highscore: 24
//   };

//   // Add to database
//   var testKey = dbRef.child("tests").push().key;
//   var updates = {};
//   updates["/tests" + testKey] = test;

//   return dbRef.update(updates);
// }

// function addToDoList (toDoList) {

// }