using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PronoesPro.Managers;

namespace PronoesPro.Projectiles { 

    public class ProjectileManager : PrefavManager
    {

        #region instancing
        public static ProjectileManager instance;

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

        public Transform lastShooter;

        public void SetLastShooter(Transform shooter)
        {
            lastShooter = shooter;
        }

        public void InitializeLastProjectile()
        {
            if (lastCreated != null)
            {
                ProjectileBehabiour pb = lastCreated.GetComponent<ProjectileBehabiour>();
                if (pb != null)
                {
                    pb.Create(lastShooter);
                }
            }
        }

    }
}