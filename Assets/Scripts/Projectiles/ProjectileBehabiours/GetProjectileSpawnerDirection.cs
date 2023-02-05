 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles
{
    public class GetProjectileSpawnerDirection : ProjectileBehabiour
    {

        public override void AfterCreate()
        {
            transform.localScale = spawner.lossyScale;
        }

    }
}