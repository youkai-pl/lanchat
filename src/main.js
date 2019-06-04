#!/usr/bin/env node

// LANCHAT BY AKIRA
const prompt = require("./lib/prompt")
const config = require("./lib/config")
const plugins = require("./lib/plugins")

// crash handler
process.on("uncaughtException", (err) => {
	console.log("Error: " + err)
})

// load plugins
plugins.load()

// load config
config.load()

// check config
if (config.validate) {
	prompt.run()
} else {
	console.log("Corrupted config file")
	console.log("Delete config file")
	process.exit(0)
}