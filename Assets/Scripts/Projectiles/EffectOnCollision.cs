using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PronoesPro.Scripting;

namespace PronoesPro.Projectiles
{
    public class EffectOnCollision : ActionReader
    {

        public string[] effects;
        public Transform collisionTarget;

        public void Collide()
        {
            foreach (string effect in effects)
            {
                if (DoEffect(effect))
                {
                    Debug.Log("Effect done!");
                }
            }
        }

        public override bool CustomCommand(string[] commandData)
        {
            switch (commandData[0].ToLower())
            {
                case "drain":
                    if (collisionTarget != null)
                    {
                        collisionTarget.SendMessage("RemoveFromResource", commandData[1]);
                    }else {
                        Debug.Log("No target");
                    }
                    break;
            }
            return base.CustomCommand(commandData);
        }

    }
}