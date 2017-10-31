var express = require('express');
var router = express.Router();

/* GET home page. */
router.get("/", function(req, res) {
  res.render("index", { 
    title: "Home"
  });
});

router.get("/about", function(req, res) {
  res.render("about", {
    title: "About"
  });
})

router.get("/artwork", function (req, res) {
  res.render("artwork", {
    title: "Artwork"
  });
})

router.get('/game', function(req, res) {
  res.render('game', {title: 'Tie Fighter'});
});

module.exports = router;
