//import
const gulp = require("gulp")
const del = require("del")
const uglify = require("gulp-uglify-es").default
const pipeline = require("readable-stream").pipeline
const { compile } = require("nexe")
const package = require("./package.json")

//gulp config

gulp.task("dist", function () {
	return new Promise(function (resolve, reject) {
		cleanDist()
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

gulp.task("pkg", async () => {
	clean("bin")
	return new Promise(function (resolve, reject) {
		compile({
			build: true,
			input: "./main.js",
			resources: "./plugins",
			output: "../bin/lanchat.exe",
			ico: "./icon.ico",
			loglevel: "verbose",
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
			target: ["windows-x86-10.15.0", "linux-x64-10.15.0", "linux-x32-10.15.0"]
		}).then(() => {
			console.log("success")
			resolve()
		})
	})
})

gulp.task("clean", function () {
	return new Promise(function (resolve, reject) {
		clean("dist")
		resolve()
	})
})

function clean(dir) {
	del.sync(["../" + dir + "/**/*"], { force: true })
}