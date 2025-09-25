// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic; // We need this for using Lists
// using TMPro;
// using UnityEngine.UI;

// // This script manages the entire flow of the game, from section to section,
// // and wave to wave, including the player death and retry logic.
// public class EncounterManager : MonoBehaviour
// {
//     // --------------- PUBLIC VARIABLES (CONFIGURED IN INSPECTOR) ---------------

//     [Header("Full Game Flow")]
//     public Section[] sections;

//     [Header("Component References")]
//     public Transform[] spawnPoints;
    
//     [Header("Transition Effect")]
//     public GameObject sectionTransitionPrefab;
//     public float transitionDuration = 5.0f;

//     [Header("UI References")]
//     public GameObject rewardPanel;
//     public TextMeshProUGUI rewardTitleText;
//     public TextMeshProUGUI rewardDescriptionText;
//     public Button githubButton;
//     public Button liveDemoButton;
//     public Button continueButton;
    
//     [Header("Juice Effects")]
//     public float slowMoFactor = 0.1f;
//     public float slowMoDuration = 1.5f;
    
//     // --------------- PRIVATE VARIABLES (FOR INTERNAL LOGIC) ---------------
    
//     private int currentSectionIndex = 0;
//     private int currentWaveIndex = 0;
//     private int enemiesRemaining;
//     private bool waitingForContinue = false;
//     private AudioSource musicAudioSource;
    
//     // A list to keep track of all enemies currently alive in the scene
//     private List<GameObject> activeEnemies = new List<GameObject>();

//     // --------------- UNITY METHODS ---------------

//     void Start()
//     {
//         musicAudioSource = gameObject.AddComponent<AudioSource>();
//         musicAudioSource.loop = true;
//         musicAudioSource.volume = 0.4f;

//         rewardPanel.SetActive(false);
//         continueButton.onClick.AddListener(OnContinueButtonPressed);
        
//         StartCoroutine(RunGameFlow());
//     }

//     // --------------- CORE GAME FLOW LOGIC ---------------

//     private IEnumerator RunGameFlow()
//     {
//         // This outer loop runs once for each SECTION (Projects, Work Experience, etc.)
//         while (currentSectionIndex < sections.Length)
//         {
//             Section currentSection = sections[currentSectionIndex];
//             currentWaveIndex = 0;
//             activeEnemies.Clear(); // Clear the enemy list for the new section

//             // This inner loop runs once for each WAVE within the current section
//             while (currentWaveIndex < currentSection.waves.Length)
//             {
//                 Wave currentWave = currentSection.waves[currentWaveIndex];
//                 enemiesRemaining = currentWave.numberOfEnemies;
                
//                 if (currentWave.waveStartVO != null) AudioSource.PlayClipAtPoint(currentWave.waveStartVO, Camera.main.transform.position, 1.0f);
//                 if (currentWave.waveStartMusic != null) { musicAudioSource.clip = currentWave.waveStartMusic; musicAudioSource.Play(); }
                
//                 for (int i = 0; i < currentWave.numberOfEnemies; i++)
//                 {
//                     // Spawn the enemy and add the new instance to our list of active enemies
//                     GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
//                     activeEnemies.Add(newEnemy);
//                     yield return new WaitForSeconds(0.5f);
//                 }

//                 while (enemiesRemaining > 0) yield return null;
                
//                 if (currentWave.rewardToShow != null)
//                 {
//                     StartCoroutine(DoSlowMotion());
//                     ShowReward(currentWave.rewardToShow);
//                     waitingForContinue = true;
//                     while (waitingForContinue) yield return null;
//                 }
                
//                 currentWaveIndex++;
//             }

//             // --- SECTION COMPLETE ---
//             rewardTitleText.text = currentSection.sectionName + " Complete";
//             rewardDescriptionText.text = currentSection.sectionCompleteMessage;
//             githubButton.gameObject.SetActive(false);
//             liveDemoButton.gameObject.SetActive(false);
//             rewardPanel.SetActive(true);

//             waitingForContinue = true;
//             while (waitingForContinue) yield return null;

//             // --- PLAY THE TRANSITION EFFECT ---
//             if (sectionTransitionPrefab != null)
//             {
//                 Instantiate(sectionTransitionPrefab, Camera.main.transform.position, Quaternion.identity);
//             }
//             yield return new WaitForSeconds(transitionDuration);

//             currentSectionIndex++;
//         }

//         // --- ENTIRE GAME COMPLETE ---
//         rewardTitleText.text = "Portfolio Complete";
//         rewardDescriptionText.text = "Thank you for playing!";
//         continueButton.gameObject.SetActive(false);
//         rewardPanel.SetActive(true);
//     }
    
//     // --------------- HELPER & PUBLIC METHODS ---------------

