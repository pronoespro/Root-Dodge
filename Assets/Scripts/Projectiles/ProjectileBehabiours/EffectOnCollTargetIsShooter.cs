using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Projectiles {
    [RequireComponent(typeof(EffectOnCollision))]
    public class EffectOnCollTargetIsShooter : ProjectileBehabiour
    {

        EffectOnCollision col;

        public override void Create(Transform _spawner)
        {
            base.Create(_spawner);
            if (col == null)
            {
                col = GetComponent<EffectOnCollision>();
            }
            if (col != null)
            {
                col.collisionTarget = _spawner;
            }
        }

    }
}