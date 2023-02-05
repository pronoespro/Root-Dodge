using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Player.Animation
{

    public enum VariableType
    {
        triggerOrNone,
        reset,
        integerNumber,
        floatNumber,
        boolean
    }

    [System.Serializable]
    public class AnimationTriggers
    {
        public string name;
        public string variableName;
        public VariableType variableType;
    }

    [System.Serializable]
    public class AnimationVariableMultiplier
    {
        public string name;
        public float multiplier;
    }

    [System.Serializable]
    public class AnimationState
    {
        public string name;

        public string[] variableChangesToCancel;

        public string[] triggersToActivate;
        public string[] triggersToReset;

        public string[] trueBools;
        public string[] falseBools;

        public AnimationVariableMultiplier[] multipliers;
    }

    public class PlayerAnimator : MonoBehaviour
    {

        public AnimationTriggers[] triggers;
        public AnimationState[] states;

        public Animator anim;

        private int curState=0;
        private List<string> savedTriggers;

        private void Start()
        {
            savedTriggers = new List<string>();
        }

        private void Update()
        {
            List<string> triggersToRemove=new List<string>();
            foreach(string trigger in savedTriggers)
            {
                if (SetAnimatorVariable(trigger))
                {
                    triggersToRemove.Add(trigger);
                }
            }

            foreach(string trigger in triggersToRemove)
            {
                savedTriggers.Remove(trigger);
            }

            if (states.Length > 0)
            {
                if (curState > states.Length)
                {
                    curState = 0;
                }
                else
                {
                    foreach(string trig in states[curState].triggersToReset)
                    {
                        ResetTrigger(trig);
                    }
                    foreach (string trig in states[curState].triggersToActivate)
                    {
                        SetTrigger(trig);
                    }
                    foreach (string trig in states[curState].falseBools)
                    {
                        SetBoolFalse(trig);
                    }
                    foreach (string trig in states[curState].trueBools)
                    {
                        SetBoolTrue(trig);
                    }

                }
            }
        }

        public void ChangeAnimationState(string name)
        {
            for(int i = 0; i < states.Length; i++)
            {
                if (states[i].name.ToLower() == name.ToLower())
                {
                    curState = i;
                    return;
                }
            }
        }

        public bool SetAnimatorVariable(string data)
        {
            string[] separatedData = data.Split('|');
            foreach(AnimationTriggers trig in triggers)
            {
                if (VariableChangesCancelledContains(separatedData[0]))
                {
                    if (!savedTriggers.Contains(data))
                    {
                        savedTriggers.Add(data);
                    }
                    return false;
                }
                if (trig.name == separatedData[0])
                {
                    switch (trig.variableType)
                    {
                        default:
                            break;
                        case VariableType.triggerOrNone:
                            SetTrigger(trig.variableName);
                            break;
                        case VariableType.reset:
                            ResetTrigger(trig.variableName);
                            break;
                        case VariableType.floatNumber:
                            SetFloat(trig.variableName + "|" + separatedData[1]);
                            break;
                        case VariableType.integerNumber:
                            SetInt(trig.variableName + "|" + separatedData[1]);
                            break;
                        case VariableType.boolean:
                            SetBool(trig.variableName, separatedData[1]);
                            break;

                    }
                }
            }
            return true;
        }

        public bool VariableChangesCancelledContains(string trigger)
        {
            for(int i = 0; i < states[curState].variableChangesToCancel.Length; i++)
            {
                if (states[curState].variableChangesToCancel[i].ToLower() == trigger.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public void SetTrigger(string triggerName)
        {
            anim.SetTrigger(triggerName);
        }

        public void ResetTrigger(string triggerName)
        {
            anim.ResetTrigger(triggerName);
        }

        public void SetBool(string boolName, string boolValue)
        {
            if (boolValue.ToLower().Contains("t") || boolValue.ToLower().Contains("v"))
            {
                SetBoolTrue(boolName);
            }
            else
            {
                SetBoolFalse(boolName);
            }
        }

        public void SetBoolTrue(string boolName)
        {
            anim.SetBool(boolName,true);
        }

        public void SetBoolFalse(string boolName)
        {
            anim.SetBool(boolName,false);
        }

        public void SetInt(string variableData)
        {
            string[] data = variableData.Split('|');
            if (data.Length > 1)
            {
                int animInt;
                if (int.TryParse(data[1], out animInt))
                {

                    anim.SetInteger(data[0], (int)(animInt * GetMultiplier(data[0])));
                }
            }
        }

        public void SetFloat(string variableData)
        {
            string[] data = variableData.Split('|');
            if (data.Length > 1)
            {
                float animFloat;
                if (float.TryParse(data[1], out animFloat))
                {
                    anim.SetFloat(data[0], animFloat*GetMultiplier(data[0]));
                }
            }
        }

        public float GetMultiplier(string name)
        {

            for(int i = 0; i < states[curState].multipliers.Length;i++)
            {
                if (name == states[curState].multipliers[i].name)
                {
                    return states[curState].multipliers[i].multiplier;
                }
            }

            return 1f;
        }

    }
}