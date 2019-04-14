//import
const gulp = require("gulp")
const yarn = require("gulp-yarn")
const del = require("del")
const uglify = require("gulp-uglify-es").default
const pipeline = require("readable-stream").pipeline
const { compile } = require("nexe")
const package = require("./package.json")

//gulp config

//dist
gulp.task("dist", function () {
	return new Promise(function (resolve, reject) {
		clean("dist")
		pipeline(
			gulp.src("./lib/*.js"),
			uglify(),
			gulp.dest("../dist/lib/")
		)
		pipeline(
			gulp.src("./plugins/*.js"),
			uglify(),
			gulp.dest("../dist/plugins/")
		)
		gulp.src(["../README.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../LICENSE.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../API.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../src/**/*", "!../src/**/node_modules/**", "!../src/yarn.lock", "!../src/**lib/**", "!../src/**plugins/**", "!../src/gulpfile.js"]).pipe(gulp.dest("../dist"))
		resolve()
	})
})

//build
gulp.task("build", function () {
	return new Promise(function (resolve, reject) {
		compile({
			build: true,
			input: "../dist/main.js",
			resources: "../dist/plugins",
			output: "../bin/build/lanchat.exe",
			ico: "../dist/icon.ico",
			rc: {
				CompanyName: "akira202",
				PRODUCTVERSION: package.version,
				FILEVERSION: package.version,
				ProductName: "Lanchat",
				FileDescription: "Lanchat: chat in lan",
				LegalCopyright: "MIT License",
				InternalName: "lanchat",
				OriginalFilename: "lanchat.exe"
			},
			target: ["windows-x86-10.15.3", "linux-x64-10.15.3", "linux-x32-10.15.3"]
		}).then(() => {
			console.log("success")
			resolve()
		})
	})
})

//pkg
gulp.task("pkg-compile", function () {
	return new Promise(function (resolve, reject) {
		compile({
			input: "../dist/main.js",
			resources: "../dist/plugins",
			output: "../bin/pkg/lanchat",
			target: ["windows-x86-10.15.3", "linux-x64-10.15.3", "linux-x32-10.15.3"]
		}).then(() => {
			console.log("success")
			resolve()
		})
	})
})

//clean
gulp.task("clean", function () {
	return new Promise(function (resolve, reject) {
		clean("dist")
		resolve()
	})
})

//prepkg
gulp.task("prepkg", function prepkg() {
	return new Promise(function (resolve, reject) {
		clean("bin")
		gulp.src(["../dist/package.json"])
			.pipe(yarn({
				production: true
			}))
		resolve()
	})
})

//clean
function clean(dir) {
	del.sync(["../" + dir + "/**/*"], { force: true })
}