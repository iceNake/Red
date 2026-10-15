using System;
using System.IO;
using UnityEngine;
using RED.Utility.Singleton;

public class SaveManager : Singleton<SaveManager>
{
    public MetaData CurrentMeta { get; private set; }
    public RunData CurrentRun { get; private set; }

    private string metaPath;
    private string runPath;

    protected override void Awake()
    {
        base.Awake();

        // rutas de los archivos
        metaPath = Application.persistentDataPath + "/metaProgress.json";
        runPath = Application.persistentDataPath + "/currentRun.json";

        LoadMeta();
        LoadRun();
    }

    #region Out Run Methods

    public void SaveMeta()
    {
        try
        {
            string json = JsonUtility.ToJson(CurrentMeta, true);
            File.WriteAllText(metaPath, json);
            Debug.Log("<color=yellow><b>[SaveManager]</b> Metaprogreso guardado.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error guardando MetaData: {e.Message}");
        }
    }

    public void LoadMeta()
    {
        if (File.Exists(metaPath))
        {
            string json = File.ReadAllText(metaPath);
            CurrentMeta = JsonUtility.FromJson<MetaData>(json);
        }
        else
        {
            CurrentMeta = new MetaData();
        }
    }

    #endregion

    #region In Run Methods

    public void SaveRun()
    {
        try
        {
            string json = JsonUtility.ToJson(CurrentRun, true);
            File.WriteAllText(runPath, json);
            Debug.Log("<color=yellow><b>[SaveManager]</b> Partida actual guardada.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error guardando RunData: {e.Message}");
        }
    }

    public void LoadRun()
    {
        if (File.Exists(runPath))
        {
            string json = File.ReadAllText(runPath);
            CurrentRun = JsonUtility.FromJson<RunData>(json);
        }
        else
        {
            CurrentRun = new RunData();
        }
    }

    public void DeleteRun()
    {
        if (File.Exists(runPath))
        {
            File.Delete(runPath);
            CurrentRun = new RunData();
            Debug.Log("<b>[SaveManager]</b> Archivo de Run eliminado (El jugador murió).");
        }
    }

    #endregion
}
