// Initialize Firebase
var config = {
    apiKey: "AIzaSyCgeNBTA_rTNn0NG2nCAYj401Kf5rXRgx0",
    authDomain: "tiefighter-imperialremnant.firebaseapp.com",
    databaseURL: "https://tiefighter-imperialremnant.firebaseio.com",
    projectId: "tiefighter-imperialremnant",
    storageBucket: "",
    messagingSenderId: "1086585271464"
};
firebase.initializeApp(config);
var db = firebase.database();
var dbRef = db.ref();