//     // Called from EnemyHealth.cs every time an enemy is destroyed
//     public void OnEnemyDestroyed(GameObject destroyedEnemy) 
//     {
//         enemiesRemaining--; 
//         activeEnemies.Remove(destroyedEnemy); // Remove the dead enemy from our list
//     }

//     // Called from PlayerHealth.cs when player health reaches zero
//     public void OnPlayerDied()
//     {
//         // Stop the main game flow coroutine immediately
//         StopAllCoroutines();
//     }
    
//     // Called by the OnClick() event of the "Retry" button
//     public void OnRetryButtonPressed()
//     {
//         Debug.Log("Retrying section from wave " + currentWaveIndex);

//         // --- RESET THE GAME STATE ---
//         Time.timeScale = 1f; // Un-pause the game
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;

//         // Destroy all enemies that are still alive from the failed attempt
//         foreach (GameObject enemy in activeEnemies)
//         {
//             if (enemy != null)
//             {
//                 Destroy(enemy);
//             }
//         }
//         activeEnemies.Clear();

//         // Find and reset the player's health
//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null)
//         {
//             playerHealth.ResetHealth();
//         }

//         // --- RESTART THE CURRENT SECTION ---
//         // We restart the main coroutine. It will automatically start from the
//         // currentSectionIndex because we haven't changed it.
//         StartCoroutine(RunGameFlow());
//     }

//     // Called ONLY when the "Continue" button on the reward panel is clicked
//     public void OnContinueButtonPressed() 
//     {
//         rewardPanel.SetActive(false); 
//         waitingForContinue = false;
//     }
    
//     // Populates the UI panel with the correct information
//     void ShowReward(ProjectData rewardData)
//     {
//         rewardTitleText.text = rewardData.title;
//         rewardDescriptionText.text = rewardData.description;

//         bool hasGithub = !string.IsNullOrEmpty(rewardData.githubLink);
//         githubButton.gameObject.SetActive(hasGithub);
//         if(hasGithub) { githubButton.onClick.RemoveAllListeners(); githubButton.onClick.AddListener(() => Application.OpenURL(rewardData.githubLink)); }

//         bool hasLiveDemo = !string.IsNullOrEmpty(rewardData.liveDemoLink);
//         liveDemoButton.gameObject.SetActive(hasLiveDemo);
//         if(hasLiveDemo) { liveDemoButton.onClick.RemoveAllListeners(); liveDemoButton.onClick.AddListener(() => Application.OpenURL(rewardData.liveDemoLink)); }

//         rewardPanel.SetActive(true);
//     }
    
//     // Handles the slow-motion effect
//     private IEnumerator DoSlowMotion()
//     {
//         Time.timeScale = slowMoFactor;
//         Time.fixedDeltaTime = Time.timeScale * 0.02f;
//         yield return new WaitForSecondsRealtime(slowMoDuration);
//         Time.timeScale = 1f;
//         Time.fixedDeltaTime = 0.02f;
//     }
// }



















// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine.UI;

// public class EncounterManager : MonoBehaviour
// {
//     // --------------- PUBLIC VARIABLES (CONFIGURED IN INSPECTOR) ---------------

//     [Header("Full Game Flow")]
//     public Section[] sections;

//     [Header("Component References")]
//     public Transform[] spawnPoints;
    
//     [Header("Transition Effect")]
//     public GameObject sectionTransitionPrefab;
//     public float transitionDuration = 5.0f;

//     [Header("UI References")]
//     public GameObject rewardPanel;
//     public TextMeshProUGUI rewardTitleText;
//     public TextMeshProUGUI rewardDescriptionText;
//     public Button githubButton;
//     public Button liveDemoButton;
//     public Button continueButton;
    
//     [Header("Juice Effects")]
//     public float slowMoFactor = 0.1f;
//     public float slowMoDuration = 1.5f;

//     [Header("Targeting System")]
//     [Tooltip("A reference to the real player's Transform.")]
//     public Transform playerTarget;
//     [Tooltip("A reference to an empty GameObject placed far away.")]
//     public Transform safeZoneTarget;
//     // This static variable can be accessed by any enemy from anywhere in the code.
//     public static Transform currentTarget; 
    
//     // --------------- PRIVATE VARIABLES (FOR INTERNAL LOGIC) ---------------
    
//     private int currentSectionIndex = 0;
//     private int currentWaveIndex = 0;
//     private int enemiesRemaining;
//     private bool waitingForContinue = false;
//     private AudioSource musicAudioSource;
//     private List<GameObject> activeEnemies = new List<GameObject>();

//     // --------------- UNITY METHODS ---------------

//     void Start()
//     {
//         musicAudioSource = gameObject.AddComponent<AudioSource>();
//         musicAudioSource.loop = true;
//         musicAudioSource.volume = 0.4f;

