//import
const https = require("https")
const fs = require("fs")
const out = require("./out")
//PLUGINS DOWNLOADER

const plugins = "https://raw.githubusercontent.com/akira202/lanchat/master/src/plugins/"

module.exports = {

	//donwload
	donwload: function (name) {

		if (fs.existsSync("./plugins")) {
			var url = plugins + name + ".js"
			var dest = "./plugins/" + name + ".js"
			var temp = "./plugins/" + name + "_temp.js"

			get(url, dest, temp)
				.then(() => out.status("Done. Relaunch required."))
				.catch(e => out.alert(e))
		} else {
			out.alert("Plugins doesn't work in portable version")
		}
	},

	//delete
	delete: function (name) {
		if (fs.existsSync("./plugins")) {
			var dest = "./plugins/" + name + ".js"
			if(fs.existsSync(dest)){
				fs.unlinkSync(dest)
				out.status("Done. Relaunch required")
			} else {
				out.status("This plugin doesn't exist")
			}
		} else {
			out.alert("Plugins doesn't work in portable version")
		}
	}
}

function get(url, dest, temp) {

	if (fs.existsSync(dest)) {
		out.status("Updating...")
	} else {
		out.status("Downloading...")
	}

	return new Promise((resolve, reject) => {
		const file = fs.createWriteStream(temp, { flags: "wx" })

		const request = https.get(url, response => {

			if (response.statusCode === 200) {
				response.pipe(file)
			} else {
				file.close()
				fs.unlink(temp, () => { })
				reject("Not found")
			}
		})

		request.on("error", err => {
			file.close()
			fs.unlink(temp, () => { })
			if (err.code === "ENOTFOUND") {
				reject("Connection error")
			} else {
				reject(err.code)
			}
		})

		file.on("finish", () => {
			if (fs.existsSync(temp) & fs.existsSync(dest)) {
				fs.unlinkSync(dest)
			}
			if (fs.existsSync(temp)) {
				fs.renameSync(temp, dest)
			}
			resolve()

		})

		file.on("error", err => {
			file.close()
			fs.unlink(temp, () => { })
			reject()
		})
	})
}