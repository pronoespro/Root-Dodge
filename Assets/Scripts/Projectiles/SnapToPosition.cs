using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Extra
{

    public enum positionTypes{
        normal,
        aimLeft,
        aimRight
    }

    [System.Serializable]
    public class PossibleSnapPositions
    {
        public Transform[] positions;
        public float rotationOffset;
        public Transform parent;

        public positionTypes type;

        public bool IsUsable()
        {
            switch (type)
            {
                default:
                    return true;
                case positionTypes.aimLeft:
                    if (parent==null){
                        return false;
                    }
                    return parent.localScale.x <= 0;
                case positionTypes.aimRight:
                    if (parent == null)
                    {
                        return false;
                    }
                    return parent.localScale.x > 0;
            }
        }

    }

    public class SnapToPosition : MonoBehaviour
    {

        public PossibleSnapPositions[] snapPositions;

        private int curPos;
        private Vector3 desPos;
        private float desRot;

        private void Update()
        {
            CheckPositions();

            if (curPos >= 0)
            {
                ApplySnapPosition();
            }
        }

        private void ApplySnapPosition()
        {
            desPos = Vector3.zero;
            desRot = 0;
            if (snapPositions[curPos].positions.Length > 0)
            {
                for(int i = 0; i < snapPositions[curPos].positions.Length;i++)
                {
                    desPos += snapPositions[curPos].positions[i].position;
                    if (i == 0)
                    {
                        desRot = snapPositions[curPos].positions[i].rotation.eulerAngles.z;
                    }
                    else
                    {
                        desRot = Quaternion.Slerp(snapPositions[curPos].positions[i].rotation, Quaternion.Euler(0, 0, desRot), 0.5f).eulerAngles.z;
                    }
                }
                desPos /= snapPositions[curPos].positions.Length;
            }
            transform.position = desPos;
            transform.rotation = Quaternion.Euler(0, 0, desRot+snapPositions[curPos].rotationOffset);
        }

        private void CheckPositions()
        {
            curPos = -1;
            for(int i = 0; i < snapPositions.Length; i++)
            {
                if (snapPositions[i].IsUsable())
                {
                    curPos = i;
                }
            }
        }

    }
}