using UnityEngine;
using UnityEngine.UI;

public class MenuCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(OnStartButtonClicked);
        if (optionsButton != null) optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        if (creditsButton != null) creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    private void OnOptionsButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowOptions();
    }
    
    private void OnCreditsButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowCredits();
    }

    private void OnExitButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowExit();
    }
}
