#include "Playlist.h"
#include "Utility.h"
#include <algorithm>
#include <vector>
//This file implements the Playlist class, which manages a linked list of songs and provides various functionalities like adding, displaying, searching, and sorting songs.
//This is also makes uses of the operator overloading to combine playlists together or using it to search for songs in the playlist through comparison.

//Contructor for Node which is used in the linked list
Playlist::Node::Node(const Song& s) : data(s), next(nullptr) {}

// Constructor for the playlist
Playlist::Playlist() : head(nullptr) {}

// Deconstructor for the playlist which deletes all the nodes in the linked list
Playlist::~Playlist() {
    while (head) {
        Node* temp = head;
        head = head->next;
        delete temp;
    }
}

// Adds a song to the end of the linked list and 
// also begins the linked list itself to create a dynamic playlist that is flexible
void Playlist::AddSong(const Song& song) {
    Node* newNode = new Node(song);
    if (!head) head = newNode;
    else {
        Node* curr = head;
        while (curr->next) curr = curr->next;
        curr->next = newNode;
    }
}

// Goes through the linked list and displays all the songs in the playlist
void Playlist::DisplayPlaylist() const {
    if (!head) {
        cout << "Playlist is empty.\n";
        return;
    }
    Node* curr = head;
    while (curr) {
        curr->data.DisplaySong();
        curr = curr->next;
    }
}

// Searches for songs by title in the playlist and displays them
void Playlist::SearchSongByTitle(const string& title) {
    vector<Song*> results = UnifiedSearch(title);
    if (results.empty()) {
        cout << "No matching songs found.\n";
    }
    else {
        cout << "\nSearch results:\n";
        for (Song* s : results) {
            s->DisplaySong();
        }
    }
}

// goes through the linked list and removes all songs that match the user input title
int Playlist::RemoveAllSongsByTitle(const string& title) {
    int count = 0;
    std::string target = toLower(title);

	// Remove from the head of the list if it matches
    while (head && toLower(head->data.title) == target) {
        Node* temp = head;
        head = head->next;
        delete temp;
        count++;
    }

    Node* curr = head;
    while (curr && curr->next) {
        if (toLower(curr->next->data.title) == target) {
            Node* temp = curr->next;
            curr->next = temp->next;
            delete temp;
            count++;
        }
        else {
            curr = curr->next;
        }
    }

    return count;
}

// Sorts the songs in the playlist by title using a vector to store the songs,
void Playlist::SortSongs() {
    std::vector<Song> songs;
    Node* curr = head;

	// Copys the linked list into a vector
    while (curr) {
        songs.push_back(curr->data);
        curr = curr->next;
    }

	// Uses overloaded operator< to sort the vector of songs
    sort(songs.begin(), songs.end());

	// Writes the sorted songs back to the linked list
    curr = head;
    for (const Song& s : songs) {
        if (curr) {
            curr->data = s;
            curr = curr->next;
        }
    }

    cout << "Playlist sorted by song title.\n";
}

// Combines the playlists together into another (operator overloading)
Playlist Playlist::operator+(const Playlist& other) const {
    Playlist result;
    Node* curr = head;
    while (curr) {
        result.AddSong(curr->data);
        curr = curr->next;
    }
    curr = other.head;
    while (curr) {
        result.AddSong(curr->data);
        curr = curr->next;
    }
    return result;
}

// Searches for the first match to the title
Song* Playlist::FindFirst(const string& title) {
    Node* curr = head;
    while (curr != nullptr) {
        if (toLower(curr->data.GetTitle()) == toLower(title)) {
            return &curr->data;
        }
        curr = curr->next;
    }
    return nullptr;
}

Song* Playlist::FindFirstByTitle(const string& title)
{
    Node* curr = head;
    while (curr) {
        if (toLower(curr->data.GetTitle()) == toLower(title)) {
            return &curr->data;
        }
        curr = curr->next;
    }
    return nullptr;
}

// Finds all songs by the artist and returns them in a vector
vector<Song*> Playlist::FindByArtist(const string& artist)
{
    vector<Song*> matches;
    Node* curr = head;
    while (curr) {
        if (toLower(curr->data.GetArtist()) == toLower(artist)) {
            matches.push_back(&curr->data);
        }
        curr = curr->next;
    }
    return matches;
}

// Finds all songs that contain the partial title in their title and returns them in a vector
vector<Song*> Playlist::FindByTitleSubstring(const string& partialTitle)
{
    vector<Song*> matches;
    string search = toLower(partialTitle);
    Node* curr = head;
    while (curr) {
        string songTitle = toLower(curr->data.GetTitle());
        if (songTitle.find(search) != string::npos) {
            matches.push_back(&curr->data);
        }
        curr = curr->next;
    }
    return matches;
}

// Combines all the search methods to provide a unified search functionality
vector<Song*> Playlist::UnifiedSearch(const string& searchQuery)
{
    vector<Song*> results;

    Song* exact = FindFirst(searchQuery);
    if (exact) results.push_back(exact);

    auto partials = FindByTitleSubstring(searchQuery);
    for (auto* s : partials) {
        if (find(results.begin(), results.end(), s) == results.end())
            results.push_back(s);
    }

    auto byArtist = FindByArtist(searchQuery);
    for (auto* s : byArtist) {
        if (find(results.begin(), results.end(), s) == results.end())
            results.push_back(s);
    }

    return results;
}
