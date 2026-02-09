using Unity.VisualScripting;
using UnityEngine;

namespace PartyAISystem
{
    public class Statistics : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] public int maxHealth = 100; // The maximum health of the being
        [SerializeField] public int maxMana = 100; // The maximum Mana/Magic of the being
        [SerializeField] public int attackStat = 10; // The attack stat of the being
        [SerializeField] public int defenseStat = 10; // The defense stat of the being
        [SerializeField] public int regenerateMana = 10; // The amount of mana to regenerate per second

        //Spell Costs
        [SerializeField] public int healSpellCost = 60; // The cost of the heal spell
        [SerializeField] public int attackSpellCost = 20; // The cost of the attack spell

        public int currentHealth; // The current health of the being
        public int currentMana; // The current Mana/Magic of the being
        public int currentAttackStat; // The current attack stat of the being
        public int currentDefenseStat; // The current defense stat of the being
        public int healItemStock = 5; // The amount of heal items the being has in stock

        //Getter and Setter for Health and half health
        public int CurrentHealth => currentHealth; // The current health of the being
        public int HalfHealth => maxHealth / 2;

        public bool wasFainted = false; // A boolean to check if the being was fainted or not

        private void Awake()
        {
            // Initialise the current stats to the max stats
            currentHealth = maxHealth;
            currentMana = maxMana;
            currentAttackStat = attackStat;
            currentDefenseStat = defenseStat;
        }

        public void Update()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp the health to be between 0 and max health
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Clamp the mana to be between 0 and max mana

            //Regenerate the mana every 5 seconds
            if (Time.deltaTime % 5 == 0) // Check if the time is a multiple of 5 second
            {
                RegenerateMana();
            }
                
            bool isCompanionFainted = currentHealth <= 0 && CompareTag("Companion"); // Check if the being is a companion and has fainted
            bool isEnemyFainted = currentHealth <= 0 && CompareTag("Enemy"); // Check if the being is an enemy and has fainted

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (gameObject.CompareTag("Player"))
                {
                    TakeDamage(80); // Deal 50 damage to the companion
                    Debug.Log($"{name}: Took 50 damage and health is now {GetCurrentHealth()}/{GetMaxHealth()}");
                }
            }

            //Keybind to close the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(); // Close the game
            }

            // Check if the being has fainted
            if (isCompanionFainted && !wasFainted)
            {
                if (TryGetComponent<CompanionController>(out var companionController))
                // Calls the faint method and sets it as true in the CompanionControlle
                companionController.hasFainted = true;
                else
                    Debug.LogWarning($"Statistics: CompanionController not found on {name}");
            }
            else if (!isCompanionFainted && wasFainted)
            {
                // Calls the faint method and sets it as false in the CompanionController
                if (TryGetComponent<CompanionController>(out var companionController))
                    companionController.hasFainted = false;
                else
                    Debug.LogWarning($"Statistics: CompanionController not found on {name}");
            }

            if (gameObject.CompareTag("Player"))
            {
                //noop
            }

            if (gameObject.CompareTag("Enemy") && isEnemyFainted)
            {
                Destroy(gameObject); // Destroy the enemy game object if it has fainted
            }

            wasFainted = isCompanionFainted; // Set the wasFainted boolean to the current fainted state
        }

        // Method to check the health
        public int CheckHealth(int currentHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp the health to be between 0 and max health
            return currentHealth; // Return the current health
        }

        // Method to take damage from an attack by damage amount
        public void TakeDamage(int damage)
        {
            // Calculate the damage after defense
            int actualdamage = Mathf.Max(0, damage - currentDefenseStat);
            currentHealth -= actualdamage;
        }

        // Method to heal by a specific amount
        public void HealAction(int healAmount)
        {
            //calculate the heal amount
            currentHealth += healAmount; // Calculate the heal amount
            Debug.Log($"Stats: Healed for {healAmount} health"); // Log the heal amount
        }

        // Method to heal the being
        public void FullHeal()
        {
            currentHealth = maxHealth; // Set the current health to the max health
        }

        public void FillMana()
        {
            currentMana = maxMana; // Set the current mana to the max mana
        }

        // Method to fully heal the being using a heal spell
        public void FullHealSpell()
        {
            if (currentMana < healSpellCost)
            {
                Debug.Log("Stats: Not enough mana to cast the heal spell!"); // Check if there is enough mana to cast the heal spell
                return;
            }
            else
                currentMana = maxHealth; // Set the current mana to the max mana
                currentMana -= healSpellCost; // Subtract the cost of the heal spell from the current mana
        }
        
        // Method to fully heal the being using a heal item
        public void FullHealItem()
        {
            if (healItemStock <= 0)
            {
                Debug.Log("Stats: No heal items left!"); // Check if there are any heal items left
                return;
            }
            else
                currentHealth = maxHealth; // Set the current health to the max health
                healItemStock--; // Subtract one from the heal item stock

        }

        // Method to subtract the mana cost of the heal spell from mana
        public void UseManaHealCost()
        {
            if (currentMana < healSpellCost)
            {
                Debug.Log("Stats: Not enough mana to cast the heal spell!"); // Check if there is enough mana to cast the heal spell
                return;
            }
            else
                currentMana -= healSpellCost; // Subtract the cost of the heal spell from the current mana
            Debug.Log("Stats: Used mana for heal spell!"); // Log the mana used for the heal spell
        }

        // Method to subtract the mana cost of the attack spell from mana
        public void UseManaAttackCost()
        {
            if (currentMana < attackSpellCost)
            {
                Debug.Log("Stats: Not enough mana to cast the attack spell!"); // Check if there is enough mana to cast the attack spell
                return;
            }
            else
                currentMana -= attackSpellCost; // Subtract the cost of the attack spell from the current mana
        }

        // Method to use the heal item
        public void UseItemHealCost()
        {
            if (healItemStock <= 0)
            {
                Debug.Log("Stats: No heal items left!"); // Check if there are any heal items left
                return;
            }
            else
                healItemStock--; // Subtract one from the heal item stock
        }


        //Method to slowly regenerate mana every second
        public void RegenerateMana()
        {
            currentMana += regenerateMana; // Add the regenerate mana to the current mana
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Clamp the mana to be between 0 and max mana
        }


        public int GetCurrentHealth() => currentHealth; // Return the current health for ui and other scripts to make use of
        public int GetMaxHealth() => maxHealth; // Return the max health for ui and other scripts to make use of
        public int GetCurrentMana() => currentMana; // Return the current mana for ui and other scripts to make use of
        public int GetMaxMana() => maxMana; // Return the max mana for ui and other scripts to make use of


    }
}
