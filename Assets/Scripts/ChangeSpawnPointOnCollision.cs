using PronoesPro.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpawnPointOnCollision : MonoBehaviour
{
    public Vector3 offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpawnPoint sp = collision.GetComponent<SpawnPoint>();
        if (sp!=null)
        {
            sp.spawnPoint = transform.position+offset;
        }
    }

}
