using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Particles.Effects
{
    public class ParticleTrail : MonoBehaviour
    {

        public string particleName;

        private ParticleSystem trail;

        private void OnEnable()
        {
            trail = ParticleManager.instance.CreateTrail(particleName);
            if (trail != null)
            {
                trail.transform.position = transform.position;
            }
        }

        private void Update()
        {
            if (trail != null)
            {
                trail.transform.position = transform.position;
                trail.transform.rotation = transform.rotation;
                trail.transform.localScale = transform.localScale;
                trail.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (trail != null)
            {
                trail.gameObject.SetActive(false);
            }
            trail = null;
        }

    }
}