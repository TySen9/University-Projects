#include "Armour.h"

using namespace std;

Armour::Armour(const string& armourName, int defenseModifier): Item(armourName), defenseModifier(defenseModifier) {}

//Returns the defense modifier
int Armour::GetDefenseModifier() const {
    return defenseModifier;
}

 //Gets the name of the armour from the item. Reusing the function from item.
const string& Armour::GetName() const {
    return Item::GetName();
}