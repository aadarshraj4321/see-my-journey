using UnityEngine;

// This line allows you to create these data assets from the "Create" menu
[CreateAssetMenu(fileName = "New Reward", menuName = "Portfolio/Reward Data")]
public class ProjectData : ScriptableObject
{
    public string title;
    [TextArea(5, 15)] // Makes the text box bigger in the Inspector
    public string description;
    // We'll keep these for projects, but can leave them blank for other rewards
    public string githubLink;
    public string liveDemoLink;
}