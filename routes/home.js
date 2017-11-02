var express = require('express');
var router = express.Router();

/* GET home page. */
router.get("/", function(req, res) {
  res.render("home/index", { 
    title: "Home"
  });
});

router.get("/about", function(req, res) {
  res.render("home/about", {
    title: "About"
  });
})

router.get("/artwork", function (req, res) {
  res.render("home/artwork", {
    title: "Artwork"
  });
})

router.get('/game', function(req, res) {
  res.render('home/game', 
  {
    title: 'Tie Fighter'
  });
});

module.exports = router;