//         rewardPanel.SetActive(false);
//         continueButton.onClick.AddListener(OnContinueButtonPressed);
        
//         // At the very start of the game, the enemies' target is the real player.
//         currentTarget = playerTarget;
        
//         StartCoroutine(RunGameFlow());
//     }

//     // --------------- CORE GAME FLOW & HELPER METHODS ---------------

//     private IEnumerator RunGameFlow()
//     {
//         while (currentSectionIndex < sections.Length)
//         {
//             Section currentSection = sections[currentSectionIndex];
//             currentWaveIndex = 0;
//             activeEnemies.Clear();

//             while (currentWaveIndex < currentSection.waves.Length)
//             {
//                 Wave currentWave = currentSection.waves[currentWaveIndex];
//                 enemiesRemaining = currentWave.numberOfEnemies;
                
//                 if (currentWave.waveStartVO != null) AudioSource.PlayClipAtPoint(currentWave.waveStartVO, Camera.main.transform.position, 1.0f);
//                 if (currentWave.waveStartMusic != null) { musicAudioSource.clip = currentWave.waveStartMusic; musicAudioSource.Play(); }
                
//                 for (int i = 0; i < currentWave.numberOfEnemies; i++)
//                 {
//                     GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
//                     activeEnemies.Add(newEnemy);
//                     yield return new WaitForSeconds(0.5f);
//                 }

//                 while (enemiesRemaining > 0) yield return null;
                
//                 if (currentWave.rewardToShow != null)
//                 {
//                     StartCoroutine(DoSlowMotion());
//                     ShowReward(currentWave.rewardToShow);
//                     waitingForContinue = true;
//                     while (waitingForContinue) yield return null;
//                 }
                
//                 currentWaveIndex++;
//             }

//             rewardTitleText.text = currentSection.sectionName + " Complete";
//             rewardDescriptionText.text = currentSection.sectionCompleteMessage;
//             githubButton.gameObject.SetActive(false);
//             liveDemoButton.gameObject.SetActive(false);
//             rewardPanel.SetActive(true);

//             waitingForContinue = true;
//             while (waitingForContinue) yield return null;

//             if (sectionTransitionPrefab != null)
//             {
//                 Instantiate(sectionTransitionPrefab, Camera.main.transform.position, Quaternion.identity);
//             }
//             yield return new WaitForSeconds(transitionDuration);

//             currentSectionIndex++;
//         }

//         rewardTitleText.text = "Portfolio Complete";
//         rewardDescriptionText.text = "Thank you for playing!";
//         continueButton.gameObject.SetActive(false);
//         rewardPanel.SetActive(true);
//     }

//     // THIS IS THE NEW PUBLIC FUNCTION FOR THE SAFE ZONE
//     public void SetPlayerInSafeZone(bool isInSafeZone)
//     {
//         if (isInSafeZone)
//         {
//             Debug.Log("Player entered safe zone. Enemies will now ignore the player.");
//             currentTarget = safeZoneTarget; // Enemies will chase the fake target
//         }
//         else
//         {
//             Debug.Log("Player left safe zone. Enemies will now chase the player.");
//             currentTarget = playerTarget; // Enemies will chase the real player again
//         }
//     }
    
//     public void OnEnemyDestroyed(GameObject destroyedEnemy) 
//     {
//         enemiesRemaining--; 
//         activeEnemies.Remove(destroyedEnemy);
//     }

//     public void OnPlayerDied()
//     {
//         StopAllCoroutines();
//     }
    
//     public void OnRetryButtonPressed()
//     {
//         Time.timeScale = 1f;
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;

//         foreach (GameObject enemy in activeEnemies)
//         {
//             if (enemy != null) Destroy(enemy);
//         }
//         activeEnemies.Clear();

//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null) playerHealth.ResetHealth();

//         StartCoroutine(RunGameFlow());
//     }

//     public void OnContinueButtonPressed() 
//     {
//         rewardPanel.SetActive(false); 
//         waitingForContinue = false;
//     }
    
//     void ShowReward(ProjectData rewardData)
//     {
//         rewardTitleText.text = rewardData.title;
//         rewardDescriptionText.text = rewardData.description;
//         bool hasGithub = !string.IsNullOrEmpty(rewardData.githubLink);
//         githubButton.gameObject.SetActive(hasGithub);
//         if(hasGithub) { githubButton.onClick.RemoveAllListeners(); githubButton.onClick.AddListener(() => Application.OpenURL(rewardData.githubLink)); }
//         bool hasLiveDemo = !string.IsNullOrEmpty(rewardData.liveDemoLink);
//         liveDemoButton.gameObject.SetActive(hasLiveDemo);
//         if(hasLiveDemo) { liveDemoButton.onClick.RemoveAllListeners(); liveDemoButton.onClick.AddListener(() => Application.OpenURL(rewardData.liveDemoLink)); }
//         rewardPanel.SetActive(true);
//     }
    
