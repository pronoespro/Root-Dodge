using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantLevelObject : MonoBehaviour
{

    public string nameTag;

    void Start()
    {
        if (LevelManager.instance != null)
        {
            LevelManager.instance.importantObjects.Add(nameTag, transform);
            Debug.Log(nameTag);
        }
    }

}
