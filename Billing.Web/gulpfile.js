var gulp = require('gulp');
var clean = require('gulp-clean-css');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var gutil = require('gulp-util');
var source = ['app/*.js', 'app/*/*/*.js', 'app/*/*/*/*.js'];
var js = ['assets/js/*.js'];
var style = 'assets/css/*.css';
var library = 'library/*.js';

gulp.task('default', function () {
    return gulp.src(source)
        .pipe(concat('app.js'))
        .pipe(uglify())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(rename('app.min.js'))
        .pipe(gulp.dest('build'));
});


gulp.task('js', function() {
    return gulp.src(js)
        .pipe(concat('js.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('build'))
});

gulp.task('css', function() {
    return gulp.src(style)
        .pipe(concat('app.css'))
        .pipe(clean('app.css'))
        .pipe(rename('style.min.css'))
        .pipe(gulp.dest('build'))
});

gulp.task('library', function() {
    return gulp.src(library)
               .pipe(concat('angular.min.js'))
               .pipe(gulp.dest('build'))
});

