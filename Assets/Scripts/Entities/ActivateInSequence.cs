using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInSequence : MonoBehaviour
{

    public GameObject[] objects;
    public float delay;

    private int curObject;
    private float delayTimer;

    private void Update()
    {
        delayTimer += Time.deltaTime;
    }

    private void OnEnable()
    {
        if (objects.Length > 0 && delayTimer>delay)
        {
            objects[curObject].SetActive(true);
            curObject = (curObject + 1) % objects.Length;
        }
    }

}
