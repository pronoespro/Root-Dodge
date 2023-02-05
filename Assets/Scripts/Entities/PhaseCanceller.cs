using PronoesPro.Entity.Tracking;
using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity.Boss
{


    [RequireComponent(typeof(BossPhases))]
    public class PhaseCanceller : ActionReader
    {

        public Condition[] cancelConditions;
        public float minCheckVel;
        private BossPhases boss;

        protected override void Start()
        {
            base.Start();
            boss = GetComponent<BossPhases>();
            StartCoroutine(CancelPhases());
        }

        public IEnumerator CancelPhases()
        {
            while (true)
            {

                for (int i = 0; i < cancelConditions.Length; i++)
                {
                    if (cancelConditions[i].forceCancel)
                    {
                        if (ShouldCancelPhase(cancelConditions[i].conditionScript, cancelConditions[i].useTargetInstead))
                        {
                            boss.ForceCancelPhase(cancelConditions[i].name);
                        }
                    }else{
                        boss.CancelPhase(cancelConditions[i].name, ShouldCancelPhase(cancelConditions[i].conditionScript, cancelConditions[i].useTargetInstead));
                    }
                }
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    oldVelocity = rb.velocity;
                }

                yield return null;
            }
        }

        public bool ShouldCancelPhase(string conditionScript,bool useTarget)
        {
            string[] conditions = conditionScript.Split(';');

            if (conditions.Length > 1)
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!ConditionCheck(conditions[i],useTarget))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (!ConditionCheck(conditionScript,useTarget))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool ConditionCheck(string condition, bool useTarget)
        {
            if (!base.ConditionCheck(condition, useTarget))
            {
                return false;
            }
            if (condition.StartsWith("<") && condition.EndsWith(">"))
            {
                string[] splitCondition = condition.Split(':');
                splitCondition[0] = splitCondition[0].Remove(0, 1);
                if (splitCondition[splitCondition.Length - 1].Length > 0)
                {
                    splitCondition[splitCondition.Length - 1] = splitCondition[splitCondition.Length - 1].Remove(splitCondition[splitCondition.Length - 1].Length - 1, 1);
                }

                switch (splitCondition[0].ToLower())
                {
                    case "hurt":
                        if (splitCondition[1] == "not") {
                            return !boss.GetIsHurt();
                        }else{
                            return boss.GetIsHurt();
                        }
                    case "end-pain":
                        boss.EndHurt();
                        return true;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

    }
}