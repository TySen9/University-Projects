// SpotifyLikeApplication.cpp : This file contains the 'main' function. Program execution begins and ends there.
// It is the core application that manages the playlist, allows user interaction, and integrates with the Now Playing Queue and JSONLoader for song management.
// Making use of the data structures: operator overloading, linked lists, queues, vectors and algorithms to create a Spotify-like application that allows users to manage their playlists and songs.

#include <iostream>
#include <sstream>
#include "Playlist.h"
#include "Song.h"
#include "NowPlayingQueue.h"
#include "JSONLoader.h"
#include "Utility.h"

using namespace std;

// Function to display the menu options
void ShowMenu() {
    cout << "\n== Welcome to evil red spotify lite ==\n";
    cout << "\n====== Playlist Menu ======\n";
    cout << "1. Add a song\n";
    cout << "2. Display playlist\n";
    cout << "3. Search for songs in playlist\n";
    cout << "4. Search for songs in database\n";
    cout << "5. Remove songs by title\n";
    cout << "6. Sort playlist by title\n";
    cout << "7. Add song to Now Playing Queue\n";
    cout << "8. Show the Now Playing Queue\n";
	cout << "9. Play next song in the queue\n";
    cout << "10. Exit\n";
    cout << "===========================\n";
    cout << "\nEnter your choice: ";
}


int main()
{
    Playlist initialPlaylist; // Creates the playlist linked list
	NowPlayingQueue nowPlayingQueue; // A Queue to hold the currently playing songs
    int choice;

	// Manually loads a preloaded playlist with some songs
    initialPlaylist.AddSong(Song("All I Need", "khai dreams", 228, "Indie Pop", "Nice Colors", 2018));
    initialPlaylist.AddSong(Song("Mellohi", "C418", 138, "Video Game Music", "Minecraft - Volume Beta", 2013));
    initialPlaylist.AddSong(Song("BEST INTREST", "Tyler, The Creator", 208, "Hip Hop", "BEST INTEREST", 2020));
    initialPlaylist.AddSong(Song("Dancing in My Room", "347aiden", 300, "Indie Pop", "Dancing in My Room", 2020));
    initialPlaylist.AddSong(Song("DEVILMAN", "Rav", 315, "Rap", "DEVILMAN", 2020));
	initialPlaylist.AddSong(Song("How Sweet", "NewJeans", 339, "K-Pop", "How Sweet", 2024));
	initialPlaylist.AddSong(Song("Outsider", "Eve", 328, "J-Pop", "Otogi", 2019));
    initialPlaylist.AddSong(Song("SHYNESS BOY", "Anri", 317, "City Pop", "Timely!!", 1983));
	initialPlaylist.AddSong(Song("Strange Things", "Double", 450, "R&B", "VISION", 2002));




    do {
        ShowMenu();
        cin >> choice;
        cin.ignore(); // Clears input buffer

        switch (choice) {
		case 1: { // collects the song information from the user and adds it to the playlist
            string title, artist, genre, album;
            int duration, releaseYear;

            cout << "Enter song title: ";
            getline(cin, title);
            cout << "Enter artist: ";
            getline(cin, artist);
            cout << "Enter duration (in seconds): "; 
            cin >> duration;
            cin.ignore();
			cout << "Enter genre: ";
			getline(cin, genre);
			cout << "Enter album: ";
			getline(cin, album);
			cout << "Enter release year: ";
			cin >> releaseYear;
			cin.ignore(); // Clears input buffer

            initialPlaylist.AddSong(Song(title, artist, duration, genre, album, releaseYear));
            cout << "Song added!\n";
            break;
        }
        case 2:
			initialPlaylist.DisplayPlaylist(); // Goes through the linked list and displays all the songs in the playlist
            break;

        case 3: {
            string title;
            cout << "Enter artist, title or partial title to search: ";
            getline(cin, title);
            initialPlaylist.SearchSongByTitle(title);// performs the unified linear search
            break;
        }

		case 4: { // Completes a search through the global music database (the JSON file)
            string keyword;
            cout << "Enter a keyword to search global database (artist, title or genre): ";
			getline(cin, keyword);

			keyword = trim(keyword);

			auto results = JSONLoader::SearchFromJSONFile("songs.json", keyword);
            if (results.empty()) {
                cout << "No matches found in the database. \n";
                break;
            }

			//Display the results of the search
			cout << "\nResults from global music catalog: \n";
			for (size_t i = 0; i < results.size(); ++i) {
				cout << i + 1 << ". ";
				results[i].DisplaySong();
			}

			// Ask user if they want to add any of the results to their playlist
            cout << "\nEnter the song number to add to add to your playlist (e.g. 1, 2, 3), or 0 to cancel choice:\n> ";
            string line;
			getline(cin, line);
            stringstream ss(line);
            int index;

            while (ss >> index) {
                if (index == 0) {
					cout << "Cancelled adding songs.\n";
                }
                if (index >= 1 && index <= results.size()) {
                    initialPlaylist.AddSong(results[index - 1]);
                    cout << "Added Song: " << results[index - 1].GetTitle() << "\n";
                }
                else {
                    cout << "Invalid index: " << index << "\n";
                }
            }
            break;
        }

        case 5: {
            string title;
            cout << "Enter title to remove: ";
            getline(cin, title);
            int removed = initialPlaylist.RemoveAllSongsByTitle(title); // linear deletion
            cout << removed << " song(s) removed.\n";
            break;
        }

        case 6: {
			initialPlaylist.SortSongs(); // Uses vector and sort to sort the playlist by title
            break;
        }

        case 7: {
			initialPlaylist.DisplayPlaylist();
            string title;
            cout << "\nEnter song title to add the queue:  ";
            getline(cin, title);
            Song* song = initialPlaylist.FindFirst(title); // Add this helper in Playlist
            if (song) {
				nowPlayingQueue.EnqueueSong(*song); // adds to the queue
            }
            else {
                cout << "Song not found in playlist.\n";
            }
            break;
        }

        case 8:
            nowPlayingQueue.DisplayQueue();
            break;

        case 9:
			nowPlayingQueue.PlayNextSong();
			break;

        case 10:
            cout << "Exiting...\n";
            EXIT_SUCCESS;
            break;
   

        default:
            cout << "Invalid choice. Try again.\n";
            break;
        }

    } while (choice != 10);

    return 0;
}

