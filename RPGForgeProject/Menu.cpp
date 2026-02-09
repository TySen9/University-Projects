#define NOMINMAX //This prevents my max from having errors since it wasn't working properly once I got to 10

#include "Menu.h"
#include "CustomException.h"
#include "WinsockClient.h"
#include <iostream>
#include <limits>
#include <algorithm> //This is required to make max work.


using namespace std;

// Constructor & Overloading Technique:
// Constructor for the menu which is an example of overloading and a constructor as it is taking in the Inventory type and the Character type data.
// For the case of it being a constructor it constructs the objects that will be used in the class. In this case it is the player object and the inventory.
// Specifically it points to the objects that reside within the character class and the inventory class so that they can be manipulated within this class too.
Menu::Menu(Character& player, Inventory& inventory) : player(player), inventory(inventory) {
	displayInventoryPtr = nullptr;// Initialises the function pointer to nullptr
}

void Menu::SetDisplayInventoryFunction(void (*func)(Inventory&)) {
	displayInventoryPtr = func; // Sets the function pointer to whichever function we want to use.
}

// Display player stats
void Menu::DisplayStats() const {
	cout << player;
}

// Functional Pointers:
// Display inventory items through use of functional pointer
void Menu::DisplayInventory() const {
	if (displayInventoryPtr) {
		displayInventoryPtr(inventory); // passes the inventory object to the function pointer and calls this function via the pointer created. (Functional Pointer)
	} 
	else {
		cout << "No inventory display function has been set." << endl;
	}

}

//Displays the default inventory layout
void DisplayInventoryFunction(Inventory& inventory) {
	inventory.DisplayInventory();

}

//Displays the detailed inventory layout.
void DisplayDetailedInventoryFunction(Inventory& inventory) {
	inventory.DisplayDetailedInventory();
}

// Craft a weapon
void Menu::CraftWeapon() {
	system("CLS");
	try {
		cout << "Enter the name of your new weapon: ";
		string weaponName;
		getline(cin, weaponName); // Gets the full weapon name

		cout << "\nSelect a material by name for the weapon (type the material name as written): \n";
		inventory.DisplayInventory();

		string materialName;
		getline(cin, materialName); // Gets the material name input

		// Find the material by name in the inventory
		auto selectedMaterial = inventory.GetMaterialByName(materialName);

		if (!selectedMaterial) {
			throw CustomException("Invalid material name! Please ensure the material exists in the inventory.");
		}

		// Create the weapon
		auto newWeapon = make_shared<Weapon>(
			weaponName,
			selectedMaterial->GetAttackModifier()
		);

		inventory.AddItem(newWeapon);
		cout << "Weapon crafted and added to inventory: " << weaponName << "!\n";
	}
	catch (const CustomException& err) {
		cerr << "Error: " << err.what() << "\n";
	}
}

// Craft Armour
void Menu::CraftArmour() {
	system("CLS");
	try {
		cout << "Enter the name of your new armour: ";
		string armourName;
		getline(cin, armourName); // Get the full armor name

		cout << "\nSelect a material by name for the armour (type the material name as written): \n";
		inventory.DisplayInventory();

		string materialName;
		getline(cin, materialName); // Get the material name input

		// Find the material by name in the inventory
		auto selectedMaterial = inventory.GetMaterialByName(materialName);

		if (!selectedMaterial) {
			throw CustomException("Invalid material name! Please ensure the material exists in the inventory.");
		}

		// Create the armour
		auto newArmour = make_shared<Armour>(
			armourName,
			selectedMaterial->GetDefenseModifier()
		);

		inventory.AddItem(newArmour);
		cout << "Armour crafted and added to inventory: " << armourName << "!\n";
	}
	catch (const CustomException& err) {
		cerr << "Error: " << err.what() << "\n";
	}
}

