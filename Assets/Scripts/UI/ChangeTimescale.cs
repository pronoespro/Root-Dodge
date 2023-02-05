using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro
{
    public class ChangeTimescale : MonoBehaviour
    {

        public float activeTimescale, deactivatedTimescale;

        private void OnEnable()
        {
            Time.timeScale = activeTimescale;
        }

        private void OnDisable()
        {
            Time.timeScale = deactivatedTimescale;
        }

        public void ChangeTimeScale(float timescale)
        {
            Time.timeScale = timescale;
        }

    }
}