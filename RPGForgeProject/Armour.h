#pragma once

#include "Item.h"
#include <string>

class Armour : public Item {
public:
    Armour(const string& armourName, int defenseModifier);

    int GetDefenseModifier() const;

    //Override for GetName()
    const string& GetName() const override;

    //Override GetType to identify this item as armour
    string GetType() const override {
        return "Armour";
    }

private:
    int defenseModifier;
};
