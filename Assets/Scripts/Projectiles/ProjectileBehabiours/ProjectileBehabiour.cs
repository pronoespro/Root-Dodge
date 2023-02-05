using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles
{
    public class ProjectileBehabiour : MonoBehaviour
    {

        protected Transform spawner;

        public virtual void Create(Transform _spawner)
        {
            spawner = _spawner;
            AfterCreate();
        }

        public virtual void AfterCreate(){}

    }
}