using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionsOnActivate : ActionReader
{

    public UnityEvent enableEvent;
    public string[] actions;
    public float delay;

    public bool doOnEnable=true, doOnDisable;

    private bool calledEnable;

    public void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            if (doOnEnable && !calledEnable)
            {
                Invoke("CallEvent", delay);
                Debug.Log("Called event");
            }
            calledEnable = true;
        }
    }

    public void CallEvent()
    {
        enableEvent.Invoke();
        for (int i = 0; i < actions.Length; i++)
        {
            DoEffect(actions[i]);
        }
    }

    public void OnDisable()
    {
        calledEnable = false;
        CancelInvoke();
        if (doOnDisable)
        {
            CallEvent();
        }
    }


}
