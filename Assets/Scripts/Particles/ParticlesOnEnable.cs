using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Particles
{
    public class ParticlesOnEnable : MonoBehaviour
    {

        public string particleName;
        public bool whenEnabled,whenDisabled;

        private void OnEnable()
        {
            if (whenEnabled)
            {
                Invoke("CreateParticle", 0.0001f);
            }
        }

        private void OnDisable()
        {
            if (whenDisabled)
            {
                ParticleManager.instance.CreateParticles(particleName);
                ParticleManager.instance.ChangeLastPrefavPosition(transform.position);
                ParticleManager.instance.PlayLastParticleUsed();
            }
        }

        public void CreateParticle()
        {
            ParticleManager.instance.CreateParticles(particleName);
            ParticleManager.instance.ChangeLastPrefavPosition(transform.position);
            ParticleManager.instance.PlayLastParticleUsed();
        }

    }
}