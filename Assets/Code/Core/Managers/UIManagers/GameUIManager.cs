using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    
    [Header("Canvas References")]
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas exitCanvas;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DisableAllCanvases();
    }
    
    public void DisableAllCanvases()
    {
        if (pauseCanvas != null) pauseCanvas.enabled = false;
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;
        if (exitCanvas != null) exitCanvas.enabled = false;
    }
    
    public void ShowPause()
    {
        DisableAllCanvases();
        if (pauseCanvas != null) pauseCanvas.enabled = true;
    }
    
    public void ShowGameOver()
    {
        DisableAllCanvases();
        if (gameOverCanvas != null) gameOverCanvas.enabled = true;
    }
    
    public void ShowExit()
    {
        DisableAllCanvases();
        if (exitCanvas != null) exitCanvas.enabled = true;
    }
}
