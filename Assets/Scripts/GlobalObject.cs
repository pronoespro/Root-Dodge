using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour
{

    public static Dictionary<string, GameObject> instances;

    public string instanceName;

    public void Awake()
    {

        if (instances == null)
        {
            instances = new Dictionary<string, GameObject>();
        }

        if (instances.ContainsKey(instanceName))
        {
            DestroyImmediate(gameObject);
            return;
        }
        instances.Add(instanceName, gameObject);
        DontDestroyOnLoad(gameObject);
    }

}
