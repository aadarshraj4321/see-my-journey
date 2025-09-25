[System.Serializable]
public class Section
{
    public string sectionName; // "Projects", "Work Experience", etc.
    public Wave[] waves; // Each section has its own array of waves
    public string sectionCompleteMessage = "Section Complete!";
}