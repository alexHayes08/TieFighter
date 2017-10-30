var express = require('express');
var router = express.Router();

/* GET home page. */
router.get("/", function(req, res) {
  var user = req.cookies.G_AUTHUSER_H || false;
  res.render("index", { 
    title: "Home", 
    user: user
  });
});

router.get('/game', function(req, res, next) {
  res.render('game', {title: 'Tie Fighter'});
});

module.exports = router;
