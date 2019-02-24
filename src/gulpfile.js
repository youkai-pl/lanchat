var gulp = require("gulp")
var del = require("del")

gulp.task("dist", function () {
	return new Promise(function (resolve, reject) {
		cleanDist()
		gulp.src(["../README.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../LICENSE.md"]).pipe(gulp.dest("../dist"))
		gulp.src(["../src/**/*", "!../src/**/node_modules/**", "!../src/yarn.lock"]).pipe(gulp.dest("../dist"))
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
	del.sync(["../dist/**/*", "!../dist/.gitkeep"], { force: true })
}