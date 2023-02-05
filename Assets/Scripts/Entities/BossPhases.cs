using PronoesPro.Entity.Tracking;
using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PronoesPro.Entity.Boss
{

    [System.Serializable]
    public class PhaseAction
    {
        public string name;
        public float moment;
        public string script;
    }

    [System.Serializable]
    public class Phase
    {
        public string name;
        public float duration;
        public PhaseAction[] actions;
        public Transform[] shooters;
        public bool canCancel;

        private bool[] doneActions;
        private int curShooter;

        public bool ActionIsDone(int actionNum)
        {
            if (actionNum>actions.Length)
            {
                return false;
            }
            return doneActions[actionNum];
        }

        public void DoAction(int actionNum)
        {
            if (actionNum < actions.Length)
            {
                doneActions[actionNum] = true;
            }
        }

        public void Reset()
        {
            doneActions = new bool[actions.Length];
        }

        public int GetNextShooter()
        {
            if (shooters.Length > 0)
            {
                curShooter = (curShooter + 1) % shooters.Length;
            }
            return curShooter;
        }

        public Transform GetCurShooter()
        {
            return shooters[curShooter];
        }

    }

    public class BossPhases : ActionReader
    {

        public Phase[] phases;
        public bool unscaledTime;


        private int curPhase;
        private bool[] activePhases;
        private float phaseTimer = 0;
        private bool hurt;

        public bool GetIsHurt() { return hurt; }

        public void HurtEffect()
        {
            Health _health = GetComponent<Health>();
            if (_health == null || !_health.invulnerable)
            {
                hurt = true;
            }
        }

        public void EndHurt(){
            hurt = false;
        }

        protected override void Start()
        {
            base.Start();
            tracker = GetComponent<TrackEntity>();
            activePhases = new bool[phases.Length];
            for (int i = 0; i < activePhases.Length; i++)
            {
                activePhases[i] = true;
            }

            if (phases != null && phases.Length > 0)
            {
                foreach (Phase phase in phases)
                {
                    phase.Reset();
                }
                StartCoroutine(DoPhase());
            }
        }

        public override bool CustomCommand(string[] commandData)
        {
            if (commandData[0].ToLower() == "shoot"|| commandData[0].ToLower()== "spawnheld" || (commandData[0].ToLower()== "particles" && commandData[2]== "special"))
            {
                phases[curPhase].GetNextShooter();
            }
            switch (commandData[0].ToLower())
            {
                case "tp-target":
                    if (tracker != null && tracker.GetTarget() != null)
                    {
                        if (phases[curPhase].shooters.Length > 0)
                        {
                            tracker.GetTarget().position = phases[curPhase].GetCurShooter().position;
                        }
                        else
                        {
                            if (commandData[1] == "pos")
                            {
                                tracker.GetTarget().position = transform.position;
                            }
                            else if (commandData[1] == "spawn")
                            {
                                SpawnPoint sp = tracker.GetTarget().GetComponent<SpawnPoint>();
                                if (sp != null)
                                {
                                    sp.TeleportToSpawn();
                                }
                                else
                                {
                                    tracker.GetTarget().position = transform.position;
                                }
                            }
                            else
                            {
                                tracker.GetTarget().position = Vector3.zero;
                            }
                        }
                    }
                    break;
                case "cancel-move":
                case "stop-move":
                    if (curMoveRoutine != null)
                    {
                        StopCoroutine(curMoveRoutine);
                        curMoveRoutine = null;
                    }
                    break;
                case "cancel-phase":
                    BossPhases _bossPhases = GetComponent<BossPhases>();
                    if (_bossPhases != null && commandData[1] != null && commandData[1].Length > 0)
                    {
                        for (int i = 0; i < _bossPhases.phases.Length; i++) {
                            if (_bossPhases.phases[i].name == commandData[1])
                            {
                                _bossPhases.activePhases[i] = false;
                            }
                        }
                    }
                    break;
                case "hurt-effect":
                        HurtEffect();
                    break;
            }
            return base.CustomCommand(commandData);
        }

        public IEnumerator DoPhase()
        {
            phaseTimer = 0;
            while (true)
            {
                phaseTimer +=(unscaledTime?Time.unscaledDeltaTime: Time.deltaTime);

                if(!activePhases[curPhase] && phases[curPhase].canCancel)
                {
                    phaseTimer = float.MaxValue - 1;
                }

                if (phaseTimer < phases[curPhase].duration)
                {
                    for(int i = 0; i < phases[curPhase].actions.Length; i++)
                    {
                        if (phases[curPhase].actions[i].moment < phaseTimer && !phases[curPhase].ActionIsDone(i))
                        {
                            DoEffect(phases[curPhase].actions[i].script);
                            phases[curPhase].DoAction(i);
                        }
                    }
                }
                else
                {
                    phases[curPhase].Reset();

                    phaseTimer = 0;
                    int initphase = curPhase;

                    curPhase = (curPhase + 1) % phases.Length;
                    while (!activePhases[curPhase] && curPhase!=initphase)
                    {
                        curPhase = (curPhase+1) % phases.Length;
                    }
                }

                yield return null;
            }
        }

        public override Transform SpecialTransform()
        {
            if (phases==null || phases.Length==0)
            {
                return base.SpecialTransform();
            }

            return (phases[curPhase].shooters.Length>0? phases[curPhase].GetCurShooter():base.SpecialTransform());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void CancelPhase(string phaseName,bool cancel=true)
        {

            if (activePhases == null)
            {
                activePhases = new bool[phases.Length];
                for (int i = 0; i < activePhases.Length; i++)
                {
                    activePhases[i] = true;
                }
                Debug.Log("Active phases reset");
            }

            for(int i = 0; i < phases.Length; i++)
            {
                if (phases[i].name == phaseName)
                {
                    activePhases[i] = !cancel;
                }
            }

            if (!activePhases[curPhase] && phases[curPhase].canCancel)
            {

                if (curMoveRoutine != null){
                    StopCoroutine(curMoveRoutine);
                }

                phases[curPhase].Reset();
                phaseTimer = 0;

                int initphase = curPhase;
                curPhase = (curPhase + 1) % phases.Length;
                while (!activePhases[curPhase] && curPhase != initphase)
                {
                    curPhase = (curPhase + 1) % phases.Length;
                }
            }

        }

        public void ForceCancelPhase(string phaseName)
        {

            if (activePhases == null)
            {
                activePhases = new bool[phases.Length];
                for (int i = 0; i < activePhases.Length; i++)
                {
                    activePhases[i] = true;
                }
            }

            for (int i = 0; i < phases.Length; i++)
            {
                if (phases[i].name == phaseName)
                {
                    activePhases[i] = false;
                }
            }

            if (!activePhases[curPhase])
            {

                if (curMoveRoutine != null)
                {
                    StopCoroutine(curMoveRoutine);
                }

                phases[curPhase].Reset();
                phaseTimer = 0;

                int initphase = curPhase;
                curPhase = (curPhase + 1) % phases.Length;
                while (!activePhases[curPhase] && curPhase != initphase)
                {
                    curPhase = (curPhase + 1) % phases.Length;
                }
            }
        }

    }
}