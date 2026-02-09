#pragma once

#include <string>
#include <memory>

using namespace std;

//The base class that weapon and armour also material will derive from.
class Item {
public:
    // Getter for the item name
    virtual const string& GetName() const;

    //Virtual function to get the type of the item
    virtual string GetType() const {
        return "Standard Item";
    }

    // Virtual destructor
    virtual ~Item();

protected:
    // Constructor
    Item(const string& itemName);

    // A shared pointer which dynamically allocates the string (the item's name)
    shared_ptr<string> pName;
};

