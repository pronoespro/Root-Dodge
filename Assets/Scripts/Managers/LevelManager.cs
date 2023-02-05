using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    public bool changeReloadScene;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        importantObjects = new Dictionary<string, Transform>();
    }

    [SerializeField]
    public Dictionary<string, Transform> importantObjects;

    public void Start()
    {
        if (changeReloadScene)
        {
            GameManager.instance.SaveVariable("savedLevel", gameObject.scene.name);
        }
    }

    public bool ActivateImportantObject(string name,bool activate=true)
    {
        if (importantObjects.ContainsKey(name))
        {
            Debug.Log((activate?"":"de")+"activated object " + name);
            importantObjects[name].gameObject.SetActive(activate);
            return true;
        }
        Debug.Log("object " + name + " not found");
        return false;
    }

    public void SendMessageToObject(string name, string methodAndData)
    {
        if (importantObjects.ContainsKey(name))
        {
            string[] splitInfo = methodAndData.Split('-');

            if (splitInfo.Length > 1)
            {
                float result;
                if (float.TryParse(splitInfo[1], out result))
                {

                    importantObjects[name].SendMessage(splitInfo[0], result);
                }
                else
                {
                    importantObjects[name].SendMessage(splitInfo[0], splitInfo[1]);
                }
                Debug.Log("Send message to " + name + " with value of " + splitInfo[1]);
            }
            else
            {
                importantObjects[name].SendMessage(methodAndData);
                Debug.Log("Send message to " + name);
            }
        }
        Debug.Log("object not found");
    }

}
