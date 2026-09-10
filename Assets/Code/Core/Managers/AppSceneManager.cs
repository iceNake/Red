using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RED.Utility.Singleton;

public class AppSceneManager : Singleton<AppSceneManager>
{
    public string MenuInicial = "InitialMenu";
    public string GameScene = "Game";
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadSceneAsync(string sceneName, Action onSceneLoaded = null)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoaded));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, Action onSceneLoaded)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        onSceneLoaded?.Invoke();
    }
}
