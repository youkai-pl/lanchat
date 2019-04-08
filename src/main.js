#!/usr/bin/env node

// LANCHAT BY AKIRA
const fs = require("fs")
const prompt = require("./lib/prompt")
const settings = require("./lib/settings")

//crash handler
process.on("uncaughtException", function (err) {
	console.log("Error: " + err)
})

//check plugins dir
if (!fs.existsSync("./plugins")) {
	fs.mkdirSync("plugins")
}

//load plugins
require("require-all")(__dirname + "/plugins")

//load config
if (settings.load()) {
	//init
	prompt.run()
} else {
	//catch wrong config file
	console.log("Corrupted config file")
	console.log("Delete config file")
	process.exit(0)
}
