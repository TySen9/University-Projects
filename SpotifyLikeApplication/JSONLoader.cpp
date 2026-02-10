#include "JSONLoader.h"
#include <fstream>
#include <json.hpp> //Accesses the JSON library
#include "Utility.h"

// This file implements the JSONLoader class, which is responsible for loading songs from a JSON file into a playlist and searching for songs in the JSON file.

using json = nlohmann::json;

// Loads the songs from a JSON file into the playlist (which is a linked list)
void JSONLoader::LoadAllToPlaylist(const string& filename, Playlist& playlist) {
    ifstream file(filename);
    if (!file.is_open()) {
        cerr << "Failed to open " << filename << "\n";
        return;
    }

    json songsData; //Declares a JSON object to hold all the songs data
    file >> songsData; // Reads the JSON file into the songsData object

	// Loops through each song and then converts them into song objects
    for (const auto& entry : songsData) {
        Song song(
            entry["title"],
            entry["artist"],
            entry["duration"],
            entry.value("genre", ""),
            entry.value("album", ""),
            entry.value("releaseYear", 0)
        );
        playlist.AddSong(song); // adds the songs to the linked list which is the playlist
    }

    cout << "Loaded songs from " << filename << " into your playlist.\n";
}

//Searches the JSON file for the song that is requested by the user using a linear seatch
vector<Song> JSONLoader::SearchFromJSONFile(const string& filename, const string& searchQuery) {
	vector<Song> results; // using a vector to store the results of the search
    ifstream file(filename);
    if (!file.is_open()) {
        cerr << "Failed to open " << filename << endl;
        return results;
    }

    json songsData;
    file >> songsData;
	string q = toLower(searchQuery); // Converts the search query to lowercase for case-insensitive search

	// Performs a linear search through the JSON data to find matching songs
    for (const auto& entry : songsData) {
        string title = entry["title"];
        string artist = entry["artist"];
        string genre = entry.value("genre", "");
        string album = entry.value("album", "");
        int year = entry.value("releaseYear", 0);
        int duration = entry["duration"];

		// checks if the there is a match with the title, artist or genre using a substring search that is case-insensitive
        if (toLower(title).find(q) != string::npos ||
            toLower(artist).find(q) != string::npos ||
            toLower(genre).find(q) != string::npos) {
            results.emplace_back(title, artist, duration, genre, album, year);
        }
    }

	return results;// returns the results of the search to the user to either be added to the playlist or just displayed
}