using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles
{
    public class DamageOnCollision : MonoBehaviour
    {

        public bool normalDamage = true;
        public int damage;
        public LayerMask damageMask;

        [Space(10),Header("If not normal damage")]
        public string resourceName;

        private EffectOnCollision effect;

        private void Start()
        {
            effect = GetComponent<EffectOnCollision>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (damageMask == (damageMask | (1 << collision.gameObject.layer)))
            {
                if (normalDamage)
                {
                    collision.transform.SendMessage("Hurt", damage);
                }
                else
                {
                    collision.transform.SendMessage( "RemoveFromResource",resourceName+","+damage.ToString());
                }
                if (effect != null) {
                    effect.Collide();
                    effect.collisionTarget = (effect.collisionTarget != null) ? effect.collisionTarget : collision.transform;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (damageMask == (damageMask | (1 << collision.gameObject.layer)))
            {
                if (normalDamage)
                {
                    collision.transform.SendMessage("Hurt", damage);
                }
                else
                {
                    collision.transform.SendMessage("RemoveFromResource", resourceName + damage.ToString());
                }
                if (effect != null)
                {
                    effect.Collide();
                    effect.collisionTarget = (effect.collisionTarget != null) ? effect.collisionTarget : collision.transform;
                }
            }
        }

    }
}