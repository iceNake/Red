using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RED.Utility.Singleton;

public class LoadingScreenManager : Singleton<LoadingScreenManager>
{
    [Header("UI References")]
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    
    protected override void Awake()
    {
        base.Awake();
        if (loadingCanvas != null)
            loadingCanvas.enabled = false;
        else
            Debug.Log("No hay Canvas de Loading");
    }

    public void ShowLoadingScreen()
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.enabled = true;
            StartCoroutine(Fade(1f));
        }
        else
            Debug.Log("No hay Canvas de Loading");
    }

    public void HideLoadingScreen()
    {
        StartCoroutine(Fade(0f, () => 
        {
            if (loadingCanvas != null)
                loadingCanvas.enabled = false;
            else
                Debug.Log("No hay Canvas de Loading");
        }));
    }

    private IEnumerator Fade(float targetAlpha, Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        onComplete?.Invoke();
    }
}