//     private IEnumerator DoSlowMotion()
//     {
//         Time.timeScale = slowMoFactor;
//         Time.fixedDeltaTime = Time.timeScale * 0.02f;
//         yield return new WaitForSecondsRealtime(slowMoDuration);
//         Time.timeScale = 1f;
//         Time.fixedDeltaTime = 0.02f;
//     }
// }













// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine.UI;

// public class EncounterManager : MonoBehaviour
// {
//     // --------------- PUBLIC VARIABLES (CONFIGURED IN INSPECTOR) ---------------
//     [Header("Full Game Flow")]
//     public Section[] sections;
//     [Header("Component References")]
//     public Transform[] spawnPoints;
//     [Header("Transition Effect")]
//     public GameObject sectionTransitionPrefab;
//     public float transitionDuration = 5.0f;
//     [Header("UI References")]
//     public GameObject rewardPanel;
//     public TextMeshProUGUI rewardTitleText;
//     public TextMeshProUGUI rewardDescriptionText;
//     public Button githubButton;
//     public Button liveDemoButton;
//     public Button continueButton;
//     [Header("Juice Effects")]
//     public float slowMoFactor = 0.1f;
//     public float slowMoDuration = 1.5f;
//     [Header("Targeting System")]
//     public Transform playerTarget;
//     public Transform safeZoneTarget;
//     public static Transform currentTarget; 
    
//     // --------------- PRIVATE VARIABLES (FOR INTERNAL LOGIC) ---------------
//     private int currentSectionIndex = 0;
//     private int currentWaveIndex = 0;
//     private int enemiesRemaining;
//     private bool waitingForContinue = false;
//     private AudioSource musicAudioSource;
//     private List<GameObject> activeEnemies = new List<GameObject>();

//     // --------------- UNITY METHODS ---------------
//     void Start()
//     {
//         musicAudioSource = gameObject.AddComponent<AudioSource>();
//         musicAudioSource.loop = true;
//         musicAudioSource.volume = 0.4f;

//         continueButton.onClick.AddListener(OnContinueButtonPressed);
        
//         // At the very start, we make sure the game is in "gameplay" mode
//         HideUIPanel(); 
        
//         currentTarget = playerTarget;
//         StartCoroutine(RunGameFlow());
//     }

//     // --------------- CORE GAME FLOW & HELPER METHODS ---------------
//     private IEnumerator RunGameFlow()
//     {
//         while (currentSectionIndex < sections.Length)
//         {
//             Section currentSection = sections[currentSectionIndex];
//             currentWaveIndex = 0;
//             activeEnemies.Clear();

//             while (currentWaveIndex < currentSection.waves.Length)
//             {
//                 Wave currentWave = currentSection.waves[currentWaveIndex];
//                 enemiesRemaining = currentWave.numberOfEnemies;
                
//                 if (currentWave.waveStartVO != null) AudioSource.PlayClipAtPoint(currentWave.waveStartVO, Camera.main.transform.position, 1.0f);
//                 if (currentWave.waveStartMusic != null) { musicAudioSource.clip = currentWave.waveStartMusic; musicAudioSource.Play(); }
                
//                 for (int i = 0; i < currentWave.numberOfEnemies; i++)
//                 {
//                     GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
//                     activeEnemies.Add(newEnemy);
//                     yield return new WaitForSeconds(0.5f);
//                 }

//                 while (enemiesRemaining > 0) yield return null;
                
//                 if (currentWave.rewardToShow != null)
//                 {
//                     StartCoroutine(DoSlowMotion());
//                     ShowReward(currentWave.rewardToShow);
//                     waitingForContinue = true;
//                     while (waitingForContinue) yield return null;
//                 }
                
//                 currentWaveIndex++;
//             }

//             ShowReward(currentSection.sectionName + " Complete", currentSection.sectionCompleteMessage);
//             waitingForContinue = true;
//             while (waitingForContinue) yield return null;

//             if (sectionTransitionPrefab != null)
//             {
//                 Instantiate(sectionTransitionPrefab, Camera.main.transform.position, Quaternion.identity);
//             }
//             yield return new WaitForSeconds(transitionDuration);

//             currentSectionIndex++;
//         }

//         ShowReward("Portfolio Complete", "Thank you for playing!", false);
//     }

//     public void SetPlayerInSafeZone(bool isInSafeZone)
//     {
//         currentTarget = isInSafeZone ? safeZoneTarget : playerTarget;
//     }
    
