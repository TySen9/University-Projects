#pragma once

#include "Item.h"
#include <string>

using namespace std;

//A derived class from the base class: Item.
class Weapon : public Item {
public:
    Weapon(const string& weaponName, int attackModifier);

    int GetAttackModifier() const;

    //Override for GetName() which allows for specifically the weapon name to be gained which is a function originating from item.
    const string& GetName() const override;

    //Override to GetType to identify this as a weapon
    string GetType() const override {
        return "Weapon";
    }

private:
    int attackModifier;
};

