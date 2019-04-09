//import
const gulp = require("gulp")
const del = require("del")
const uglify = require("gulp-uglify-es").default
const pipeline = require("readable-stream").pipeline
const { exec } = require("pkg")

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
	cleanBin()
	await exec([".", "--target", "win-x86", "--output", "../bin/lanchat"])
	await exec([".", "--target", "linux-x64", "--output", "../bin/lanchat-linux64"])
	await exec([".", "--target", "linux-x86", "--output", "../bin/lanchat-linux32"])
	return
})

gulp.task("clean", function () {
	return new Promise(function (resolve, reject) {
		cleanDist()
		resolve()
	})
})

function cleanDist() {
	del.sync(["../dist/**/*"], { force: true })
}

function cleanBin() {
	del.sync(["../bin/**/*"], { force: true })
}