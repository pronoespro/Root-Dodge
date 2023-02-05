using System.Collections;
using UnityEngine;

namespace PronoesPro.Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerSlopedMovement : MonoBehaviour
    {

        public bool active;

        [SerializeField] private Transform feetPosStartTransform, feetPosEndTransform, slopePosEndTransform;
        [SerializeField] private float velocityMultiplier = 5;
        [SerializeField] private float distanceMultiplier = 10;
        [SerializeField] private float velocityPercentageToAdd = 0.25f;
        [SerializeField] private float slopeYOffset;
        [SerializeField] private float slopeMinDiference = 0.1f;
        [SerializeField] private float snapVelY = -0.1f;
        [SerializeField] private float snapThreshold = 0.75f;
        [SerializeField] private LayerMask mask;

        Rigidbody2D rb;
        Vector2 rayCastOrigin;
        Vector3 hitPoint;
        float direction;
        PlayerMovement move;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(CheckRaise());
            move = GetComponent<PlayerMovement>();
        }

        public IEnumerator CheckRaise()
        {
            while (true)
            {
                if (active)
                {
                    float distance = Mathf.Abs(feetPosEndTransform.position.y - feetPosStartTransform.position.y);
                    rayCastOrigin = (Vector2)feetPosStartTransform.position + new Vector2(direction, 0) * velocityMultiplier;
                    RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, distance, mask);
                    if (hit.transform != transform && hit.fraction < 1f && hit.point != rayCastOrigin)
                    {
                        if (hit.distance > 0f)
                        {
                            hitPoint = hit.point;

                            float endYVel = (distance - hit.distance) * distanceMultiplier * Mathf.Abs(1 + Mathf.Abs(rb.velocity.x) * velocityPercentageToAdd);

                            if (rb.velocity.y < endYVel)
                            {
                                SendMessage("GroundPlayer");
                            }
                            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, endYVel));
                        }
                        else
                        {
                            hit = Physics2D.Raycast(feetPosStartTransform.position, Vector2.down, distance, mask);
                            if (hit.transform != transform && hit.fraction < 1f && hit.point != (Vector2)feetPosStartTransform.position && hit.distance > 0f)
                            {
                                hitPoint = hit.point;

                                float endYVel = (distance - hit.distance) * distanceMultiplier * Mathf.Abs(1 + Mathf.Abs(rb.velocity.x) * velocityPercentageToAdd);

                                if (rb.velocity.y < endYVel)
                                {
                                    SendMessage("GroundPlayer");
                                }
                                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, endYVel));
                            }
                        }
                    }
                    if (move != null)
                    {
                        SnapToSlopes();
                    }
                }
                yield return null;
            }
        }

        public void SnapToSlopes()
        {
            if (move.GetGrounded())
            {
                float distance = Mathf.Abs(slopePosEndTransform.position.y - feetPosEndTransform.position.y);
                rayCastOrigin = feetPosEndTransform.position;
                RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, distance, mask);
                if (hit.transform != transform && hit.fraction < 1f && hit.distance > slopeMinDiference && hit.point != rayCastOrigin)
                {
                    hitPoint = hit.point;

                    rb.position = Vector2.Lerp(rb.position, new Vector2(rb.position.x, hitPoint.y + slopeYOffset), snapThreshold * Time.deltaTime);
                    rb.velocity = new Vector2(rb.velocity.x, snapVelY);
                    move.GroundPlayer();
                }
            }
        }

        public bool Collides(Transform t)
        {
            float distance = Mathf.Abs(feetPosEndTransform.position.y - feetPosStartTransform.position.y);
            rayCastOrigin = (Vector2)feetPosStartTransform.position + new Vector2(direction, 0) * velocityMultiplier;
            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, distance, mask);
            if (hit.transform != transform && hit.fraction < 1f && hit.distance > 0f && hit.point != rayCastOrigin)
            {
                if (hit.transform == t)
                {
                    return true;
                }
            }
            return false;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPoint, 0.02f);
        }

        public void DeactivateSlopes(string data)
        {
            float yDir;
            if (float.TryParse(data, out yDir))
            {
                active = yDir >= 0;
            }
            active = true;
        }

        public void ActivateSlopes()
        {
            active = false;
        }

        public void ChangeCharacterDirection(string dir)
        {
            float result;
            if (float.TryParse(dir, out result))
            {
                direction = result;
            }
        }

    }
}