var express = require('express');
var router = express.Router();

/* GET users listing. */
router.get("/", function(req, res) {
  res.render("account/index", { title: "Account" });
});

router.get("/stats", function (req, res) {
  res.render("account/stats", {
    title: "Stats"
  });
});

module.exports = router;
