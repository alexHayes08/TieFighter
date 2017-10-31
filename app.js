var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
// const gcloud = require("google-cloud")
// const datastore = gcloud.datastore({
//     projectId: "testmanager-140802",
//     keyFilename: "TestManager-38a238fd7f42.json",
// });


var index = require("./routes/index");
var account = require("./routes/account");
var users = require("./routes/users");

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

// uncomment after placing your favicon in /public
//app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));
app.use(express.static("public"));

// Rest API
// Good response
function goodMessageTemplate (optionalMessage) {
  var status =  {
    error: false
  };

  if (optionalMessage)
    status.message = optionalMessage;

  return status;
}

// Bad response
function errorMessageTemplate (error, optionalMessage) {
  var status =  {
    error: error
  };

  if (optionalMessage)
    status.message = optionalMessage;

  return status;
}

// Webpages
app.get("/", index);
app.get("/about", index);
app.get("/account", account);
app.get("/artwork", index);
app.get("/game", index);
app.get("/users", users);

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error("Not Found");
  err.status = 404;
  next(err);
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