// Equip an item
void Menu::EquipItem() {
	system("CLS");
	try {
		cout << "\nSelect an item to equip (use numbers): \n";
		inventory.DisplayInventory();

		int itemChoice;
		cin >> itemChoice;

		// Handles invalid input (non-integer or out-of-range)
		if (cin.fail() || itemChoice <= 0 || itemChoice > inventory.GetItemCount()) {
			cin.clear();
			cin.ignore(numeric_limits<streamsize>::max(), '\n'); // Clear invalid input
			throw CustomException("Invalid item selected. Please enter a valid number!");
		}

		auto item = inventory.GetItemByIndex(itemChoice - 1);

		// Checks the type and equips appropriately
		if (auto weapon = dynamic_pointer_cast<Weapon>(item)) {
			player.EquipWeapon(weapon);
			cout << "\nWeapon equipped.\n";
		}
		else if (auto armour = dynamic_pointer_cast<Armour>(item)) {
			player.EquipArmour(armour);
			cout << "\nArmour equipped.\n";
		}
		else {
			throw CustomException("Your selected item can't be equipped!");
		}
	}
	catch (const CustomException& err) {
		cerr << "Error: " << err.what() << "\n";
	}
}

// Winstock Technique:
// Connects to the Winsock server to send a weapons data of the users choosing. 
// The must have made a weapon first for it to be sent. 
// It has error checking for if it is beyond the count or not a weapon that was chosen.
void Menu::ConnectToServerAndSendWeapon() {
	if (!winsockClient.ConnectToServer()) {
		cerr << "Could not connect you to the server.\n";
		return;
	}

	cout << "\nSelect a weapon to send to the server (Use numbers):\n";
	inventory.DisplayInventory();

	int weaponChoice;
	cin >> weaponChoice;

	if (cin.fail() || weaponChoice <= 0 || weaponChoice > inventory.GetItemCount()) {
		cin.clear();
		cin.ignore(numeric_limits<streamsize>::max(), '\n');
		cerr << "Invalid selection. Returning to the menu.\n";
		return;
	}

	auto selectedWeapon = dynamic_pointer_cast<Weapon>(inventory.GetItemByIndex(weaponChoice - 1));
	if (!selectedWeapon) {
		cerr << "Selected item is not a weapon. Returning to menu.\n";
		return;
	}

	winsockClient.SendWeaponData(*selectedWeapon);

	string serverResponse = winsockClient.ReceiveMessage();
	cout << "Server Response: " << serverResponse << endl;

	winsockClient.CloseConnection();
}

// Show the main menu
void Menu::ShowMainMenu() {
	bool running = true;
	system("CLS");
	while (running) {
		try {
			cout << "\n~~~~~~~~ MAIN MENU ~~~~~~~~\n";
			cout << "1. View Player Stats\n";
			cout << "2. View Inventory (Standard)\n";
			cout << "3. View Inventory (Detailed)\n";
			cout << "4. Craft a Weapon\n";
			cout << "5. Craft an Armour\n";
			cout << "6. Equip an Item\n";
			cout << "7. Save Stats\n";
			cout << "8. Load Stats\n";
			cout << "9. Send Weapon to the Server\n";
			cout << "10. Exit\n";
			cout << "Enter your choice (1 - 10): ";
			cout << "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";

			int choice;
			cin >> choice;

			// Handles the invalid input (non-integer or out-of-range)
			if (cin.fail()) {
				cin.clear(); // Clears the error flag
				cin.ignore(numeric_limits<streamsize>::max(), '\n'); // Clears the input buffer
				throw CustomException("Invalid input! Please enter a number between 1 and 10.");
			}

			cin.ignore(numeric_limits<streamsize>::max(), '\n'); // Clears any leftover newline from `cin`

			switch (choice) {
			case 1:
				DisplayStats();
				break;
			case 2:
				SetDisplayInventoryFunction(&DisplayInventoryFunction);
				DisplayInventory();
				break;
			case 3:
				SetDisplayInventoryFunction(&DisplayDetailedInventoryFunction);
				DisplayInventory();
				break;
			case 4:
				CraftWeapon();
				break;
			case 5:
				CraftArmour();
				break;
			case 6:
				EquipItem();
				break;
			case 7:
				player.SaveStats("stats.txt"); //saves the stats to a text file
				break;
			case 8:
				player.LoadStats("stats.txt"); //Loads the stats from the same text file
				break;
			case 9:
				ConnectToServerAndSendWeapon();
				break;
			case 10:
				running = false;
				break;
			default:
				throw CustomException("Invalid choice! Please enter a number between 1 and 10.");
			}
		}
		catch (const CustomException& err) {
			cerr << "Error: " << err.what() << "\n";
		}
	}
	cout << "\nEnding Program...";
}
