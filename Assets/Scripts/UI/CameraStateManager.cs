using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Cameras
{

    #region classes
    [System.Serializable]
    public class CamState
    {
        public string name;

        public Transform target;
        public Vector3 targetOffset;
        public float targetLerp;

        [Space(15)]

        public float rotation;
        public float rotationLerp;

        [Space(15)]

        public float shakeSpeed;
        public Vector2 screenShake;

        [Space(15)]

        public float camSizeMultiplier=1;
        public float sizeChangeSpeed=0.5f;
    }
    #endregion

    public class CameraStateManager : MonoBehaviour
    {

        #region Instancing
        public static CameraStateManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion


        public CamState[] states;
        [SerializeField] private bool isPerspective;

        private int curState;
        private float defaultSize;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
            if (cam != null) {
                defaultSize =(isPerspective)?cam.fieldOfView:cam.orthographicSize;
            }
            StartCoroutine(DoCameraState());
        }

        public void ChangeCameraState(int state)
        {
            if (state < states.Length)
            {
                curState = state;
            }
        }

        public IEnumerator DoCameraState()
        {
            Vector3 camDesiredPos;
            while (true)
            {
                camDesiredPos = Vector3.zero;

                if (states[curState].target != null)
                {
                    camDesiredPos = states[curState].target.position + states[curState].targetOffset;
                    camDesiredPos = Vector3.Lerp(transform.position, camDesiredPos, states[curState].targetLerp*Time.deltaTime);
                }

                if (states[curState].shakeSpeed != 0)
                {
                    camDesiredPos += new Vector3(states[curState].screenShake.x * Mathf.Cos(Time.time * states[curState].shakeSpeed), states[curState].screenShake.y * Mathf.Sin(Time.time * states[curState].shakeSpeed));
                }

                if (cam != null)
                {
                    if (isPerspective)
                    {

                        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultSize * states[curState].camSizeMultiplier, states[curState].sizeChangeSpeed * Time.deltaTime);
                    }
                    else
                    {
                        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultSize * states[curState].camSizeMultiplier, states[curState].sizeChangeSpeed * Time.deltaTime);
                    }
                }

                transform.position = camDesiredPos;
                yield return null;
            }
        }

    }

}