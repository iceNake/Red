using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    
    [Header("Canvas References")]
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas exitCanvas;

    [Header("Camera Reference")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioListener mainAudioListener;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        if(mainCamera != null) mainCamera.enabled = false;
        if(mainAudioListener != null) mainAudioListener.enabled = false;

        DisableAllCanvases();
    }
    
    public void DisableAllCanvases()
    {
        if (pauseCanvas != null) pauseCanvas.enabled = false;
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;
        if (exitCanvas != null) exitCanvas.enabled = false;
        
        if(mainCamera != null) mainCamera.enabled = false;
        if(mainAudioListener != null) mainAudioListener.enabled = false;
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
        if(mainCamera != null) mainCamera.enabled = true;
        if(mainAudioListener != null) mainAudioListener.enabled = true;
    }
    
    public void ShowExit()
    {
        DisableAllCanvases();
        if (exitCanvas != null) exitCanvas.enabled = true;
    }
}
