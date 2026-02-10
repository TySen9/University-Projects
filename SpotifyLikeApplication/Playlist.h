#pragma once
#include "Song.h"
#include <iostream>
#include <vector>
#include <string>

using namespace std;

class Playlist {
private:
    struct Node {
        Song data;
        Node* next;
        Node(const Song& s);
    };

    Node* head;

public:
    Playlist();
    ~Playlist();

    void AddSong(const Song& song);
    void DisplayPlaylist() const;
    void SearchSongByTitle(const string& title);
    int RemoveAllSongsByTitle(const string& title);
    void SortSongs();
    Playlist operator+(const Playlist& other) const;
    Song* FindFirst(const string& title);
    Song* FindFirstByTitle(const string& title);
    vector<Song*> FindByArtist(const string& artist);
	vector<Song*> FindByTitleSubstring(const string& partialTitle);
    vector<Song*> UnifiedSearch(const string& searchQuery);

};
