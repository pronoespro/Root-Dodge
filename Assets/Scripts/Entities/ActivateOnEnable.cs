using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnEnable : MonoBehaviour
{
    public Transform obj, target;

    public bool resetOnDisable,resetPos;

    bool doing;

    private IEnumerator Start()
    {
        if (obj != null)
        {
            doing = true;
            while (!obj.gameObject.activeSelf)
            {
                yield return null;
            }

            if (!target.gameObject.activeSelf)
            {
                target.gameObject.SetActive(true);
            }

            if (resetPos)
            {
                target.localPosition = Vector3.zero;
            }
            doing = false;
        }
    }

    private void Update()
    {
        if (obj!=null && resetOnDisable && !obj.gameObject.activeSelf && !doing)
        {
            StartCoroutine(Start());
        }
    }

    public void OnEnable()
    {
        if (obj == null)
        {
            if (!target.gameObject.activeSelf)
            {
                target.gameObject.SetActive(true);
            }

            if (resetPos)
            {
                target.localPosition = Vector3.zero;
            }
        }
    }

}
