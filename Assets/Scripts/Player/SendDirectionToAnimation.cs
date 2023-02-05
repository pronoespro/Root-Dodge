using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Player.Animation
{

    public class SendDirectionToAnimation : MonoBehaviour
    {

        public string variableName;
        public bool useRigidbody;
        public bool sendZero;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                useRigidbody = false;
            }
        }

        void LateUpdate()
        {
            if (useRigidbody)
            {
                if (sendZero || Mathf.Abs(rb.velocity.x) > 0)
                {
                    float sign = (Mathf.Sign(rb.velocity.x) * Mathf.Sign(transform.localScale.x));
                    SendMessage("SetAnimatorVariable", variableName + "|" +(rb.velocity.x==0?0.ToString(): sign.ToString()));
                }
            }
            else
            {
                SendMessage("SetAnimatorVariable", variableName + "|" + Mathf.Sign(transform.localScale.x));
            }
        }
    }
}