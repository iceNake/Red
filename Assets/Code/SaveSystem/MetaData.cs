using System;
using System.Collections.Generic;

// cosas de out run
[Serializable]
public class MetaData
{
    public int permanentDamageLevel;
    public int highscore;
    
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public MetaData()
    {
        permanentDamageLevel = 0;
        highscore = 0;
        masterVolume = 1f;
        musicVolume = 1f;
        sfxVolume = 1f;
    }
}

// cosas de in run
[Serializable]
public class RunData
{
    public float currentHealth;
    public int currentDamage;
    public int currentRoom;
    
    public int currentMoney;
    public int currentHeartPotions;
    
    public string equippedWeaponID;
    public string equippedCharmID;
    public string equippedHoodID;
    public List<string> equippedAmuletIDs;
    
    public RunData()
    {
        currentHealth = 100f;
        currentDamage = 10;
        currentRoom = 1;
        currentMoney = 0;
        currentHeartPotions = 0;
        equippedWeaponID = "";
        equippedCharmID = "";
        equippedHoodID = "";
        equippedAmuletIDs = new List<string>();
    }
}
