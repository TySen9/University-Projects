#include "Weapon.h"

using namespace std;

//constructor for weapon.
Weapon::Weapon(const string& weaponName, int attackModifier): Item(weaponName), attackModifier(attackModifier) {}

//Returns the attack modifier
int Weapon::GetAttackModifier() const {
    return attackModifier;
}

//Gets the name of the weapon from the item. Reusing the function from item through overriding it.
const string& Weapon::GetName() const {
    return Item::GetName();
}