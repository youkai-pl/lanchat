//import
const gulp = require("gulp")
const del = require("del")
const uglify = require("gulp-uglify-es").default
const pipeline = require("readable-stream").pipeline

//gulp config

gulp.task("dist", function () {
	return new Promise(function (resolve, reject) {
		cleanDist()

		pipeline(
			gulp.src("./lib/*.js"),
			uglify(),
			gulp.dest("../dist/lib/")
		)

		gulp.src(["../README.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../LICENSE.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../src/**/*", "!../src/**/node_modules/**", "!../src/yarn.lock", "!../src/**lib/**"]).pipe(gulp.dest("../dist"))
		resolve()
	})
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