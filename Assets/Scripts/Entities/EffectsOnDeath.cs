using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PronoesPro.Entity
{
    public enum DeathEffect { 
        destroy,
        particles,
        projectiles
    }

    public class EffectsOnDeath : MonoBehaviour
    {

        public DeathEffect[] effects;

        public void Die()
        {
            foreach(DeathEffect effect in effects)
            {
                switch (effect)
                {
                    case DeathEffect.destroy:
                        Destroy(gameObject);
                        return;
                }
            }
        }

    }
}