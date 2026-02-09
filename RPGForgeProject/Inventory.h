#pragma once

#include <vector>
#include <memory>
#include "Item.h"
#include "Material.h"

using namespace std;

class Inventory {
private:
    vector<shared_ptr<Item>> items;
    vector<shared_ptr<Material>> materials;

    static int itemCount; //A Static member to track number of items in the character's Inventory

public:
    // Add item
    void AddItem(const shared_ptr<Item>& item);

    // Add material
    void AddMaterial(const shared_ptr<Material>& material);

    // Display all items and materials
    void DisplayInventory() const;

    //Displays the detailed version of all items and materials
    void DisplayDetailedInventory() const;

    // Get total item count (non-static)
    int GetItemCount() const;

    // Get material count
    int GetMaterialCount() const;

    // Get material by index
    shared_ptr<Material> GetMaterialByIndex(int index) const;

    // Get item by index
    shared_ptr<Item> GetItemByIndex(int index) const;

    shared_ptr<Material> GetMaterialByName(const string& name) const;

    // Destructor
    ~Inventory();
};
