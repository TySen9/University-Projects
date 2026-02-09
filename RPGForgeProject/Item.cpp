#include "Item.h"


// Constructor
Item::Item(const string& itemName) : pName(make_shared<string>(itemName)) {}

// Getter for the item's name
const string& Item::GetName() const {
    return *pName;
}

// Virtual destructor
Item::~Item() {}