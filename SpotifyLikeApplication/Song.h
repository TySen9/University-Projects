#pragma once
#include <string>
#include <iostream>

using namespace std;

class Song {
public:
    string title;
    string artist;
    int duration;
	string genre;
	string album;
	int releaseYear;

    Song(const string& t, const string& a, int d, const string& g, const string& al, int y);
    void DisplaySong() const;
	string GetTitle() const { return title; }
	string GetArtist() const { return artist; }

    bool operator==(const Song& other) const;
    bool operator<(const Song& other) const;
};
