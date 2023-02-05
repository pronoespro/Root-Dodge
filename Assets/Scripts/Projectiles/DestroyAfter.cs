using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles
{
    public class DestroyAfter : MonoBehaviour
    {

        public float dissapearTimer = 1;
        public bool disableInstead;

        private void OnEnable()
        {
            StartCoroutine(DeactivateOnTimer());
        }

        IEnumerator DeactivateOnTimer(){
            float timer = 0;
            while (timer < dissapearTimer)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (disableInstead)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

    }
}