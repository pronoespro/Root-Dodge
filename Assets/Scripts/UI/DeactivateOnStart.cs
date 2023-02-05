using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.UI { 
public class DeactivateOnStart : MonoBehaviour
{
    public GameObject[] objs;

    private void Start()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(false);
        }
    }
}
}