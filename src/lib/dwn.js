//import
const https = require("https")
const fs = require("fs")
const out = require("./out")
const pkg = require("../package.json")
//PLUGINS DOWNLOADER

const plugins = "https://raw.githubusercontent.com/akira202/lanchat/master/src/plugins/"
const self = "https://raw.githubusercontent.com/akira202/lanchat/master/src/package.json"

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
			if (fs.existsSync(dest)) {
				fs.unlinkSync(dest)
				out.status("Done. Relaunch required")
			} else {
				out.status("This plugin doesn't exist")
			}
		} else {
			out.alert("Plugins doesn't work in portable version")
		}
	},

	//check self update
	selfCheck: function () {
		return new Promise((resolve, reject) => {
			https.get(self, response => {

				var body = ""
				response.on("data", function (d) {
					body += d
				})

				response.on("end", function () {
					try {
						var parsed = JSON.parse(body)
						if (compare(pkg.version, parsed.version) === -1) {
							resolve(parsed.version)
						} else {
							resolve(false)
						}
					} catch (e) {
						resolve(false)
					}
				})
			}).on("error", function () {
				resolve(false)
			})
		})
	}
}

function get(url, dest, temp) {

	if (fs.existsSync(dest)) {
		out.loading("Updating")
	} else {
		out.loading("Downloading")
	}

	return new Promise((resolve, reject) => {
		const file = fs.createWriteStream(temp, { flags: "wx" })

		const request = https.get(url, response => {

			if (response.statusCode === 200) {
				response.pipe(file)
			} else {
				file.close()
				fs.unlink(temp, () => { })
				out.stopLoading()
				reject("Not found")
			}
		})

		request.on("error", err => {
			file.close()
			fs.unlink(temp, () => { })
			if (err.code === "ENOTFOUND") {
				out.stopLoading()
				reject("Connection error")
			} else {
				out.stopLoading()
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
			out.stopLoading()
			resolve()

		})

		file.on("error", err => {
			file.close()
			fs.unlink(temp, () => { })
			out.stopLoading()
			reject()
		})
	})
}

function compare(v1, v2, options) {
	var lexicographical = options && options.lexicographical,
		zeroExtend = options && options.zeroExtend,
		v1parts = v1.split("."),
		v2parts = v2.split(".")

	function isValidPart(x) {
		return (lexicographical ? /^\d+[A-Za-z]*$/ : /^\d+$/).test(x)
	}

	if (!v1parts.every(isValidPart) || !v2parts.every(isValidPart)) {
		return NaN
	}

	if (zeroExtend) {
		while (v1parts.length < v2parts.length) v1parts.push("0")
		while (v2parts.length < v1parts.length) v2parts.push("0")
	}

	if (!lexicographical) {
		v1parts = v1parts.map(Number)
		v2parts = v2parts.map(Number)
	}

	for (var i = 0; i < v1parts.length; ++i) {
		if (v2parts.length == i) {
			return 1
		}

		if (v1parts[i] == v2parts[i]) {
			continue
		} else if (v1parts[i] > v2parts[i]) {
			return 1
		} else {
			return -1
		}
	}

	if (v1parts.length != v2parts.length) {
		return -1
	}

	return 0
}