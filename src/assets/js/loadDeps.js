/**
 * electron require extension
 *
 * With this we can set aliases for specific path`s and require more strict and less issue tolerant due to aliases
 */
var req = require("electron-require");

//! angular aliases
req.set("angular", "app/");
req.set("component", "app/components/");
req.set("core", "app/core/");
req.set("shared", "app/shared/");

//! library aliases
req.set("asset", "assets/js/");
req.set("lib", "app/libs/");

//! require node modules
require('jquery');
require("angular");
require("angular-animate");
require("angular-aria");
require("angular-messages");
require("angular-material");
require("angular-sanitize");
require("angular-route");

//! require angular objects
req.angular("app.module.js");
req.angular("app.routes.js");

//! Shared Applications
//...

//! Core Applications
req.core("header/headerController.js");
req.core("header/headerService.js");
req.core("footer/footerController.js");
req.core("footer/footerService.js");

//! Components
//...