using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Animator animator;
    private string sceneToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel(string sceneName)
    {
        sceneToLoad = sceneName;
        animator.SetTrigger("StartTransition");
    }

    // This function is called by the Animation Event
    public void OnTransitionComplete()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        SceneManager.LoadScene(sceneToLoad);
        yield return null;
    }
}