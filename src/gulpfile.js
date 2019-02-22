var gulp = require('gulp');
var del = require('del');

gulp.task('dist', function () {
    return new Promise(function (resolve, reject) {
        gulp.src(['../README.md']).pipe(gulp.dest('../dist'));
        gulp.src(['../LICENSE.md']).pipe(gulp.dest('../dist'));
        gulp.src(['../src/**/*', '!../src/**/node_modules/**']).pipe(gulp.dest('../dist'));
        resolve();
    });
});

gulp.task('clean', function (cb) {
    return del(['../dist/**/*', '!../dist/.gitkeep'], { force: true }, cb);
});