//     public void OnEnemyDestroyed(GameObject destroyedEnemy) 
//     {
//         enemiesRemaining--; 
//         activeEnemies.Remove(destroyedEnemy);
//     }

//     public void OnPlayerDied()
//     {
//         StopAllCoroutines();
//     }
    
//     public void OnRetryButtonPressed()
//     {
//         HideUIPanel(); // This now handles un-pausing and re-locking the cursor
        
//         foreach (GameObject enemy in activeEnemies)
//         {
//             if (enemy != null) Destroy(enemy);
//         }
//         activeEnemies.Clear();

//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null) playerHealth.ResetHealth();

//         StartCoroutine(RunGameFlow());
//     }

//     public void OnContinueButtonPressed() 
//     {
//         HideUIPanel(); // This now handles re-locking the cursor
//         waitingForContinue = false;
//     }
    
//     // This is the old ShowReward, for ProjectData
//     void ShowReward(ProjectData rewardData)
//     {
//         rewardTitleText.text = rewardData.title;
//         rewardDescriptionText.text = rewardData.description;

//         bool hasGithub = !string.IsNullOrEmpty(rewardData.githubLink);
//         githubButton.gameObject.SetActive(hasGithub);
//         if(hasGithub) { githubButton.onClick.RemoveAllListeners(); githubButton.onClick.AddListener(() => Application.OpenURL(rewardData.githubLink)); }

//         bool hasLiveDemo = !string.IsNullOrEmpty(rewardData.liveDemoLink);
//         liveDemoButton.gameObject.SetActive(hasLiveDemo);
//         if(hasLiveDemo) { liveDemoButton.onClick.RemoveAllListeners(); liveDemoButton.onClick.AddListener(() => Application.OpenURL(rewardData.liveDemoLink)); }

//         ShowUIPanel(); // Call our new centralized function
//     }

//     // This is a new, simpler version for messages
//     void ShowReward(string title, string description, bool showContinue = true)
//     {
//         rewardTitleText.text = title;
//         rewardDescriptionText.text = description;
//         githubButton.gameObject.SetActive(false);
//         liveDemoButton.gameObject.SetActive(false);
//         continueButton.gameObject.SetActive(showContinue);
        
//         ShowUIPanel(); // Call our new centralized function
//     }
    
//     private IEnumerator DoSlowMotion()
//     {
//         Time.timeScale = slowMoFactor;
//         Time.fixedDeltaTime = Time.timeScale * 0.02f;
//         yield return new WaitForSecondsRealtime(slowMoDuration);
//         Time.timeScale = 1f;
//         Time.fixedDeltaTime = 0.02f;
//     }

//     // --- NEW CENTRALIZED UI FUNCTIONS ---

//     private void ShowUIPanel()
//     {
//         rewardPanel.SetActive(true);
//         // This is the key part: we now unlock and show the mouse automatically.
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }

//     private void HideUIPanel()
//     {
//         rewardPanel.SetActive(false);
//         // We re-lock and hide the mouse.
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         // Also make sure game is un-paused
//         Time.timeScale = 1f;
//     }
// }
























// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine.UI;

// public class EncounterManager : MonoBehaviour
// {
//     // --------------- PUBLIC VARIABLES (CONFIGURED IN INSPECTOR) ---------------

//     [Header("Full Game Flow")]
//     public Section[] sections;
    
//     [Header("Endless Mode")]
//     [Tooltip("The enemy that will spawn in endless mode.")]
//     public GameObject endlessEnemyPrefab;
//     [Tooltip("How many seconds between each enemy spawn in endless mode.")]
//     public float endlessSpawnInterval = 3.0f;

//     [Header("Component References")]
//     public Transform[] spawnPoints;
    
//     [Header("Transition Effect")]
//     public GameObject sectionTransitionPrefab;
//     public float transitionDuration = 5.0f;

//     [Header("UI References")]
//     public GameObject rewardPanel;
//     public TextMeshProUGUI rewardTitleText;
//     public TextMeshProUGUI rewardDescriptionText;
//     public Button githubButton;
//     public Button liveDemoButton;
//     public Button continueButton;
    
//     [Header("Juice Effects")]
//     public float slowMoFactor = 0.1f;
//     public float slowMoDuration = 1.5f;

//     [Header("Targeting System")]
//     public Transform playerTarget;
//     public Transform safeZoneTarget;
//     public static Transform currentTarget; 
    
//     // --------------- PRIVATE VARIABLES (FOR INTERNAL LOGIC) ---------------
    
//     private int currentSectionIndex = 0;
//     private int currentWaveIndex = 0;
//     private int enemiesRemaining;
//     private bool waitingForContinue = false;
//     private AudioSource musicAudioSource;
//     private List<GameObject> activeEnemies = new List<GameObject>();

