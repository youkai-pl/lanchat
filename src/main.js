#!/usr/bin/env node

// LANCHAT BY AKIRA
const fs = require("fs")
const prompt = require("./lib/prompt")
const config = require("./lib/config")
var global = require("./lib/global")

console.log("Loading...")

//crash handler
/* process.on("uncaughtException", function (err) {
	console.log("Error: " + err)
}) */

//load plugins
if (fs.existsSync("./plugins")) {
	global.plugins = require("require-all")(__dirname + "/plugins")
}

//load config
config.load()

//check config
if (config.validate) {
	prompt.run()
} else {
	console.log("Corrupted config file")
	console.log("Delete config file")
	process.exit(0)
}