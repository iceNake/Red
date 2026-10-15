using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button audioButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        if (audioButton != null) audioButton.onClick.AddListener(OnAudioButtonClicked);
        if (backButton != null) backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnAudioButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowAudio();
    }

    private void OnBackButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowMenu();
    }
}
