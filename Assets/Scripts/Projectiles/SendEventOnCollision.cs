using PronoesPro.Player.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEventOnCollision : MonoBehaviour
{

    public Transform target;

    public string eventName;
    public VariableType variableType;
    public string eventData;
    public float eventDelay;

    [Space(15)]
    public LayerMask collisionMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            target =(target!=null)?target: collision.transform;
            Invoke("SendMessageToTarget",eventDelay); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            target = (target != null) ? target : collision.transform;
            Invoke("SendMessageToTarget", eventDelay);
        }
    }

    public void SendMessageToTarget()
    {
        switch (variableType)
        {
            case VariableType.boolean:
                target.SendMessage(eventName, eventData.ToLower().Contains("t"));
                break;
            case VariableType.floatNumber:
                float resultFloat;
                if (float.TryParse(eventData,out resultFloat))
                {
                    target.SendMessage(eventName, resultFloat);
                }
                break;
            case VariableType.integerNumber:
                int resultInt;
                if (int.TryParse(eventData, out resultInt))
                {
                    target.SendMessage(eventName, resultInt);
                }
                break;
            case VariableType.reset:
                target.SendMessage(eventName, eventData);
                break;
            case VariableType.triggerOrNone:
                target.SendMessage(eventName);
                break;
        }
    }

}
