#pragma once
#include <queue>
#include <string>
#include "Song.h"

using namespace std;

class NowPlayingQueue
{
private:
	queue<Song> songQueue;
public:
	void EnqueueSong(const Song& song);
	void PlayNextSong();
	void DisplayQueue() const;
	bool IsEmpty() const;
};

