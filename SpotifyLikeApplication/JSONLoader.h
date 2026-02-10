#pragma once
#include <string>
#include <vector>
#include "Song.h"
#include "Playlist.h"

using namespace std;

class JSONLoader
{
public:
	static void LoadAllToPlaylist(const string& filename, Playlist& playlist);
	static vector<Song> SearchFromJSONFile(const string& filename, const string& searchQuery);
};

