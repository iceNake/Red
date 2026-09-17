using UnityEngine;
using UnityEngine.UI;

public class PauseCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeButtonClicked);
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuButtonClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.TogglePause();
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void OnMenuButtonClicked()
    {
        GameManager.Instance.ReturnToMenu();
    }

    private void OnExitButtonClicked()
    {
        GameUIManager.Instance.ShowExit();
    }
}
