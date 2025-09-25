using UnityEngine;

public class SocialLinks : MonoBehaviour
{
    public void OpenGitHub()
    {
        // Replace this with your actual GitHub profile URL
        string githubURL = "https://github.com/aadarshraj4321?tab=repositories";
        Debug.Log("Opening GitHub: " + githubURL);
        Application.OpenURL(githubURL);
    }

    // --- LINKEDIN ---
    public void OpenLinkedIn()
    {
        // Replace this with your actual LinkedIn profile URL
        string linkedinURL = "https://www.linkedin.com/in/aadarsh-raj-60676a191/";
        Debug.Log("Opening LinkedIn: " + linkedinURL);
        Application.OpenURL(linkedinURL);
    }

    // --- BLOGGING WEBSITE ---
    public void OpenBlog()
    {
        // Replace this with your actual Blog URL
        string blogURL = "https://www.google.com/";
        Debug.Log("Opening Blog: " + blogURL);
        Application.OpenURL(blogURL);
    }
}