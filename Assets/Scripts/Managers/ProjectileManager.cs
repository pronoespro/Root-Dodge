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

        public void CreateProjectile(string name,Vector3? pos=null,float rot=0, Transform spawner = null)
        {
            CreatePrefav(name);
            lastShooter = spawner;
            if (pos != null)
            {
                ChangeLastPrefavPosition(pos.Value);
            }
            ChangeLastPrefavRotation(rot);
            InitializeLastProjectile();
        }

        public void SetLastShooter(Transform shooter)
        {
            lastShooter = shooter;
        }

        public void InitializeLastProjectile()
        {
            if (lastCreated != null)
            {
                ProjectileBehabiour[] pb = lastCreated.GetComponents<ProjectileBehabiour>();
                foreach (ProjectileBehabiour proj in pb)
                {
                    if (proj != null)
                    {
                        proj.Create(lastShooter);
                    }
                }
            }
        }

    }
}