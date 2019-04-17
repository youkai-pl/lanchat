#!/usr/bin/env node

// LANCHAT BY AKIRA
const fs = require("fs")
const prompt = require("./lib/prompt")
const config = require("./lib/config")
var global = require("./lib/global")

console.log("loading...")

//crash handler
/* process.on("uncaughtException", function (err) {
	console.log("Error: " + err)
}) */

//load plugins
if (fs.existsSync("./plugins")) {
	global.plugins = require("require-all")(__dirname + "/plugins")
}

//load config
if (config.load()) {
	//init
	prompt.run()
} else {
	//catch wrong config file
	console.log("Corrupted config file")
	console.log("Delete config file")
	process.exit(0)
}
