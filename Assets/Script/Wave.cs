using UnityEngine;

// This line is important. It lets us edit this class in the Unity Inspector.
[System.Serializable]
public class Wave
{
    // --- For Organization ---
    public string waveName; // Example: "Project 1: AI Operating System"

    // --- Wave Logic ---
    public int numberOfEnemies;
    public GameObject enemyPrefab; // You can use different enemies for different waves

    // --- Reward System ---
    public ProjectData rewardToShow; // The data asset to show after the wave is cleared

    [Header("Audio (Optional)")]
    public AudioClip waveStartMusic;  // The background music to play during this wave
    public AudioClip waveStartVO;     // A voiceover line to play when the wave starts
}