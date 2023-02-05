using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PronoesPro.UI
{
    public class ControllMenuSlider : MonoBehaviour
    {

        public Slider slider;
        public int ammountToAdd = 10;

        [Space(10)]
        public bool canHoldModify;
        public float holdTime = 0.5f;
        public float holdRepeatTime = 0.1f;

        private bool holdingButton, repeating;
        private float holdTimer;
        private int modifyDirection;

        private void Update()
        {
            if (holdingButton)
            {
                if (repeating)
                {
                    if (holdTimer > holdRepeatTime)
                    {
                        slider.value = slider.value + modifyDirection * ammountToAdd;
                        holdTimer -= holdRepeatTime;
                    }
                }
                else
                {
                    if (holdTimer > holdTime)
                    {
                        slider.value = slider.value + modifyDirection * ammountToAdd;
                        holdTimer = 0;
                        repeating = true;
                    }
                }
                holdTimer += Time.deltaTime;
            }
        }

        public void ModifySlider(float direction)
        {
            if (canHoldModify)
            {
                holdingButton = true;
                modifyDirection = (int)direction;
                holdTime = 0;
                repeating = false;
            }
            else
            {
                slider.value = slider.value + (int)direction * ammountToAdd;
            }
        }

        public void ModifySlider(string direction)
        {
            float dir;
            if (float.TryParse(direction, out dir))
            {
                ModifySlider(dir);
            }
        }

        public void EndModifySlider()
        {
            holdingButton = false;
        }

    }
}