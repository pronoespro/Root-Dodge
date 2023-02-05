using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity
{
    public class Health : Resource
    {

        public bool invulnerable;

        public override void Start()
        {
            base.Start();
            maxResource = resource;
        }

        public void SwitchInvincible(bool invincible)
        {
            invulnerable = invincible;
            if (invincible)
            {
                SendMessage("DisableTracking");
            }
            else
            {
                SendMessage("EnableTracking");
            }
        }

        public void Hurt(int damage)
        {
            if (!invulnerable && resource>0)
            {
                RemoveFromResource(damage);
                if (resource <= 0)
                {
                    resource = 0;
                    SendMessage("Die");
                }
                else
                {
                    SendMessage("GetHurt");
                }
            }
        }

        public void Heal(int ammount)
        {
            resource = Mathf.Clamp(resource + ammount, 0, maxResource);
        }

    }
}