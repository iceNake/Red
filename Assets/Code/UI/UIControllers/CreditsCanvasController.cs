using UnityEngine;
using UnityEngine.UI;

public class CreditsCanvasController : MonoBehaviour
{
    [Header("Buttons References")]
    [SerializeField] private Button backButton;

    private void Start()
    {
        if (backButton != null) backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        InitialMenuUIManager.Instance.ShowMenu();
    }
}
