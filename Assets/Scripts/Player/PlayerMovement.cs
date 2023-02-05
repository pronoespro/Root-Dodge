using UnityEngine;

namespace PronoesPro.Player.Movement
{

    [System.Serializable]
    public class MovementType
    {
        public string name;
        public bool defaultMove;
        public bool dontChangeMidDoing;

        [Space(5)]
        public float minPressTime;
        public float maxPressTime;

        [Space(5)]
        public bool canChangeSpeed;
        public bool faceVelocity;

        [Space(5)]
        public bool dependentPlayerDirection;
        public Vector2 forcedVelocity;

        [Space(5)]
        public float horizontalMoveSpeed;
        public float slidingAmmount;

        [Space(5)]
        public float gravityScale;
        public bool midairOnFinish;
        public float verticalMoveSpeed;
        public float jumpPower;
        public float jumpTime;
        public int airJumpTimes;
        public float airJumpPower = 0.5f;
        public float coyoteTime;
        public float forceFallVel=4;

        [Space(5)]
        public bool canWallJump;
        public float wallStickAmmount;
        public float wallJumpPower=1;
        public string wallJumpState;

        [Space(5)]
        public string[] requirements;

        public string animationState;
        public string playOnEnterState;

        public bool CheckPresstime(float time)
        {

            if(minPressTime!=0 && time >= minPressTime)
            {
                return true;
            }

            if(maxPressTime!=0 && time <= maxPressTime)
            {
                return true;
            }

            return false;
        }

    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {

        public float jumpCheckTimer = 0.1f;
        public float groundedDotProduct = 0.5f;
        public float jumpEnsureTime = 0.01f;
        public float contactIgnoreDistance=1f;

        public MovementType[] movementStates;

        public int curMoveState = 0;

        public string fallVelAnimVar;

        private Rigidbody2D rb;
        private Vector2 moveSpeed;
        private float slidingAmmount;
        private Vector2 curMoveDirection;
        private Vector2 inputDirection;

        private bool grounded;
        private float jumpTime;
        private float lastJumped=-1;
        private bool running;
        private float forceAirYSpeed;

        private float curFallTime;

        private float curMovementUsedTime;

        private bool askedBool;
        private bool cancelledJump;
        private int curJump = 0;
        private bool canAirJump;
        private bool collidingL, collidingR;
        private bool forceGrounded;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ApplyMovementState(0);
        }

