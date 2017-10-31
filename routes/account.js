var express = require('express');
var router = express.Router();

/* GET users listing. */
router.get("/account", function(req, res) {
  res.render("account", { title: "Account" });
});

router.get("/account/stats", function (req, res) {
  res.render("stats", {
    title: "Stats"
  });
});

module.exports = router;
