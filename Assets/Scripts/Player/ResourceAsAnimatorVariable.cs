using PronoesPro.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PronoesPro.Player.Animation;

namespace PronoesPro.Player
{
    [RequireComponent(typeof(PlayerAnimator))]
    public class ResourceAsAnimatorVariable : MonoBehaviour
    {
        public Resource resource;
        public string varName;

        private PlayerAnimator anim;

        private void Start()
        {
            anim = GetComponent<PlayerAnimator>();
            if (resource != null)
            {
                resource.OnResourceChanged.AddListener(() => SendResourceToAnim());
                SendResourceToAnim();
            }
        }

        public void SendResourceToAnim()
        {
            anim.SetAnimatorVariable(varName+ '|'+resource.resource);
        }

    }
}