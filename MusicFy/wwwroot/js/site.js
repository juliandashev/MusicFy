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
function addSelectedSong() {
    var selectedSongsDropdown = document.getElementById('selectedSongs');

    var selectedSongId = selectedSongsDropdown.value;
    var selectedSongName = selectedSongsDropdown.options[selectedSongsDropdown.selectedIndex].text;

    var table = document.getElementById('songsTable');
    var existingSongs = table.getElementsByTagName('tr');

    for (var i = 1; i < existingSongs.length; i++) {
        var existingSongId = existingSongs[i].getAttribute('data-song-id');

        if (existingSongId === selectedSongId) {
            alert('Song already added.');
            return;
        }
    }

    // Създаване на нов ред в таблицата и добавяне на информация за избраната песен
    var newRow = table.insertRow(table.rows.length);
    newRow.setAttribute('data-song-id', selectedSongId);

    var cell = newRow.insertCell(0);
    cell.innerHTML = selectedSongName;
}

function getSongById(songId) {
   
    var song = {
        Id: songId,
        Name: "Song Name " + songId, 
    };
    return song;
}

function addSongToTable(song) {
    var table = $("#songsTable");

    if (!table.find('tr[data-song-id="' + song.Id + '"]').length) {
        var row = "<tr data-song-id='" + song.Id + "'>" +
            "<td>" + song.Name + "</td>" +
            "</tr>";

        table.append(row);
    }
}