using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Managers
{

    [System.Serializable]
    public class PrefavBase
    {
        public string name;
        public bool important;

        [Space(15)]
        public GameObject prefav;
        public int ammountToCreate;

        [HideInInspector]
        public List<Transform> createdPrefavs;
        [HideInInspector]
        public int lastUsed;

        public Transform GetNext()
        {
            if (createdPrefavs.Count > 0)
            {
                lastUsed = (lastUsed + 1) % createdPrefavs.Count;
                return createdPrefavs[lastUsed];
            }
            else
            {
                return null;
            }
        }

    }

    public abstract class PrefavManager : MonoBehaviour
    {


        public PrefavBase[] prefavBases;

        protected Transform lastCreated;

        public virtual void Start()
        {
            foreach (PrefavBase b in prefavBases)
            {
                if (b.important)
                {
                    for (int i = 0; i < b.ammountToCreate; i++)
                    {
                        Transform t = Instantiate(b.prefav,transform).transform;
                        b.createdPrefavs.Add(t);
                        t.gameObject.SetActive(false);
                    }
                }
            }
        }

        public virtual Transform CreatePrefav(string name)
        {
            foreach (PrefavBase b in prefavBases)
            {
                if (b.name.ToLower() == name.ToLower())
                {

                    if (!b.important && b.createdPrefavs.Count==0)
                    {
                        for (int i = 0; i < b.ammountToCreate; i++)
                        {
                            Transform t = Instantiate(b.prefav, transform).transform;
                            b.createdPrefavs.Add(t);
                            t.gameObject.SetActive(false);
                        }
                    }

                    lastCreated = b.GetNext();
                    if (lastCreated != null)
                    {
                        lastCreated.gameObject.SetActive(true);
                    }
                    return lastCreated;
                }
            }
            return null;
        }

        public virtual Transform GetPrefav(string name)
        {
            foreach (PrefavBase b in prefavBases)
            {
                if (b.name.ToLower() == name.ToLower())
                {

                    if (!b.important && b.createdPrefavs.Count == 0)
                    {
                        for (int i = 0; i < b.ammountToCreate; i++)
                        {
                            Transform t = Instantiate(b.prefav, transform).transform;
                            b.createdPrefavs.Add(t);
                            t.gameObject.SetActive(false);
                        }
                    }

                    lastCreated = b.GetNext();
                    return lastCreated;
                }
            }
            return null;
        }

        public virtual void ChangeLastPrefavPosition(Vector3 position)
        {
            if (lastCreated != null)
            {
                lastCreated.position = position;
            }
        }

        public virtual void ChangeLastPrefavRotation(float rotation)
        {
            if (lastCreated != null)
            {
                lastCreated.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }


    }
}
