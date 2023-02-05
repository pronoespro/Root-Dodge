using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager singleton;
    public Animator animator;

    private void Awake()
    {
        if (singleton != null) Destroy(this);
        singleton = this;

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void NavigateToScene(string sceneName)
    {
        animator.SetTrigger("FadeIn");
        Debug.Log("Cargando siguiente escena");
        SceneManager.LoadScene(sceneName);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (singleton != null && singleton != this) Destroy(this.gameObject);
        Debug.Log("ESCENA CARGADA");
        animator.SetTrigger("FadeOut");
    }
}
