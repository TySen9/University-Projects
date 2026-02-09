#include "Character.h"
#include "Weapon.h"
#include "Armour.h"
#include <fstream> //enables the ability to allow for reading and writing to a file.

using namespace std;

//A shared pointer which allows for data to be dynamically allocated and once it goes out of range then it will delete/clear itself.
//This one equips a weapon for the player character allowing the stats to be altered.
void Character::EquipWeapon(shared_ptr<Weapon> weapon) {
	equippedWeapon = move(weapon); // Transfer ownership
	if (equippedWeapon) {
		cout << "Equipped Weapon: " << equippedWeapon->GetName() << "\n";
		attack += equippedWeapon->GetAttackModifier();
	}
}

//A shared pointer which allows for data to be dynamically allocated and once it goes out of range then it will delete/clear itself.
//This one equips a armour for the player character allowing the stats to be altered.
void Character::EquipArmour(shared_ptr<Armour> armour) {
	equippedArmour = move(armour); // Transfers ownership
	if (equippedArmour) {
		cout << "Equipped Armour: " << equippedArmour->GetName() << "\n";
		defense += equippedArmour->GetDefenseModifier();
	}
}

//void Character::DisplayStats() const {
//	cout << "\n---- Character Stats ----\n";
//	cout << "Name: " << name << "\n";
//	cout << "Health: " << health << "\n";
//	cout << "Attack: " << attack << "\n";
//	cout << "Defense: " << defense << "\n";
//
//	if (equippedWeapon) {
//		cout << "Equipped Weapon: " << equippedWeapon->GetName() << "\n";
//	}
//	else {
//		cout << "No weapon equipped.\n";
//	}
//
//	if (equippedArmour) {
//		cout << "Equipped Armour: " << equippedArmour->GetName() << "\n";
//	}
//	else {
//		cout << "No armour equipped.\n";
//	}
//}

// Save stats to a file (using the overloaded operator << for the output format)
void Character::SaveStats(const string& filename) const {
    ofstream file(filename); // Open the file for writing
    if (file.is_open()) {
        file << *this; // Use the overloaded operator << to write the character's stats to the file
        cout << "Character stats saved to " << filename << "\n";
    }
    else {
        cerr << "Error: Unable to open file for saving stats.\n";
    }
}

// Load the characters stats from a file
void Character::LoadStats(const string& filename) {
	ifstream file(filename); // Open the file for reading
	if (file.is_open()) {
		file >> *this; // Use the operator >> to read the character's stats from the file
		cout << "Character stats loaded from " << filename << "\n";
	}
	else {
		cerr << "Error: Unable to open file for loading stats.\n";
	}
}

//Overloading:
// Overload the stream insertion operator to display a Character object and potentially allow it to be saved in the future
ostream& operator<<(ostream& os, const Character& c) {
	os << c.name << "\n";
	os << "Health: " << c.health << "\n";
	os << "Attack: " << c.attack << "\n";
	os << "Defense: " << c.defense << "\n";

	if (c.equippedWeapon) {
		os << "Equipped Weapon: " << c.equippedWeapon->GetName() << "\n";
	}
	else {
		os << "No weapon equipped.\n";
	}

	if (c.equippedArmour) {
		os << "Equipped Armour: " << c.equippedArmour->GetName() << "\n";
	}
	else {
		os << "No armour equipped.\n";
	}

	return os;

}

// Overloads the stream operator to read character stats to a file
istream& operator>>(istream& is, Character& c) {
	string line;

	// Reads the players name
	getline(is, c.name);    

	// Read and parse "Health: <value>" turning the string number into an integer
	getline(is, line);
	c.health = stoi(line.substr(line.find(":") + 1));

	// Read and parse "Attack: <value>" turning the string number into an integer
	getline(is, line);
	c.attack = stoi(line.substr(line.find(":") + 1));

	// Read and parse "Defense: <value>" turning the string number into an integer
	getline(is, line);
	c.defense = stoi(line.substr(line.find(":") + 1));

	return is;
}