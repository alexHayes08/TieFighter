var express = require('express');
var router = express.Router();

/* GET home page. */
router.get("/", function(req, res) {
  res.render("admin/index", { 
    title: "Admin"
  });
});

router.get("/users", function(req, res) {
  res.render("admin/users", {
    title: "Admin"
  });
})

router.get("/campaign", function (req, res) {
  res.render("admin/campaign", {
    title: "Admin"
  });
})

module.exports = router;
