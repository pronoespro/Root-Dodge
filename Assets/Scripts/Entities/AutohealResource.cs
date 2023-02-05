using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity
{
    public class AutohealResource : MonoBehaviour
    {

        public string resource = "health";
        public int ammount = 1;
        public float healTime = 0.5f;

        public Transform target;

        private float healTimer;

        public void Update()
        {
            healTimer += Time.deltaTime;
            while (healTimer > healTime)
            {
                target.SendMessage("AddToResource", resource + ',' + ammount.ToString());
                healTimer -= healTime;
            }
        }
    }
}