//     // --------------- UNITY METHODS ---------------

//     void Start()
//     {
//         musicAudioSource = gameObject.AddComponent<AudioSource>();
//         musicAudioSource.loop = true;
//         musicAudioSource.volume = 0.4f;

//         continueButton.onClick.AddListener(OnContinueButtonPressed);
        
//         HideUIPanel(); 
        
//         currentTarget = playerTarget;
        
//         StartCoroutine(RunGameFlow());
//     }

//     // --------------- CORE GAME FLOW & HELPER METHODS ---------------

//     private IEnumerator RunGameFlow()
//     {
//         // This is the main "story" part of your portfolio
//         while (currentSectionIndex < sections.Length)
//         {
//             Section currentSection = sections[currentSectionIndex];
//             currentWaveIndex = 0;
//             activeEnemies.Clear();

//             while (currentWaveIndex < currentSection.waves.Length)
//             {
//                 Wave currentWave = currentSection.waves[currentWaveIndex];
//                 enemiesRemaining = currentWave.numberOfEnemies;
                
//                 if (currentWave.waveStartVO != null) AudioSource.PlayClipAtPoint(currentWave.waveStartVO, Camera.main.transform.position, 1.0f);
//                 if (currentWave.waveStartMusic != null) { musicAudioSource.clip = currentWave.waveStartMusic; musicAudioSource.Play(); }
                
//                 for (int i = 0; i < currentWave.numberOfEnemies; i++)
//                 {
//                     GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
//                     activeEnemies.Add(newEnemy);
//                     yield return new WaitForSeconds(0.5f);
//                 }

//                 while (enemiesRemaining > 0) yield return null;
                
//                 if (currentWave.rewardToShow != null)
//                 {
//                     StartCoroutine(DoSlowMotion());
//                     ShowReward(currentWave.rewardToShow);
//                     waitingForContinue = true;
//                     while (waitingForContinue) yield return null;
//                 }
//                 currentWaveIndex++;
//             }

//             ShowReward(currentSection.sectionName + " Complete", currentSection.sectionCompleteMessage);
//             waitingForContinue = true;
//             while (waitingForContinue) yield return null;

//             if (sectionTransitionPrefab != null)
//             {
//                 Instantiate(sectionTransitionPrefab, Camera.main.transform.position, Quaternion.identity);
//             }
//             yield return new WaitForSeconds(transitionDuration);

//             currentSectionIndex++;
//         }

//         // --- ENTIRE PORTFOLIO COMPLETE ---
//         ShowReward("Portfolio Complete", "Thank you for reviewing my work!");
//         waitingForContinue = true;
//         while(waitingForContinue) yield return null;

//         // --- SHOW THE EXPLORATION MESSAGE ---
//         ShowReward("The World is Yours", "There are many hidden things you have to know about me. To find them, explore everything.");
//         waitingForContinue = true;
//         while(waitingForContinue) yield return null;

//         // --- START ENDLESS MODE ---
//         Debug.Log("Endless mode has started!");
//         StartCoroutine(RunEndlessMode());
//     }
    
//     private IEnumerator RunEndlessMode()
//     {
//         // This loop will run forever after the main story is complete
//         while (true)
//         {
//             // Wait for the specified interval
//             yield return new WaitForSeconds(endlessSpawnInterval);

//             // Spawn one enemy at a random point
//             if (endlessEnemyPrefab != null)
//             {
//                 Instantiate(endlessEnemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
//             }
//         }
//     }
    
//     public void SetPlayerInSafeZone(bool isInSafeZone)
//     {
//         currentTarget = isInSafeZone ? safeZoneTarget : playerTarget;
//     }
    
//     public void OnEnemyDestroyed(GameObject destroyedEnemy) 
//     {
//         // Only decrement the counter if we are still in the "story" part of the game
//         if(currentSectionIndex < sections.Length)
//         {
//             enemiesRemaining--; 
//         }
//         activeEnemies.Remove(destroyedEnemy);
//     }

//     public void OnPlayerDied()
//     {
//         StopAllCoroutines();
//     }
    
//     public void OnRetryButtonPressed()
//     {
//         HideUIPanel();
        
//         foreach (GameObject enemy in activeEnemies)
//         {
//             if (enemy != null) Destroy(enemy);
//         }
//         activeEnemies.Clear();

//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null) playerHealth.ResetHealth();

//         StartCoroutine(RunGameFlow());
//     }

//     public void OnContinueButtonPressed() 
//     {
//         HideUIPanel();
//         waitingForContinue = false;
//     }
    
//     void ShowReward(ProjectData rewardData)
//     {
//         rewardTitleText.text = rewardData.title;
//         rewardDescriptionText.text = rewardData.description;

