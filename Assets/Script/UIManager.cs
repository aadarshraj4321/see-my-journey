using UnityEngine;
using TMPro; // We need this line to work with TextMeshPro elements

public class UIManager : MonoBehaviour
{
    // A reference to the entire panel, so we can show/hide it
    public GameObject infoPanel;
    // A reference to the title text component
    public TextMeshProUGUI titleText;
    // A reference to the body text component
    public TextMeshProUGUI bodyText;

    void Start()
    {
        // Start with the panel hidden so it's not visible at the beginning
        infoPanel.SetActive(false);
    }

    // A public function that other scripts can call to show the panel
    public void ShowInfoPanel(string newTitle, string newBody)
    {
        // Set the text to the values passed in from the target
        titleText.text = newTitle;
        bodyText.text = newBody;
        // Make the panel visible
        infoPanel.SetActive(true);
    }

    // A public function that our "Close" button will call
    public void HideInfoPanel()
    {
        // Hide the panel
        infoPanel.SetActive(false);
    }
}