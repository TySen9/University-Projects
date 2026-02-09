#include "Inventory.h"
#include <iostream>

using namespace std;

// initialise the static member which is the item count. 
// This is an example of static members which in this case is used as the inventory's permanent starting value.
int Inventory::itemCount = 0;

// Add an item to the inventory stack
void Inventory::AddItem(const shared_ptr<Item>& item) {
    items.push_back(item);
    itemCount++;
}

// Add a material to the inventory stack
void Inventory::AddMaterial(const shared_ptr<Material>& material) {
    materials.push_back(material);
}

// Polymorphism:
// Display inventory. This specifically is a show of dynamic polymorphism as it is 
// inheritance and function overloading as a weapon or armour is a type of item 
// since item is the base class that those two derive from. with inventory holding a 
// collection of item objects using shared pointers.
void Inventory::DisplayInventory() const {
	cout << "~~~~~~~~~~~~~~~~~~~~~~\n";
    cout << "------Inventory-------\n";

	cout << "\nItems:\n";
	for (const auto& item : items) {
		cout << item->GetName() << "\n"; // Uses Item::GetName
	}

	cout << "\nMaterials:\n";
	for (const auto& material : materials) {
		material->Display(); // Uses Material::Display
	}

	cout << "\n~~~~~~~~~~~~~~~~~~~~~~\n";

	/*for (size_t i = 0; i < items.size(); i++) {
		cout << i + 1 << ". Item: " << items[i]->GetName() << "\n";
	}

	for (size_t i = 0; i < materials.size(); i++) {
		cout << i + 1 + items.size() << ". Material: " << materials[i]->GetName()
			<< " (Attack Modifier: " << materials[i]->GetAttackModifier()
			<< ", Defense Modifier: " << materials[i]->GetDefenseModifier() << ")\n";
	}*/
}

// Functional Pointers:
//This is a detailed version of the menu that the user can choose to use and will show 
// through the functional pointer if they want and is an option on the main menu.
void Inventory::DisplayDetailedInventory() const {
	cout << "\n~~~~~~~ DETAILED INVENTORY ~~~~~~~\n";

	if (items.empty() && materials.empty()) {
		cout << "Your inventory is empty.\n";
		return;
	}

	if (!items.empty()) {
		cout << "\nItems:\n";
		cout << "-----------------------------------\n";
		for (size_t i = 0; i < items.size(); i++) {
			cout << i + 1 << ". " << items[i]->GetName()
				<< " | Type: " << items[i]->GetType() << "\n";
		}
	}
	else {
		cout << "\nNo items in your inventory.\n";
	}

	if (!materials.empty()) {
		cout << "\nMaterials:\n";
		cout << "-----------------------------------\n";
		for (size_t i = 0; i < materials.size(); i++) {
			cout << i + 1 + items.size() << ". " << materials[i]->GetName()
				<< " | Type: " << materials[i]->GetType()
				<< " | Attack Modifier: " << materials[i]->GetAttackModifier()
				<< " | Defense Modifier: " << materials[i]->GetDefenseModifier()<< "\n";
		}
	}
	else {
		cout << "\nNo materials in your inventory.\n";
	}

	cout << "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
}

// Get the item by index of the size of the array
shared_ptr<Item> Inventory::GetItemByIndex(int index) const {
    if (index >= 0 && index < static_cast<int>(items.size())) {
        return items[index];
    }
    return nullptr;
}

// Get the material by index which is the size of the array
shared_ptr<Material> Inventory::GetMaterialByIndex(int index) const {
    if (index >= 0 && index < static_cast<int>(materials.size())) {
        return materials[index];
    }
    return nullptr;
}

// Get material count
int Inventory::GetMaterialCount() const {
    return static_cast<int>(materials.size());
}

// Get total item count (static)
int Inventory::GetItemCount() const {
    return static_cast<int>(items.size());
}

// Pointers:
// A shared pointer with material that will get the materials name, 
// it is used to get the material the user wants through what the user enters.
shared_ptr<Material> Inventory::GetMaterialByName(const string& name) const {
	for (const auto& material : materials) {
		if (material->GetName() == name) {
			return material;
		}
	}
	return nullptr; // Return null if the material is not found
}

// Destructor
Inventory::~Inventory() {}