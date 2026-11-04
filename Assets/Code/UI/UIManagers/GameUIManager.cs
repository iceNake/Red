using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    
    [Header("Canvas References")]
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas exitCanvas;
    
    [Header("Visual Effects")]
    [SerializeField] private SpriteRenderer redGameOverBackground;
    
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
    
    public void ShowExit()
    {
        DisableAllCanvases();
        if (exitCanvas != null) exitCanvas.enabled = true;
    }
    
    public void ShowGameOver()
    {
        DisableAllCanvases();
        CinemachineCamera cam = Object.FindFirstObjectByType<CinemachineCamera>();
        CinemachinePositionComposer composer = cam.GetComponent<CinemachinePositionComposer>();
        
        if (cam != null)
        {
            if (composer != null)
                composer.Composition.DeadZone.Size = new Vector2(0f, 0f);
        }
        
        StartCoroutine(ShowGameOverRoutine(cam));
    }

    private IEnumerator ShowGameOverRoutine(CinemachineCamera camera)
    {
        float zoomDuration = 3f;
        float targetSize = 6f;
        
        if (redGameOverBackground != null)
        {
            redGameOverBackground.gameObject.SetActive(true);
            redGameOverBackground.transform.position = camera.transform.position; 
            redGameOverBackground.color = new Color(1f, 0f, 0f, 0f);
        }

        if (camera != null)
        {
            float startSize = camera.Lens.OrthographicSize;
            float elapsedTime = 0f;
            camera.PreviousStateIsValid = false;
            
            while (elapsedTime < zoomDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / zoomDuration;
                float smoothT = t * t * (3f - 2f * t); 
                camera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, smoothT);
                
                if (redGameOverBackground != null)
                {
                    float alpha = Mathf.Lerp(0f, 1f, smoothT);
                    redGameOverBackground.color = new Color(1f, 0f, 0f, alpha);
                    redGameOverBackground.gameObject.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -3f);
                }
            
                yield return null;
            }
            
            camera.Lens.OrthographicSize = targetSize; 
        }

        yield return new WaitForSeconds(1f);
        
        if (gameOverCanvas != null) gameOverCanvas.enabled = true;
    }
}
