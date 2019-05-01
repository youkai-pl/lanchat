// zielonka dlc
// if you are not from Zielonka, you will not understand it

// import
const c = require("../lib/colors")
const config = require("../lib/config")
const out = require("../lib/out")
const rl = require("../lib/interface").rl

// variables
var loaded

// exports
module.exports = {
	start: function () {
		if (loaded) {
			if (config.zsp2) {
				out.blank("załadowano zapis")
			} else {
				newSave()
			}
		}
	}
}

process.on("ready", () => {
	rl.pause()
	process.stdout.write("\033c")
	process.stdout.write(c.green)
	weclome()
})

function weclome() {
	process.stdout.write("\033c")
	process.stdout.write(c.green)
	if (process.stdout.columns > 60) {
		console.log("")
		console.log(" ▒███████▒  ██████  ██▓███   ▄████▄   ██░ ██  ▄▄▄     ▄▄▄█████▓")
		console.log(" ▒ ▒ ▒ ▄▀░▒██    ▒ ▓██░  ██▒▒██▀ ▀█  ▓██░ ██▒▒████▄   ▓  ██▒ ▓▒")
		console.log(" ░ ▒ ▄▀▒░ ░ ▓██▄   ▓██░ ██▓▒▒▓█    ▄ ▒██▀▀██░▒██  ▀█▄ ▒ ▓██░ ▒░")
		console.log("   ▄▀▒   ░  ▒   ██▒▒██▄█▓▒ ▒▒▓▓▄ ▄██▒░▓█ ░██ ░██▄▄▄▄██░ ▓██▓ ░ ")
		console.log(" ▒███████▒▒██████▒▒▒██▒ ░  ░▒ ▓███▀ ░░▓█▒░██▓ ▓█   ▓██▒ ▒██▒ ░ ")
		console.log(" ░▒▒ ▓░▒░▒▒ ▒▓▒ ▒ ░▒▓▒░ ░  ░░ ░▒ ▒  ░ ▒ ░░▒░▒ ▒▒   ▓▒█░ ▒ ░░   ")
		console.log(" ░░▒ ▒ ░ ▒░ ░▒  ░ ░░▒ ░       ░  ▒    ▒ ░▒░ ░  ▒   ▒▒ ░   ░    ")
		console.log(" ░ ░ ░ ░ ░░  ░  ░  ░░       ░         ░  ░░ ░  ░   ▒    ░      ")
		console.log("   ░ ░          ░           ░ ░       ░  ░  ░      ░  ░        ")
		console.log(" ░                          ░                                  ")
		console.log("                        Super DLC kurwo")
		out.blank("                    Wpisz start i daj enter")
		out.blank("")
	} else {
		console.log("za wąsko na ascii arta")
		console.log("rozszerz to okienko a jak masz to w dupie to wpisz start i lecimy dalej")
		out.blank("")
	}
	loaded = true
}

function newSave() {

	console.log("Wybierz klase postaci:")
	console.log(" 1 - haker")
	console.log(" 2 - tank")
	console.log(" 3 - paladyn")
	console.log(" 4 - mag")
	console.log("")
	ask("wpisz numerek: ")
		.then((ans) => {
			console.log("Wybrałeś: " + ans)
			rl.prompt(true)
		})
}

function ask(query) {
	return new Promise(resolve => rl.question(query, ans => {
		resolve(ans)
	}))
}
