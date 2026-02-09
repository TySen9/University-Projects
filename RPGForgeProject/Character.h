#pragma once

#include <string>
#include <memory> //allows for the use of smart pointers aka shared_ptr. Through shared pointers it allows for memory to be handles dynamically.
//This removes the need to manually manage the memory through delete. Shared pointer objects allows for ownership to be shared properly.
#include <iostream> 



using namespace std;

//Forward deceleration of classes to be utilised in this class.
class Weapon; 
class Armour; 
class Menu; 

// A class which handles specifically the players data and stats.
class Character {
private:
    string name;
    int health;
    int attack;
    int defense;

    // MEMORY MANAGEMENT AND POINTERS TECHNIQUE:
    //Shared pointers are used to help with data management via sharing the objects ownership.
    // As said above it then allows for 
    shared_ptr<Weapon> equippedWeapon;
    shared_ptr<Armour> equippedArmour;

public:
    // Constructor
    Character(const string& playerName, int playerHealth, int playerAttack, int playerDefense):name(playerName), health(playerHealth), attack(playerAttack), defense(playerDefense) {}

    // Friendship Technique:
    // Friend function for stream insertion which allows for if I wanted to implement, the ability to save the characters data into a file. 
    // Through this it also allows for the menu to access it due to friendship. 
    // In addition to that from my research I have found out that this is a more modern solution to storing a persons information.
    // Also what is being done here is I am using the operator << to display/write the character's data. Then later use >> to read the data.
    friend ostream& operator<<(ostream& os, const Character& c);

    //this allows it to read the
    friend istream& operator>>(istream& is, Character& c);

    // Getters and setters for the name, attack and defense
    const string& GetName() const { return name; }
    int GetHealth() const { return health; }
    int GetAttack() const { return attack; }
    int GetDefense() const { return defense; }

    //shared pointers for the weapon and armour | deals with the memory handling so that the 
    //aren't individual and are reused with the only thing changing that their ownership is shared and transfered around.
    void EquipWeapon(shared_ptr<Weapon> weapon);
    void EquipArmour(shared_ptr<Armour> armour);

    // Display character stats
    /*void DisplayStats() const;*/

	// Save character stats to a file
	void SaveStats(const string& filename) const;

	// Load character stats from a file
	void LoadStats(const string& filename);

    // Virtual destructor
    ~Character() = default;

};
