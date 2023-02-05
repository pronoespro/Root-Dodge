using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PronoesPro.Managers;

namespace PronoesPro.Particles
{

    [System.Serializable]
    public class ParticleBase
    {
        public string name;
        public bool important;

        [Space(15)]
        public GameObject prefav;
        public int ammountToCreate;

        [HideInInspector]
        public List<Transform> createdParticles;
        [HideInInspector]
        public int lastUsed;

        public Transform GetNext()
        {
            lastUsed = (lastUsed + 1) % createdParticles.Count;
            return createdParticles[lastUsed];
        }

    }
    public class ParticleManager : PrefavManager
    {

        #region instancing
        public static ParticleManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        public ParticleSystem CreateParticles(string name)
        {
            Transform t = CreatePrefav(name);
            if (t != null)
            {
                ParticleSystem particles = t.GetComponent<ParticleSystem>();

                if (particles != null)
                {
                    return particles;
                }

            }
            return null;
        }

        public ParticleSystem CreateTrail(string name)
        {
            Transform t = GetPrefav(name);
            if (t != null)
            {
                ParticleSystem particles = t.GetComponent<ParticleSystem>();

                if (particles != null)
                {
                    return particles;
                }

            }
            return null;
        }

        public void PlayLastParticleUsed()
        {
            if (lastCreated == null)
            {
                return;
            }
            ParticleSystem particles= lastCreated.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
                return;
            }
            particles = GetComponentInChildren<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
            }
            
        }

        public void StopLastParticleUsed()
        {
            if (lastCreated == null)
            {
                return;
            }
            ParticleSystem particles = lastCreated.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Stop();
                return;
            }
            particles = GetComponentInChildren<ParticleSystem>();
            if (particles != null)
            {
                particles.Stop();
            }
        }

    }
}