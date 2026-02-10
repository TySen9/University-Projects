#include "NowPlayingQueue.h"
#include <iostream>
// This file implements the NowPlayingQueue class, which manages a queue (data structure) of songs that are currently playing and makes uses of the queue data structure.

// Adds a song to the Now Playing Queue in a first -in-first-out order
void NowPlayingQueue::EnqueueSong(const Song& song)
{
	songQueue.push(song); // queue is in FIFO order, so the song is added to the end of the queue
	cout << "Song added to the Currently Playing Queue: " << song.GetTitle() << "\n";
}

// Plays the current song and therefore removes it from the queue
void NowPlayingQueue::PlayNextSong() {
    if (!songQueue.empty()) {
        cout << "Finished playing: " << songQueue.front().GetTitle() << "\n";
        songQueue.pop();
    }
    else {
        cout << "Song queue is empty. No songs left in queue.\n";
    }
}

// Displays the current song and the remaining songs in the queue
void NowPlayingQueue::DisplayQueue() const
{
    if (!songQueue.empty()) {
        cout << "\nNow Playing:\n";
        songQueue.front().DisplaySong();
		cout << "Remaining songs in queue:\n";
		queue<Song> tempQueue = songQueue; // Copy to avoid modifying original queue
		while (!tempQueue.empty()) {
			tempQueue.front().DisplaySong();
			tempQueue.pop();
		}

    }
    else {
        cout << "Nothing is currently playing.\n";
    }
}

// Checks if the queue is empty
bool NowPlayingQueue::IsEmpty() const {
    return songQueue.empty();
}