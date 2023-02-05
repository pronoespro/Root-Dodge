using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Player.Input
{
    [System.Serializable]
    public class InputToMethod
    {
        public string inputName;
        public string requiredInput;
        public string methodToLoad;
        public string extraValue;
    }

    public class PlayerController : MonoBehaviour
    {

        public InputToMethod[] possibleInput;
        public PlayerInputs input;
        public Dictionary<string, float> inputPressTime;

        private void Awake()
        {
            input = new PlayerInputs();

            input.Movement.Horizontal.performed += ctx => PerformInput("Horizontal",ctx.ReadValue<float>());
            input.Movement.Horizontal.canceled += ctx => PerformInput("CancelHorizontal",0.0f);

            input.Movement.Vertical.performed += ctx => PerformInput("Vertical", ctx.ReadValue<float>());
            input.Movement.Vertical.canceled += ctx => PerformInput("CancelVertical", 0.0f);

            input.Movement.Jump.performed += _ => PerformInput("Jump");
            input.Movement.Jump.canceled += _ => PerformInput("CancelJump");

            input.Movement.Run.performed += _ => PerformInput("Run");
            input.Movement.Run.canceled += _ => PerformInput("CancelRun");

            input.Menues.Scroll.performed += ctx => PerformInput("Scroll",ctx.ReadValue<float>());
            input.Menues.Pause.performed += ctx => PerformInput("Pause",ctx.ReadValue<float>());
            input.Menues.Select.performed += ctx => PerformInput("Select");

            input.Aiming.LeftClick.performed += _ => PerformInput("LeftClick");
            input.Aiming.LeftClick.canceled += _ => PerformInput("CancelLeftClick");

            input.Aiming.RightClick.performed += _ => PerformInput("RightClick");
            input.Aiming.RightClick.canceled+= _ => PerformInput("CancelRightClick");

            input.Inventory._1.performed += _ => PerformInput("Inventory1");
            input.Inventory._2.performed += _ => PerformInput("Inventory2");
            input.Inventory._3.performed += _ => PerformInput("Inventory3");
            input.Inventory._4.performed += _ => PerformInput("Inventory4");
            input.Inventory._5.performed += _ => PerformInput("Inventory5");
            input.Inventory._6.performed += _ => PerformInput("Inventory6");
            input.Inventory._7.performed += _ => PerformInput("Inventory7");
            input.Inventory._8.performed += _ => PerformInput("Inventory8");
            input.Inventory._9.performed += _ => PerformInput("Inventory9");
            input.Inventory._0.performed += _ => PerformInput("Inventory0");
        }

        private void Start()
        {
            inputPressTime = new Dictionary<string, float>();
        }

        public void PerformInput(string inputName, float value)
        {
            for (int i = 0; i < possibleInput.Length; i++)
            {
                if (possibleInput[i] != null && possibleInput[i].requiredInput == inputName)
                {
                    SendMessage(possibleInput[i].methodToLoad, GetData(possibleInput[i].extraValue, value));
                }
            }
        }

        public void PerformInput(string inputName)
        {
            for (int i = 0; i < possibleInput.Length; i++)
            {
                if (possibleInput[i].requiredInput == inputName)
                {
                    string normalInputName = inputName.Replace("Cancel", "");
                    if (inputName.StartsWith("Cancel"))
                    {
                        if (inputPressTime.ContainsKey(normalInputName))
                        {
                            SendMessage(possibleInput[i].methodToLoad, GetData(possibleInput[i].extraValue, inputPressTime[normalInputName]));
                            inputPressTime[normalInputName] = -1;
                        }
                        else
                        {
                            SendMessage(possibleInput[i].methodToLoad, GetData(possibleInput[i].extraValue, 0f));
                        }
                    }
                    else
                    {
                        SendMessage(possibleInput[i].methodToLoad, GetData(possibleInput[i].extraValue, 0f));
                        if (inputPressTime.ContainsKey(inputName))
                        {
                            inputPressTime[inputName] =(inputPressTime[inputName]>=0)?0:-1f;
                        }
                        else
                        {
                            inputPressTime.Add(inputName,0f);
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            input.Enable();
        }

        public void Update()
        {
            string[] keys = new string[inputPressTime.Count];
            int keyName = 0;
            foreach(string key in inputPressTime.Keys)
            {
                keys[keyName] = key;
                keyName++;
            }

            for (keyName = 0; keyName < keys.Length; keyName++) {
                if (inputPressTime[keys[keyName]] > 0)
                {
                    inputPressTime[keys[keyName]] += Time.deltaTime;
                }
            }
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void OnDestroy()
        {
            input.Dispose();
            input = null;
        }


        public string GetData(string original, float inputData)
        {
            string data="";

            if (!original.Contains("|") && !original.Contains("<"))
            {
                return original;
            }

            string[] splitData = original.Split('|');

            foreach(string s in splitData)
            {
                if (data.Length != 0)
                {
                    data += "|";
                }
                switch (s.ToLower())
                {
                    default:
                        data += s;
                        break;
                    case "<input>":
                        data += inputData.ToString();
                        break;
                    case "<absinput>":
                        data += Mathf.Abs(inputData).ToString();
                        break;
                    case "<-input>":
                        data +=(-inputData).ToString();
                        break;
                }
            }

            return data;
        }

        public void DeactivateInputsFor(float time)
        {
            input.Disable();
            CancelInvoke("ActivateInputs");
            Invoke("ActivateInputs", time);
        }

        public void DeactivateInputs()
        {
            input.Disable();
        }

        public void ActivateInputs()
        {
            input.Enable();
        }

    }
}