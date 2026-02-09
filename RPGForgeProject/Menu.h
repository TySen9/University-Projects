#pragma once

#include "Character.h"
#include "Inventory.h"
#include "Weapon.h"
#include "Armour.h"
#include "WinsockClient.h"
#include <memory>
#include <iostream>

//This was declared outside the class to allow for it to work later for the functional pointer
void DisplayInventoryFunction(Inventory& inventory);

class Menu {
private:
    Character& player;       // Reference to the player character
    Inventory& inventory;    // Reference to the inventory
    WinsockClient winsockClient; //Reference to the Winstock Client

    // FUNCTIONAL POINTER TECHNIQUE:
    // A functional pointer which will display the inventory.
    // This allows for the inventory to be reusable in other contexts and would allow for the ability to 
    // switch between different versions of the inventory display such as if 
    // I wanted a detailed version due to it being a pointer so it could be assigned a different function during runtime.
    void (*displayInventoryPtr)(Inventory&); 
    

    // Helper methods for menu actions
    void DisplayStats() const;
    void DisplayInventory() const;
    void CraftWeapon();
    void CraftArmour();
    void EquipItem();
    void ConnectToServerAndSendWeapon();


public:
    // Constructor
    Menu(Character& player, Inventory& inventory);

    // Display and handles the main menu
    void ShowMainMenu();

    //Setter for the displayinventory function pointer.
    void SetDisplayInventoryFunction(void (*func)(Inventory&)); 
};
