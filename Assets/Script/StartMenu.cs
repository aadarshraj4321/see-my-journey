using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // We need this to access the Button component

public class StartMenu : MonoBehaviour
{
    // The name of your main game scene file
    public string mainGameSceneName = "SampleScene";

    [Header("UI References")]
    // A reference to the button that will have the animation
    public Button startButton;

    // A private variable to hold the Animator component of the button
    private Animator buttonAnimator;

    void Start()
    {
        // Get the Animator component from the startButton
        if (startButton != null)
        {
            buttonAnimator = startButton.GetComponent<Animator>();
        }

        // --- NEW ---
        // Immediately trigger the animation to start playing its loop
        if (buttonAnimator != null)
        {
            // We use "StartPulse" as the name of the trigger we will create
            buttonAnimator.SetTrigger("StartPulse");
        }
    }

    // This function is called by the "Start" button's OnClick() event
    public void StartGame()
    {
        Debug.Log("Start button clicked. Requesting fullscreen and loading main scene...");

        // Stop the pulsing animation when the button is clicked
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("StopPulse");
        }
        
        // --- THIS IS THE KEY PART ---
        // This function tells the browser to go fullscreen.
        Screen.fullScreen = true;

        // Load your main game scene
        SceneManager.LoadScene(mainGameSceneName);
    }
}