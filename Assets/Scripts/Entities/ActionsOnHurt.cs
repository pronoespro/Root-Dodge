using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity
{
    public class ActionsOnHurt : ActionReader
    {

        public string hurtActions;
        public string deathActions;
        public bool doHurtOnDeath;

        public void GetHurt()
        {
            DoEffect(hurtActions);
        }

        public void Die()
        {
            if (doHurtOnDeath)
            {
                DoEffect(hurtActions);
            }
            DoEffect(deathActions);
        }

    }
}