//         bool hasGithub = !string.IsNullOrEmpty(rewardData.githubLink);
//         githubButton.gameObject.SetActive(hasGithub);
//         if(hasGithub) { githubButton.onClick.RemoveAllListeners(); githubButton.onClick.AddListener(() => Application.OpenURL(rewardData.githubLink)); }

//         bool hasLiveDemo = !string.IsNullOrEmpty(rewardData.liveDemoLink);
//         liveDemoButton.gameObject.SetActive(hasLiveDemo);
//         if(hasLiveDemo) { liveDemoButton.onClick.RemoveAllListeners(); liveDemoButton.onClick.AddListener(() => Application.OpenURL(rewardData.liveDemoLink)); }

//         ShowUIPanel();
//     }

//     void ShowReward(string title, string description, bool showContinue = true)
//     {
//         rewardTitleText.text = title;
//         rewardDescriptionText.text = description;
//         githubButton.gameObject.SetActive(false);
//         liveDemoButton.gameObject.SetActive(false);
//         continueButton.gameObject.SetActive(showContinue);
        
//         ShowUIPanel();
//     }
    
//     private IEnumerator DoSlowMotion()
//     {
//         Time.timeScale = slowMoFactor;
//         Time.fixedDeltaTime = Time.timeScale * 0.02f;
//         yield return new WaitForSecondsRealtime(slowMoDuration);
//         Time.timeScale = 1f;
//         Time.fixedDeltaTime = 0.02f;
//     }
    
//     private void ShowUIPanel()
//     {
//         rewardPanel.SetActive(true);
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }

//     private void HideUIPanel()
//     {
//         rewardPanel.SetActive(false);
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         Time.timeScale = 1f;
//     }
// }

















