// INTERFACE
var readline = require("readline")
var rl = readline.createInterface({
	input: process.stdin,
	output: process.stdout,
	prompt: "> ",
	terminal: true
})

// Exports
exports.rl = rl
exports.readline = readline