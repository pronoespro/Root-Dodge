using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity
{
    public class SpawnPoint : MonoBehaviour
    {

        public Vector3 spawnPoint;

        private void OnEnable()
        {
            spawnPoint = transform.position;
        }

        public void ChangeSpawn(Vector2 spawn)
        {
            spawnPoint = spawn;
        }

        public void TeleportToSpawn()
        {
            transform.position = spawnPoint;
        }

    }
}