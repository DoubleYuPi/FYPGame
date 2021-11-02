using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public PowerUpItem thisLoot;
    public float lootChance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;
    public PowerUpItem lootPowup()
    {
        float cumProb = 0f; //cumulative probability
        float currentProb = Random.Range(0f, 100f); //range of random number
        for(int i = 0; i <loots.Length; i++)
        {
            cumProb += loots[i].lootChance;
            if(currentProb <= cumProb) //return the item at the part of that number
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
