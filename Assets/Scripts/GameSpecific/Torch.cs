using PronoesPro.Entity;
using UnityEngine;

public class Torch : MonoBehaviour
{

    public LayerMask mask;
    public ParticleSystem particles;
    public Transform lightToDisable;
    public string resourceToAdd = "Fire";
    public int ammountToAdd = 1;

    private bool used;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (mask == (mask | (1 << collision.gameObject.layer)) && !used)
        {
            Resource[] resources = collision.GetComponents<Resource>();
            foreach (Resource res in resources)
            {
                if (res.resourceName ==resourceToAdd) {
                    collision.SendMessage("AddToResource", resourceToAdd+","+ ammountToAdd);
                    used = true;
                    particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    lightToDisable.gameObject.SetActive(false);
                }
            }
        }
    }

}
