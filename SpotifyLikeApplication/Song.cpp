#include "Song.h"
#include <iostream>

// This file implements the Song class, which represents a song with various attributes and provides methods to display and compare songs.

Song::Song(const string& t, const string& a, int d, const string& g, const string& al, int y)
    : title(t), artist(a), duration(d), genre(g), album(al), releaseYear(y) {
}

void Song::DisplaySong() const {
    cout << title << " by " << artist 
        << " | " << duration << "s"
        << " | Genre: " << genre
        << " | Album: " << album
        << " | Year: " << releaseYear << "\n";
}

bool Song::operator==(const Song& other) const {
    return title == other.title && artist == other.artist;
}

bool Song::operator<(const Song& other) const {
    return title < other.title;
}