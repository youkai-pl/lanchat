// import
const gulp = require("gulp")
const yarn = require("gulp-yarn")
const del = require("del")
const uglify = require("gulp-uglify-es").default
const pipeline = require("readable-stream").pipeline
const { compile } = require("nexe")

// gulp config

// dist
gulp.task("dist", () => {
	return new Promise(((resolve, reject) => {
		clean("dist")
		pipeline(
			gulp.src("./lib/*.js"),
			uglify(),
			gulp.dest("../dist/lib/")
		)
		gulp.src(["../README.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../LICENSE.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../API.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../src/**/*", "!../src/**/node_modules/**", "!../src/yarn.lock", "!../src/lib/**/*", "!../src/plugins/**/*", "!../src/gulpfile.js", "!../src/icon.ico"]).pipe(gulp.dest("../dist"))
		resolve()
	}))
})

// pkg
gulp.task("pkg-compile", () => {
	return new Promise(((resolve, reject) => {
		compile({
			input: "../dist/main.js",
			output: "../bin/lanchat.exe",
			target: "windows-x86-10.15.3"
		}).then(() => {
			console.log("success windows")
			resolve()
		})
	}))
})

// clean
gulp.task("clean", () => {
	return new Promise(((resolve, reject) => {
		clean("dist")
		resolve()
	}))
})

// prepkg
gulp.task("prepkg", () => {
	return new Promise(((resolve, reject) => {
		clean("bin")
		gulp.src(["../dist/package.json"])
			.pipe(yarn({
				production: true
			}))
		resolve()
	}))
})

// clean
function clean(dir) {
	del.sync(["../" + dir + "/**/*"], { force: true })
}