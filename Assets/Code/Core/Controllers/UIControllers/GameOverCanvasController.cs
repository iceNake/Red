using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    
    private void Start()
    {
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuButtonClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    
    private void OnRestartButtonClicked()
    {
        // Hacer que se reinicie
    }

    private void OnMenuButtonClicked()
    {
        // Regresar al menu
    }

    private void OnExitButtonClicked()
    {
        GameUIManager.Instance.ShowExit();
    }
}
