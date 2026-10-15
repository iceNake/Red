using UnityEngine;

public class GamePersistence : MonoBehaviour
{
    [Header("Player Stats")] public float health;
    public int damage;

    public void SaveCheckpoint(int roomNumber)
    {
        SaveManager.Instance.CurrentRun.currentHealth = health;
        SaveManager.Instance.CurrentRun.currentDamage = damage;
        SaveManager.Instance.CurrentRun.currentRoom = roomNumber;
        InventoryManager.Instance.SyncDataToSave();
        SaveManager.Instance.SaveRun();
    }

    public void OnPlayerDeath()
    {
        SaveManager.Instance.SaveMeta();
        SaveManager.Instance.DeleteRun();
    }

    public void StartGame()
    {
        RunData currentRun = SaveManager.Instance.CurrentRun;
        MetaData meta = SaveManager.Instance.CurrentMeta;

        if (currentRun.currentRoom > 1)
        {
            health = currentRun.currentHealth;
            damage = currentRun.currentDamage;
            Debug.Log("<color=cyan>Continuando la Run existente...</color>");
        }
        else
        {
            damage = 10 + meta.permanentDamageLevel;
            health = 100;
            currentRun.currentHealth = health;
            currentRun.currentDamage = damage;

            Debug.Log("<color=green>Iniciando nueva Run con mejoras de MetaData.</color>");
        }

        InventoryManager.Instance.LoadDataFromSave();
    }
}
