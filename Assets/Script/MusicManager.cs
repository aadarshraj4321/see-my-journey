using UnityEngine;
using System.Collections.Generic; // We need this to use a List

public class MusicManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The list of background music tracks to play in order.")]
    public List<AudioClip> musicPlaylist; // We now use a List to hold multiple tracks
    
    [Tooltip("The volume of the music. 0.0 is silent, 1.0 is full volume.")]
    [Range(0.0f, 1.0f)]
    public float volume = 0.2f;

    public static MusicManager Instance;
    
    private AudioSource audioSource;
    private int currentTrackIndex = 0; // A variable to remember which track we are on

    void Awake()
    {
        // Singleton Pattern: Ensures only one MusicManager exists
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Add and configure the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        // IMPORTANT: We set loop to FALSE now, because we want to know when a track finishes.
        audioSource.loop = false; 
    }

    void Start()
    {
        // Start playing the first track in the playlist.
        if (musicPlaylist != null && musicPlaylist.Count > 0)
        {
            PlayTrack(currentTrackIndex);
        }
    }

    // This is the main logic for the playlist.
    void Update()
    {
        // Check if the AudioSource is NOT currently playing a song.
        if (!audioSource.isPlaying)
        {
            // If the playlist is not empty...
            if (musicPlaylist != null && musicPlaylist.Count > 0)
            {
                // ...move to the next track.
                currentTrackIndex++;
                
                // If we've gone past the end of the playlist...
                if (currentTrackIndex >= musicPlaylist.Count)
                {
                    // ...loop back to the first track.
                    currentTrackIndex = 0;
                }
                
                // Play the new track.
                PlayTrack(currentTrackIndex);
            }
        }
    }

    // A simple helper function to play a track from the list.
    void PlayTrack(int trackIndex)
    {
        // Make sure the index is valid
        if (trackIndex < musicPlaylist.Count)
        {
            audioSource.clip = musicPlaylist[trackIndex];
            audioSource.Play();
            Debug.Log("Music Manager: Now playing track " + (trackIndex + 1) + ": " + audioSource.clip.name);
        }
    }
}