using UnityEngine;

public class ShootableTarget : MonoBehaviour
{
    // Public fields that we can edit in the Unity Inspector for each target
    public string infoTitle;
    [TextArea(3, 10)] // This makes the text box in the Inspector bigger and easier to edit
    public string infoBody;

    // A private variable to hold a reference to our UI Manager
    private UIManager uiManager;

    void Start()
    {
        // At the start of the game, find the one and only UIManager script in the scene
        // so we can communicate with it.
        uiManager = FindObjectOfType<UIManager>();
    }

    public void OnHit()
    {
        // Instead of Debug.Log, we now call the UIManager's function,
        // passing in the specific title and body text for this target.
        uiManager.ShowInfoPanel(infoTitle, infoBody);
    }
}