#include "Character.h"
#include "Inventory.h"
#include "Menu.h"
#include <iostream>
#include <string>

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Tyrese Senior's RPG Forging System.
// How it works:

// The user can prompt their name and then it will go to a main menu.
// from that main menu they can choose to craft weapons or armour, view their inventory in a detailed way or simplified way,
// equip items, display their stats, save their stats to a file, load their stairs from a file, connect to a server and send weapon data to it.
// The weapon/armour crafting system is based on the materials that are in the inventory and 
// the user can craft weapons and armour based on the materials they have using the names of the materials.
// In addition they can name the weapons and armour they craft.

// The techniques are spread throughout but the key places where the extra detailed comments are in:
// - Main.cpp
// - Character.h and character.cpp
// - Menu.h and Menu.cpp
// - WinsockClient.h
// However please still look at every file as there are comments in each one where appropriate and applicable. 
// Also I added a few extra comments in the other files for things that I learned through my own research into C++ programming to show I understand what I have added.
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using namespace std;

int main() {
    // Creates the character
    cout << "Enter your character's name: ";
    string playerName;
    getline(cin, playerName);

    Character player(playerName, 100, 10, 10);

    // Creates the inventory
    Inventory inventory;
    
    // Memory Management:
    // Adds initial materials to craft weapons and armour from the reason 
    // I am using make_shared here instead of new is be cause this allows faster 
    // allocation due to me not using many material pointers and also allows for better exception safety. 
    // Shared new could lead to potential memory leaks where as make_shared will do automatic cleanup once beyond scope in this case. 
    // Also it is more cache friendly.
    inventory.AddMaterial(make_shared<Material>("Iron", 5, 2));
    inventory.AddMaterial(make_shared<Material>("Steel", 8, 4));
    inventory.AddMaterial(make_shared<Material>("Leather", 0, 6));
    inventory.AddMaterial(make_shared<Material>("Ligma", 10, 10));

    // Creates the main menu
    Menu menu(player, inventory);
    

	// Sets the functional pointer to a standalone inventory display function
	menu.SetDisplayInventoryFunction(&DisplayInventoryFunction);

    //Runs the main menu and shows it
    menu.ShowMainMenu();

    return 0;
}