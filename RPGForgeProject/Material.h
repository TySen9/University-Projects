#pragma once

#include <string>
#include <iostream>

#include "Item.h"

using namespace std;

//A derived class from the base class: Item
class Material : public Item {
private:
    int attackModifier;
    int defenseModifier;

public:
    // Constructor
    Material(const string& name, int attackMod, int defenseMod): Item(name), attackModifier(attackMod), defenseModifier(defenseMod) {}

    // Getters for Attack Modifier and Defense Modifier
    int GetAttackModifier() const { return attackModifier; }
    int GetDefenseModifier() const { return defenseModifier; }

    // Display material's details
    void Display() const {
        cout << "Material: " << GetName() //Using the name from Item
            << ", Attack Modi: " << attackModifier
            << ", Defense Modi: " << defenseModifier << "\n";
    }

    //Override GetType to identify this as a Material
    string GetType() const override {
        return "Material";
    }
};

