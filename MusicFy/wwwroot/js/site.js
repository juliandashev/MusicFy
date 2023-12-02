// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage() {
    var input = document.getElementById('fileInput');
    var preview = document.getElementById('preview');

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            preview.src = e.target.result;
        }

        reader.readAsDataURL(input.files[0]);
    }
}
document.addEventListener('DOMContentLoaded', function () {
    var songFileUpload = document.getElementById('audio');

    if (songFileUpload) {
        songFileUpload.addEventListener('change', handleSongFileUpload, false);
    }
});

function handleSongFileUpload() {
    var fInput = document.getElementById('audio');
    var file = fInput.files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var songFile = e.target.result;

            // Your code to handle the song file goes here
        };

        reader.readAsArrayBuffer(file);
    }
}