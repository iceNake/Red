using UnityEngine;
using UnityEngine.UI;

public class ExitCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public bool gameScene;
    
    private void Start()
    {
        if (yesButton != null) yesButton.onClick.AddListener(OnYesButtonClicked);
        
        if (noButton != null) noButton.onClick.AddListener(OnNoButtonClicked);
    }
    
    private void OnYesButtonClicked()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnNoButtonClicked()
    {
        if (gameScene)
            GameUIManager.Instance.ShowPause();
        else
            InitialMenuUIManager.Instance.ShowMenu();
    }
}