using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    // --------------- PUBLIC VARIABLES (CONFIGURED IN INSPECTOR) ---------------
    [Header("Full Game Flow")]
    public Section[] sections;
    
    [Header("Endless Mode")]
    public GameObject endlessEnemyPrefab;
    public float endlessSpawnInterval = 3.0f;

    [Header("Component References")]
    public Transform[] spawnPoints;
    
    [Header("Transition Effect")]
    public GameObject sectionTransitionPrefab;
    public float transitionDuration = 5.0f;

    [Header("UI References")]
    public GameObject rewardPanel;
    public TextMeshProUGUI rewardTitleText;
    public TextMeshProUGUI rewardDescriptionText;
    public Button githubButton;
    public Button liveDemoButton;
    public Button continueButton;
    
    [Header("Juice Effects")]
    public float slowMoFactor = 0.1f;
    public float slowMoDuration = 1.5f;

    [Header("Targeting System")]
    public Transform playerTarget;
    public Transform safeZoneTarget;
    public static Transform currentTarget; 
    
    // --------------- PRIVATE VARIABLES (FOR INTERNAL LOGIC) ---------------
    private int currentSectionIndex = 0;
    private int currentWaveIndex = 0;
    private int enemiesRemaining;
    private bool waitingForContinue = false;
    private AudioSource musicAudioSource;
    private List<GameObject> activeEnemies = new List<GameObject>();

    // --------------- UNITY METHODS ---------------

    void Start()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0.4f;
        continueButton.onClick.AddListener(OnContinueButtonPressed);
        
        // At the very start, the default target is the player.
        currentTarget = playerTarget;
        
        // --- NEW: START THE INTRO INSTEAD OF THE MAIN GAME FLOW ---
        StartCoroutine(IntroSequence());
    }

    // --------------- CORE GAME FLOW & HELPER METHODS ---------------

    // --- NEW: INTRO SEQUENCE COROUTINE ---
    private IEnumerator IntroSequence()
    {
        // Show the initial welcome message. The game is effectively paused.
        ShowReward("Welcome This is Interactive Portfolio", "You are starting a never-seen-before adventure. Click Continue to begin.");

        // Pause the coroutine here until the player clicks "Continue"
        waitingForContinue = true;
        while (waitingForContinue)
        {
            yield return null; // Wait for the next frame
        }

        // After the player clicks continue, start the main game flow.
        // StartCoroutine(RunGameFlow());
        StartCoroutine(HelpSequence());
    }

    private IEnumerator HelpSequence()
    {
        ShowReward("On right side, you can see the controls", "You have to kill every moving object in this game — either it is drone tech, or ghost tech. You must destroy them to know about my portfolio. If your health is low, there are three types of food: banana, burger, and egg. You can eat them to increase your health. You have to find them in this map.");

        // Pause the coroutine here until the player clicks "Continue"
        waitingForContinue = true;
        while (waitingForContinue)
        {
            yield return null; // Wait for the next frame
        }

        StartCoroutine(RunGameFlow());
    }

    private IEnumerator RunGameFlow()
    {
        // This is the main "story" part of your portfolio
        while (currentSectionIndex < sections.Length)
        {
            Section currentSection = sections[currentSectionIndex];
            currentWaveIndex = 0;
            activeEnemies.Clear();

            while (currentWaveIndex < currentSection.waves.Length)
            {
                Wave currentWave = currentSection.waves[currentWaveIndex];
                enemiesRemaining = currentWave.numberOfEnemies;

                if (currentWave.waveStartVO != null) AudioSource.PlayClipAtPoint(currentWave.waveStartVO, Camera.main.transform.position, 1.0f);
                if (currentWave.waveStartMusic != null) { musicAudioSource.clip = currentWave.waveStartMusic; musicAudioSource.Play(); }

                for (int i = 0; i < currentWave.numberOfEnemies; i++)
                {
                    GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                    activeEnemies.Add(newEnemy);
                    yield return new WaitForSeconds(0.5f);
                }

                while (enemiesRemaining > 0) yield return null;

                if (currentWave.rewardToShow != null)
                {
                    StartCoroutine(DoSlowMotion());
                    ShowReward(currentWave.rewardToShow);
                    waitingForContinue = true;
                    while (waitingForContinue) yield return null;
                }
                currentWaveIndex++;
            }

            ShowReward(currentSection.sectionName + " Complete", currentSection.sectionCompleteMessage);
            waitingForContinue = true;
            while (waitingForContinue) yield return null;

            if (sectionTransitionPrefab != null)
            {
                Instantiate(sectionTransitionPrefab, Camera.main.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(transitionDuration);

            currentSectionIndex++;
        }

        // --- ENTIRE PORTFOLIO COMPLETE ---
        ShowReward("Portfolio Complete", "Thank you for reviewing my work!");
        waitingForContinue = true;
        while (waitingForContinue) yield return null;

        // --- SHOW THE EXPLORATION MESSAGE ---
        ShowReward("The World is Yours", "There are many hidden things you have to know about me. To find them, explore everything.");
        waitingForContinue = true;
        while (waitingForContinue) yield return null;

        // --- START ENDLESS MODE ---
        Debug.Log("Endless mode has started!");
        StartCoroutine(RunEndlessMode());
    }
    
    private IEnumerator RunEndlessMode()
    {
        while (true)
        {
            yield return new WaitForSeconds(endlessSpawnInterval);
            if (endlessEnemyPrefab != null)
            {
                Instantiate(endlessEnemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            }
        }
    }
    
    // All other helper functions remain the same
    #region Full Helper Functions (No Changes)
    public void SetPlayerInSafeZone(bool isInSafeZone)
    {
        currentTarget = isInSafeZone ? safeZoneTarget : playerTarget;
    }
    public void OnEnemyDestroyed(GameObject destroyedEnemy) 
    {
        if(currentSectionIndex < sections.Length)
        {
            enemiesRemaining--; 
        }
        activeEnemies.Remove(destroyedEnemy);
    }
    public void OnPlayerDied()
    {
        StopAllCoroutines();
    }
    public void OnRetryButtonPressed()
    {
        HideUIPanel();
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        activeEnemies.Clear();
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null) playerHealth.ResetHealth();
        StartCoroutine(RunGameFlow());
    }
    public void OnContinueButtonPressed() 
    {
        HideUIPanel();
        waitingForContinue = false;
    }
    void ShowReward(ProjectData rewardData)
    {
        rewardTitleText.text = rewardData.title;
        rewardDescriptionText.text = rewardData.description;
        bool hasGithub = !string.IsNullOrEmpty(rewardData.githubLink);
        githubButton.gameObject.SetActive(hasGithub);
        if(hasGithub) { githubButton.onClick.RemoveAllListeners(); githubButton.onClick.AddListener(() => Application.OpenURL(rewardData.githubLink)); }
        bool hasLiveDemo = !string.IsNullOrEmpty(rewardData.liveDemoLink);
        liveDemoButton.gameObject.SetActive(hasLiveDemo);
        if(hasLiveDemo) { liveDemoButton.onClick.RemoveAllListeners(); liveDemoButton.onClick.AddListener(() => Application.OpenURL(rewardData.liveDemoLink)); }
        ShowUIPanel();
    }
    void ShowReward(string title, string description, bool showContinue = true)
    {
        rewardTitleText.text = title;
        rewardDescriptionText.text = description;
        githubButton.gameObject.SetActive(false);
        liveDemoButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(showContinue);
        ShowUIPanel();
    }
    private IEnumerator DoSlowMotion()
    {
        Time.timeScale = slowMoFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSecondsRealtime(slowMoDuration);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    private void ShowUIPanel()
    {
        rewardPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void HideUIPanel()
    {
        rewardPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
    #endregion
}