        private void Update()
        {
            curMovementUsedTime += Time.deltaTime;

            if (grounded && rb.velocity.y <= 0)
            {
                canAirJump = false;
                curJump = 0;
                curFallTime = 0;
                if (lastJumped >= 0)
                {
                    grounded = false;
                    curFallTime = float.MaxValue;
                    curMoveDirection = new Vector2(curMoveDirection.x, movementStates[curMoveState].jumpPower);
                    lastJumped = -1;
                }
                else if (lastJumped >= jumpEnsureTime && rb.velocity.y <= movementStates[curMoveState].jumpPower * 0.75f)
                {
                    grounded = false;
                    curFallTime = float.MaxValue;
                    curMoveDirection = new Vector2(curMoveDirection.x, movementStates[curMoveState].jumpPower);
                    lastJumped = -1;
                }
                jumpTime = 0;
            }
            else
            {

                if (curFallTime < movementStates[curMoveState].coyoteTime)
                {
                    curFallTime += Time.deltaTime;
                    if (lastJumped >= 0)
                    {
                        grounded = false;
                        curMoveDirection = new Vector2(curMoveDirection.x, movementStates[curMoveState].jumpPower);
                        rb.velocity = new Vector2((Mathf.Abs(rb.velocity.x) > 0 ? rb.velocity.x : curMoveDirection.x), curMoveDirection.y);
                        curFallTime = float.MaxValue;
                        lastJumped = -1;
                    }
                    else if (lastJumped >= jumpEnsureTime && rb.velocity.y <= movementStates[curMoveState].jumpPower * 0.75f)
                    {
                        grounded = false;
                        curMoveDirection = new Vector2(curMoveDirection.x, movementStates[curMoveState].jumpPower);
                        rb.velocity = new Vector2((Mathf.Abs(rb.velocity.x) > 0 ? rb.velocity.x : curMoveDirection.x), curMoveDirection.y);
                        curFallTime = float.MaxValue;
                        lastJumped = -1;
                    }
                }
                else
                {
                    jumpTime += Time.deltaTime;
                    if (jumpTime < movementStates[curMoveState].jumpTime)
                    {
                        if (!cancelledJump)
                        {
                            curMoveDirection.y = movementStates[curMoveState].jumpPower;
                        }
                        else
                        {
                            curMoveDirection.y = 0;
                        }
                    }else{
                        if (lastJumped > 0 && canAirJump && curJump < movementStates[curMoveState].airJumpTimes)
                        {
                            jumpTime = movementStates[curMoveState].jumpTime*movementStates[curMoveState].airJumpPower;
                            curJump++;
                            cancelledJump = false;
                            canAirJump = false;
                            curMoveDirection.y = movementStates[curMoveState].jumpPower;
                        }
                        else
                        {
                            if (cancelledJump){
                                canAirJump = true;
                            }
                            curMoveDirection.y = 0;
                        }
                    }
                }
                rb.velocity = new Vector2(curMoveDirection.x*moveSpeed.x, (curMoveDirection.y > 0 ? curMoveDirection.y : rb.velocity.y)*moveSpeed.y);

                lastJumped -= Time.deltaTime;
            }
            if (fallVelAnimVar.Length > 0)
            {
                SendMessage("SetAnimatorVariable", fallVelAnimVar + "|" + rb.velocity.y);
            }

            if (movementStates[curMoveState].canChangeSpeed)
            {
                rb.velocity = new Vector2(Mathf.Lerp(curMoveDirection.x * moveSpeed.x, rb.velocity.x, slidingAmmount), rb.velocity.y + curMoveDirection.y * moveSpeed.y);
                rb.velocity = new Vector2(IsDirectionBlocked(rb.velocity.x) ? 0 : rb.velocity.x, rb.velocity.y);

                curMoveDirection = new Vector2(
                    (movementStates[curMoveState].horizontalMoveSpeed == 0f ? 0 : curMoveDirection.x),
                    (movementStates[curMoveState].verticalMoveSpeed == 0f ? 0 : curMoveDirection.y));

            }

            if (!movementStates[curMoveState].dontChangeMidDoing)
            {
                ChangeMovementToDefault();
            }
            else if(!MovementCheck(curMoveState))
            {
                ChangeMovementToDefault();
            }

            if (movementStates[curMoveState].faceVelocity)
            {
                transform.localScale = new Vector3((rb.velocity.x != 0 ? Mathf.Sign(rb.velocity.x) : transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (movementStates[curMoveState].forcedVelocity.magnitude > 0.1f)
            {
                Vector2 desVel = movementStates[curMoveState].forcedVelocity;
                desVel.x = (movementStates[curMoveState].dependentPlayerDirection ? desVel.x * transform.localScale.x : desVel.x);
                rb.velocity = movementStates[curMoveState].canChangeSpeed ? rb.velocity + desVel : desVel;
            }


        }

        private bool IsDirectionBlocked(float direction)
        {
            if (Mathf.Sign(direction) >= 0)
            {
                return collidingR;
            }
            else
            {
                return collidingL;
            }
        }

        public void ChangeMovementToDefault()
        {
            for (int i = 0; i < movementStates.Length; i++)
            {
                if (movementStates[i].defaultMove && MovementCheck(i))
                {
                    ChangeMovementType(movementStates[i].name);
                    return;
                }
            }
            curMoveState = 0;
        }

        public bool MovementCheck(int move, bool newMove = false)
        {
            foreach (string criteria in movementStates[move].requirements)
            {
                switch (criteria.ToLower())
                {
                    case "air":
                        if (GetGrounded())
                        {
                            return false;
                        }
                        break;
                    case "ground":
                        if (!GetGrounded())
                        {
                            return false;
                        }
                        break;
                    case "static":
                        if (curMoveDirection.magnitude > 0)
                        {
                            return false;
                        }
                        break;
                    case "move":
                        if (curMoveDirection.magnitude == 0)
                        {
                            return false;
                        }
                        break;
                    case "run":
                        if (!running)
                        {
                            return false;
                        }
                        break;
                }

                if (criteria.ToLower().Contains("resource"))
                {
                    string[] splitData = criteria.Split(':');
                    if (splitData.Length > 1) {
                        askedBool = false;

                        SendMessage((splitData[0].Contains("not")? "IfLessThanResource" : "NeedResource"),splitData[1]);

                        if (!askedBool)
                        {
                            return false;
                        }
                    }
                }else if (criteria.ToLower().Contains("timer") && !newMove)
                {
                    string[] splitData = criteria.ToLower().Split(':');
                    if (splitData.Length > 1)
                    {
                        float maxTime;
                        if (float.TryParse(splitData[1], out maxTime))
                        {
                            if (curMovementUsedTime >= maxTime)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Debug.LogError("Failed to find number in: '" + splitData[1] + "'");
                        }
                    }
                }
            }
            return true;
        }

        public void HasResource()
        {
            askedBool = true;
        }

        public bool GetGrounded()
        {
            if (rb.velocity.y > forceAirYSpeed || rb.velocity.y<-forceAirYSpeed)
            {
                grounded = false;
                return false;
            }
            return grounded || curFallTime < movementStates[curMoveState].coyoteTime;
        }

        public MovementType GetMovement()
        {
            return movementStates[curMoveState];
        }

        public void ChangeMovementType(string movementData)
        {
            if (movementData.Contains("|"))
            {
                string[] moveSplitData = movementData.Split('|');
                string moveName = moveSplitData[0];

                if (moveName.ToLower() == movementStates[curMoveState].name.ToLower())
                {
                    return;
                }
                for (int i = 0; i < movementStates.Length; i++)
                {
                    float parsedPressTime;
                    if (float.TryParse(moveSplitData[1],out parsedPressTime))
                    {
                        if (movementStates[i].name.ToLower() == moveName.ToLower() && MovementCheck(i, true) && movementStates[i].CheckPresstime(parsedPressTime))
                        {
                            ApplyMovementState(i);
                        }
                    }
                    else
                    {

                        if (movementStates[i].name.ToLower() == moveName.ToLower() && MovementCheck(i, true))
                        {
                            ApplyMovementState(i);
                        }
                    }
                }
            }
            else
            {
                if (movementData.ToLower() == movementStates[curMoveState].name.ToLower())
                {
                    return;
                }
                for (int i = 0; i < movementStates.Length; i++)
                {
                    if (movementStates[i].name.ToLower() == movementData.ToLower() && MovementCheck(i, true))
                    {
                        ApplyMovementState(i);
                    }
                }
            }
        }

        public void ApplyMovementState(int movementType)
        {
            if (movementStates[curMoveState].midairOnFinish)
            {
                grounded = false;
            }

            if (movementType < movementStates.Length)
            {
                curMoveState = movementType;
                rb.gravityScale = movementStates[curMoveState].gravityScale;
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, movementStates[curMoveState].slidingAmmount);
                curMoveDirection= inputDirection;

                moveSpeed = new Vector2(movementStates[curMoveState].horizontalMoveSpeed,
                    movementStates[curMoveState].verticalMoveSpeed);
                slidingAmmount = movementStates[curMoveState].slidingAmmount;
                forceAirYSpeed = movementStates[curMoveState].forceFallVel;

                curMovementUsedTime = 0f;

                if (movementStates[curMoveState].animationState.Length > 0)
                {
                    SendMessage("ChangeAnimationState", movementStates[curMoveState].animationState);
                }
                if (movementStates[curMoveState].playOnEnterState.Length > 0)
                {
                    string[] splitSounds = movementStates[curMoveState].playOnEnterState.Split(',');
                    if (splitSounds.Length > 1)
                    {
                        Sound.SoundManager.instance.PlayAudio(splitSounds[Random.Range(0,splitSounds.Length)]);
                    }
                    else
                    {
                        Sound.SoundManager.instance.PlayAudio(movementStates[curMoveState].playOnEnterState);
                    }
                }
            }
        }

        public void MoveInHorizontalDirection(float dir)
        {
            curMoveDirection = new Vector2(dir, curMoveDirection.y);
            inputDirection = curMoveDirection;
        }

        public void MoveInHorizontalDirection(string dir)
        {
            float direction;
            if (float.TryParse(dir, out direction))
            {
                curMoveDirection = new Vector2(direction, curMoveDirection.y);
                inputDirection = curMoveDirection;
            }
        }

        public void Jump()
        {
            lastJumped = jumpCheckTimer;
            cancelledJump = false;
        }
        
        public void EndJump()
        {
            cancelledJump = true;
        }

        public void Run()
        {
            running = true;
        }

        public void EndRun()
        {
            running = false;
        }

        public void GroundPlayer()
        {
            grounded = true;
            curFallTime = 0;
            forceGrounded = true;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            bool inGround = false;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector2.Dot(Vector2.up, collision.contacts[i].normal) > groundedDotProduct)
                {
                    grounded = true;
                    return;
                }
            }
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector2.Dot(Vector2.up, collision.contacts[i].normal) <= groundedDotProduct)
                {
                    if ((!inGround && !grounded) && Mathf.Abs(rb.velocity.y) > 0.2f)
                    {
                        if (Vector2.Dot(Vector2.down, collision.contacts[i].normal) <= groundedDotProduct)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * movementStates[curMoveState].wallStickAmmount);

                            if (movementStates[curMoveState].canWallJump && lastJumped > 0)
                            {
                                if (movementStates[curMoveState].wallJumpState.Length > 0)
                                {
                                    ChangeMovementType(movementStates[curMoveState].wallJumpState + "|100");
                                }

                                curMoveDirection = new Vector2(movementStates[curMoveState].horizontalMoveSpeed * 0.1f * Mathf.Sign(collision.contacts[i].normal.x), movementStates[curMoveState].jumpPower);
                                rb.velocity = curMoveDirection;
                                jumpTime = movementStates[curMoveState].jumpTime * movementStates[curMoveState].airJumpPower;
                                canAirJump = false;
                                lastJumped = -1;

                            }
                        }
                    }

                    grounded = inGround;
                    if (forceGrounded)
                    {
                        grounded = true;
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (Vector2.Distance(collision.contacts[i].point, transform.position)<contactIgnoreDistance){
                    if (Vector2.Dot(Vector2.up, collision.contacts[i].normal) > groundedDotProduct)
                    {
                        grounded = true;
                    }
                }
            }
            grounded = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            grounded = false;
        }

        public void SendGrounded()
        {
            SendMessage("IsGrounded", GetGrounded());
        }

    }
}