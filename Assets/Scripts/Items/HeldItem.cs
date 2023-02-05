using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Items
{

    [System.Serializable]
    public class HeldItemPosition
    {
        public string name;
        public bool showItem;

        [Space(15)]
        public Transform targetPosition;
        public Vector3 posOffset;

        [Space(15)]
        public bool followRotation;
        public float rotOffset;
    }

    public class HeldItem : MonoBehaviour
    {

        public HeldItemPosition[] positions;
        public float sizeMultiplier=1f;
        public SpriteRenderer rend;

        private Vector2 desiredSize;
        private int curPosition;

        private void Start()
        {
            rend =(rend!=null)?rend: GetComponent<SpriteRenderer>();
        }

        public HeldItemPosition CurHelpPosition()
        {
            return positions[curPosition];
        }

        private void Update()
        {
            transform.rotation = (CurHelpPosition().followRotation) ? CurHelpPosition().targetPosition.rotation * Quaternion.Euler(0,0,CurHelpPosition().rotOffset): transform.rotation;
            transform.localScale = desiredSize;

            if (CurHelpPosition().targetPosition != null)
            {
                transform.position = CurHelpPosition().targetPosition.position + (transform.rotation * CurHelpPosition().posOffset);
            }
        }

        public void SwitchSprite(Sprite sprite)
        {
            if (rend != null)
            {
                rend.sprite = sprite;
            }
        }

        public void RemoveSprite()
        {
            rend.sprite = null;
        }

        public void ChangePosition(string position)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                if (positions[i].name.ToLower() == position.ToLower())
                {
                    curPosition = i;
                    if (!positions[i].showItem)
                    {
                        SwitchSprite(null);
                    }
                    return;
                }
            }
        }

        public void ChangeSize(Vector2 size)
        {
            desiredSize =new Vector3(size.x,size.y,1)*sizeMultiplier;
        }

    }
}