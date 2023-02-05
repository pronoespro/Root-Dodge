using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateIfDeactivated : MonoBehaviour
{

    public Transform obj,target;

    public bool resetOnEnable, resetPos,thisObjectOnly=true;

    bool doing;

    private IEnumerator Start()
    {
        if (obj != null && target!=null)
        {
            doing = true;
            while (obj.gameObject.activeSelf)
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
        if(obj!=null && resetOnEnable && obj.gameObject.activeSelf && !doing)
        {
            StartCoroutine(Start());
        }
    }

    public void OnDisable()
    {
        if(transform.parent!=null && !transform.parent.gameObject.activeInHierarchy && thisObjectOnly)
        {
            return;
        }
        if (obj == null && target!=null)
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
