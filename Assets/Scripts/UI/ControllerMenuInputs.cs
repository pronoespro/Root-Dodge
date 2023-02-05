using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PronoesPro.UI
{
    [System.Serializable]
    public class ControllerMenuInputOption
    {
        public string optionName;

        public Transform option;
        public UnityEvent optionEvent;
        public string optionActions;
    }
    public class ControllerMenuInputs : ActionReader
    {

        public ControllerMenuInputOption[] options;

        private int curOption;

        protected override void Start()
        {

            base.Start();

            foreach (ControllerMenuInputOption opt in options)
            {
                if (opt.option != null)
                {
                    opt.option.gameObject.SetActive(false);
                }
            }
            if (options[curOption].option != null)
            {
                options[curOption].option.gameObject.SetActive(true);
            }
        }

        public void MoveCursor(float direction)
        {
            options[curOption].option.gameObject.SetActive(false);

            curOption = (curOption + (int)direction) % options.Length;

            curOption = (curOption < 0) ? options.Length - 1 : curOption;

            options[curOption].option.gameObject.SetActive(true);
        }

        public void MoveCursor(string direction)
        {
            float dir;

            if (float.TryParse(direction, out dir))
            {
                MoveCursor(dir);
            }
        }

        public void SelectOption()
        {
            options[curOption].optionEvent.Invoke();
            DoEffect(options[curOption].optionActions);
        }

    }
}