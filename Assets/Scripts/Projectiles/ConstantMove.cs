using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ConstantMove : MonoBehaviour
    {

        public Vector3 Velocity;
        public bool Relative;
        public float SlowDownOverTime;
        public bool ResetOnEnable=true;

        private Rigidbody2D rb;
        private float curSlowDown;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Vector3 finalVel = new Vector3(Velocity.x * transform.localScale.x, Velocity.y * transform.localScale.y, Velocity.z * transform.localScale.z);
            rb.velocity = (Relative?transform.rotation: Quaternion.identity) * finalVel*Mathf.Clamp01(1-curSlowDown);
            curSlowDown += SlowDownOverTime * Time.deltaTime;
        }

        private void OnEnable()
        {
            curSlowDown = 0;
        }

    }
}