using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PronoesPro.UI
{
    public class FadeManager : MonoBehaviour
    {

        #region instancing
        public static FadeManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        public float fadeInTime, fadeOutTime;

        public Animator anim;

        public Transform fadeObj, blackBGObj;

        private bool midTransition;
        private string levelToLoad;

        public void LoadLevel(string levelName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (!midTransition)
            {
                StartCoroutine(FadeToLevel(levelName, mode));
            }
            else if (levelName != levelToLoad)
            {
                StartCoroutine(WaitToFade(levelName, mode));
            }
        }

        public IEnumerator WaitToFade(string scene, LoadSceneMode mode)
        {
            while (midTransition)
            {
                yield return null;
            }
            StartCoroutine(FadeToLevel(scene, mode));
        }

        public IEnumerator FadeToLevel(string levelName, LoadSceneMode mode)
        {
            levelToLoad = levelName;
            anim.SetBool("FadeIn", true);
            yield return new WaitForSecondsRealtime(fadeInTime);
            midTransition = true;

            fadeObj.gameObject.SetActive(false);
            for (int i = 0; i < fadeObj.childCount; i++)
            {
                fadeObj.GetChild(i).gameObject.SetActive(false);
            }
            blackBGObj.gameObject.SetActive(true);

            AsyncOperation op = SceneManager.LoadSceneAsync(levelName, mode);

            while (!op.isDone)
            {
                yield return null;
            }

            anim.SetBool("FadeIn", false);
            yield return new WaitForSecondsRealtime(fadeOutTime);

            fadeObj.gameObject.SetActive(true);
            for (int i = 0; i < fadeObj.childCount; i++)
            {
                fadeObj.GetChild(i).gameObject.SetActive(true);
            }

            blackBGObj.gameObject.SetActive(false);

            midTransition = false;
        }

    }
}