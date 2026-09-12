using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    
    private void Start()
    {
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuButtonClicked);
    }
    
    private void OnRestartButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.StartGame();
    }

    private void OnMenuButtonClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.ReturnToMenu();
    }
}
