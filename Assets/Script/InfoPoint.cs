using UnityEngine;
using TMPro;

public class InfoPoint : MonoBehaviour
{
    [Header("Data to Display")]
    [Tooltip("The Reward Data asset that holds the info for this point.")]
    public ProjectData infoToShow;

    [Header("UI References")]

    [Tooltip("The parent GameObject of the entire reward panel.")]
    public GameObject rewardPanel;
    [Tooltip("The TextMeshPro text for the reward title.")]
    public TextMeshProUGUI rewardTitleText;
    [Tooltip("The TextMeshPro text for the reward description.")]
    public TextMeshProUGUI rewardDescriptionText;
    
    // A private variable to hold a reference to our EncounterManager
    private EncounterManager encounterManager;
    // A flag to ensure the UI starts hidden
    private bool isPanelActive = false;

    void Start()
    {
        // Find the EncounterManager when the game starts
        encounterManager = FindObjectOfType<EncounterManager>();
        
        // It's good practice to make sure the panel is hidden at the start
        if (rewardPanel != null && !isPanelActive)
        {
            rewardPanel.SetActive(false);
        }
    }

    // This built-in Unity function runs when a Rigidbody ENTERS a trigger collider
    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        encounterManager.SetPlayerInSafeZone(true);
        rewardTitleText.text = infoToShow.title;
        rewardDescriptionText.text = infoToShow.description;
        rewardPanel.SetActive(true); 
        // No cursor logic needed here! It stays locked.
    }
}

// And the new, clean OnTriggerExit
private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        encounterManager.SetPlayerInSafeZone(false);
        rewardPanel.SetActive(false);
    }
}
}