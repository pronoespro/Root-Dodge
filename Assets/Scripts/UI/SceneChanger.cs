using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PronoesPro.UI
{
    public class SceneChanger : MonoBehaviour
    {

        public void ChangeScene(string sceneName)
        {
            if (FadeManager.instance != null)
            {
                FadeManager.instance.LoadLevel(sceneName);
            }
        }

        public void ChangeToLastGameplayScene(string defaultScene)
        {
            if (GameManager.instance != null)
            {
                ChangeScene(GameManager.instance.AddOrGetVariable("savedLevel", defaultScene));
            }
            else
            {
                ChangeScene(defaultScene);
            }
        }

        public void LoadSceneOnTop(string sceneName)
        {
            if (FadeManager.instance != null)
            {
                FadeManager.instance.LoadLevel(sceneName, LoadSceneMode.Additive);
            }
        }

    }
}