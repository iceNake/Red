using UnityEngine;

public class InitialMenuUIManager : MonoBehaviour
{
    public static InitialMenuUIManager Instance;
    
    [Header("Canvas References")]
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Canvas exitCanvas;
    [SerializeField] private Canvas audioCanvas;
    [SerializeField] private Canvas creditsCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        ShowMenu();
    }

    private void DisableAllCanvases()
    {
        if (menuCanvas != null) menuCanvas.enabled = false;
        if (optionsCanvas != null) optionsCanvas.enabled = false;
        if (exitCanvas != null) exitCanvas.enabled = false;
        if (audioCanvas != null) audioCanvas.enabled = false;
        if (creditsCanvas != null) creditsCanvas.enabled = false;
    }

    public void ShowMenu()
    {
        DisableAllCanvases();
        if (menuCanvas != null) menuCanvas.enabled = true;
    }
    
    public void ShowOptions()
    {
        DisableAllCanvases();
        if (optionsCanvas != null) optionsCanvas.enabled = true;
    }
    
    public void ShowExit()
    {
        DisableAllCanvases();
        if (exitCanvas != null) exitCanvas.enabled = true;
    }
    
    public void ShowAudio()
    {
        DisableAllCanvases();
        if (audioCanvas != null) audioCanvas.enabled = true;
    }
    
    public void ShowCredits()
    {
        DisableAllCanvases();
        if (creditsCanvas != null) creditsCanvas.enabled = true;
    }
}
