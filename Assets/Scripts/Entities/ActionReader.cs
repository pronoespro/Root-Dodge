using PronoesPro.Entity;
using PronoesPro.Entity.Tracking;
using PronoesPro.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PronoesPro.Scripting
{
    [System.Serializable]
    public class Condition
    {
        public string name;
        public bool forceCancel;
        public string conditionScript;
        public bool useTargetInstead;

    }

    public class ActionReader : MonoBehaviour
    {

        protected static Coroutine curRumble;

        protected Coroutine curMoveRoutine;
        protected Transform target;
        protected Transform selectedTransform;
        protected Gamepad gamepad;
        protected TrackEntity tracker;
        protected Rigidbody2D rb;
        protected bool needsResource;
        protected Vector2[] collisionPoints;
        protected Vector2 oldVelocity;

        private bool needResource,foundResource;

        protected virtual void Start()
        {
            gamepad = Gamepad.current;
            rb = GetComponent<Rigidbody2D>();
            tracker = GetComponent<TrackEntity>();
            collisionPoints = new Vector2[0];
        }

        public virtual bool DoEffect(string data)
        {
            if (rb == null){
                rb = GetComponent<Rigidbody2D>();
            }
            if (tracker == null){
                tracker = GetComponent<TrackEntity>();
            }
            string[] splitData = data.Split(';');
            for (int i = 0; i < splitData.Length; i++)
            {
                if (splitData[i].Length > 0 && splitData[i][0] == '<' && splitData[i][splitData[i].Length - 1] == '>')
                {
                    splitData[i] = splitData[i].Remove(0, 1);
                    splitData[i] = splitData[i].Remove(splitData[i].Length - 1);

                    string[] commandData = splitData[i].Split(':');
                    if (commandData.Length > 1)
                    {
                        if (!CustomCommand(commandData))
                        {
                            return false;
                        }
                    }

                }
            }
            return true;
        }

        public virtual bool CustomCommand(string[] commandData)
        {
            switch (commandData[0].ToLower())
            {
                case "recoil":
                    if (commandData.Length > 1)
                    {
                        float strength;
                        if (float.TryParse(commandData[1], out strength) && this.rb != null)
                        {
                            this.rb.velocity = SpecialTransform().rotation * Vector3.right * strength;
                        }
                    }
                    break;
                case "shoot":

                    float rot = SpecialTransform().rotation.eulerAngles.z;

                    if (commandData.Length > 2)
                    {
                        float deviation;
                        if (float.TryParse(commandData[2], out deviation))
                        {
                            rot += Random.Range(-deviation, deviation);
                        }
                    }

                    Projectiles.ProjectileManager.instance.CreateProjectile(commandData[1], SpecialTransform().position, rot, transform);

                    break;
                case "spawnheld":
                    Projectiles.ProjectileManager.instance.CreatePrefav(commandData[1]);
                    Projectiles.ProjectileManager.instance.ChangeLastPrefavPosition(SpecialTransform().position);
                    break;
                case "spawn":
                    Projectiles.ProjectileManager.instance.CreatePrefav(commandData[1]);
                    break;
                case "particles":
                    Particles.ParticleManager.instance.CreatePrefav(commandData[1]);
                    if (commandData.Length > 2)
                    {
                        switch (commandData[2])
                        {
                            case "special":
                                Particles.ParticleManager.instance.ChangeLastPrefavPosition(SpecialTransform().position);
                                break;
                            case "pos":
                                Particles.ParticleManager.instance.ChangeLastPrefavPosition(transform.position);
                                break;
                        }
                    }
                    Particles.ParticleManager.instance.PlayLastParticleUsed();
                    break;
                case "heal":
                    if (commandData.Length > 1)
                    {
                        if (commandData.Length > 2)
                        {
                            LevelManager.instance.SendMessageToObject(commandData[2], "AddToResource-" + commandData[1]);
                        }
                        else
                        {
                            SendMessage("AddToResource", commandData[1]);
                        }
                    }
                    break;
                case "use":
                    if (commandData.Length > 1)
                    {
                        needResource = true;
                        SendMessage("UseResource", commandData[1]);
                        if (needResource != false)
                        {
                            return false;
                        }
                        needResource = false;
                    }
                    break;
                case "need":
                    if (commandData.Length > 1)
                    {
                        needResource = true;
                        foundResource = false;
                        SendMessage("NeedResource", commandData[1]);
                        if (needResource != false || foundResource==false)
                        {
                            return false;
                        }
                        needResource = false;
                    }
                    break;
                case "lessthan":
                    if (commandData.Length > 1)
                    {
                        needResource = true;
                        SendMessage("IfLessThanResource", commandData[1]);
                        if (needResource != false)
                        {
                            return false;
                        }
                        needResource = false;
                    }
                    break;
                case "lost":
                    if (commandData.Length > 1)
                    {
                        needResource = true;
                        SendMessage("IfLostResource", commandData[1]);
                        if (needResource != false)
                        {
                            return false;
                        }
                        needResource = false;
                    }
                    break;
                case "teleport":
                    if (commandData.Length > 1)
                    {
                        SendMessage("TeleportToSpawn");
                    }
                    else
                    {
                        SendMessage("TeleportToSpawn");
                    }
                    break;
                case "sfx":
                    if (commandData[1].Contains(","))
                    {
                        string[] sfxData = commandData[1].Split(',');
                        Sound.SoundManager.instance.PlayAudio(sfxData[Random.Range(0, sfxData.Length)]);
                    }
                    else
                    {
                        Sound.SoundManager.instance.PlayAudio(commandData[1]);
                    }

                    break;
                case "music":
                    if (MusicManager.instance != null)
                    {
                        MusicManager.instance.ChangeTrack(commandData[1]);
                    }
                    break;
                case "move":

                    if (curMoveRoutine != null)
                    {
                        StopCoroutine(curMoveRoutine);
                        curMoveRoutine = null;
                    }

                    Vector2 desDir = new Vector2();
                    {
                        if (commandData.Length > 2)
                        {
                            if (commandData[1].ToLower().Contains("rb"))
                            {
                                desDir = ParseVector(commandData[2], (commandData.Length > 3) ? commandData[3] : "", rb.velocity.x, rb.velocity.y);
                            }
                            else
                            {
                                desDir = ParseVector(commandData[2], (commandData.Length > 3) ? commandData[3] : "", rb.velocity.x, rb.velocity.y);
                            }
                            if (commandData[1].ToLower().Contains("local"))
                            {
                                desDir = transform.rotation * desDir;
                            }
                        }

                        if (commandData[1].ToLower().Contains("sin"))
                        {
                            curMoveRoutine = StartCoroutine(MoveInSine(rb, desDir));
                        }
                        else
                        {
                            if (commandData[1].ToLower().Contains("rb"))
                            {
                                float timer;
                                if (commandData.Length > 4 && float.TryParse(commandData[4], out timer))
                                {
                                    curMoveRoutine = StartCoroutine(ChangeVelocity(desDir, this.rb, timer));
                                }
                                else
                                {
                                    rb.velocity = desDir;
                                }
                            }
                            else
                            {
                                transform.position += (Vector3)desDir;
                            }
                        }
                    }

                    break;
                case "move-to-target":
                case "move-target":
                case "move-targ":
                case "tmove":
                    if (curMoveRoutine != null)
                    {
                        StopCoroutine(curMoveRoutine);
                        curMoveRoutine = null;
                    }

                    desDir = new Vector2();
                    if (commandData.Length > 2)
                    {
                        if (commandData[1].ToLower().Contains("rb"))
                        {
                            desDir = ParseVector(commandData[2], (commandData.Length > 3) ? commandData[3] : "", rb.velocity.x, rb.velocity.y);
                        }
                        else
                        {
                            desDir = ParseVector(commandData[2], (commandData.Length > 3) ? commandData[3] : "");
                        }
                        if (tracker.GetTarget() != null)
                        {
                            desDir.x *= (tracker.GetTarget().transform.position.x < transform.position.x) ? -1 : 1;
                            if (commandData[1].ToLower().Contains("lookat"))
                            {
                                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(desDir.x), transform.localScale.y, transform.localScale.z);
                            }
                        }
                        else
                        {
                            if (commandData[1].ToLower().Contains("local"))
                            {
                                desDir = transform.rotation * desDir;
                                desDir = new Vector2(desDir.x * transform.lossyScale.x, desDir.y * transform.lossyScale.y);
                            }
                        }
                    }

                    if (commandData[1].ToLower().Contains("sin"))
                    {
                        curMoveRoutine = StartCoroutine(MoveInSine(rb, desDir));
                    }
                    else
                    {
                        if (commandData[1].ToLower().Contains("rb"))
                        {
                            float timer;
                            if (commandData.Length > 4 && float.TryParse(commandData[4], out timer))
                            {
                                curMoveRoutine = StartCoroutine(ChangeVelocity(desDir, rb, timer));
                            }
                            else
                            {
                                rb.velocity = desDir;
                            }
                        }
                        else
                        {
                            transform.position += (Vector3)desDir;
                        }
                    }


                    break;
                case "state":
                    SendMessage("ChangeMovementType", commandData[1]);
                    break;
                case "flip-direction":
                    SendMessage("FlipMoveDirection");
                    break;
                case "anim-variable":
                case "anim-var":
                    SendMessage("SetAnimatorVariable", commandData[1]);
                    break;
                case "anim-state":
                    SendMessage("ChangeAnimationState", commandData[1]);
                    break;
                case "activate":
                    string objName = commandData[1].StartsWith("d_") ? commandData[1].Replace("d_", "") : commandData[1];
                    if (objName == "")
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        if (LevelManager.instance != null && LevelManager.instance.ActivateImportantObject(objName, !commandData[1].StartsWith("d_")))
                        {
                            //successful

                        }
                    }
                    break;
                case "send-message":
                    if (LevelManager.instance != null && commandData.Length > 2)
                    {
                        LevelManager.instance.SendMessageToObject(commandData[1], commandData[2]);
                    }
                    break;
                case "scene":
                case "loadscene":
                case "changescene":
                    if (FadeManager.instance != null)
                    {
                        FadeManager.instance.LoadLevel(commandData[1]);
                    }
                    break;
                case "losecontrol":

                    if (commandData.Length > 2 && commandData[2] != "")
                    {
                        if (commandData[1].Length == 0)
                        {
                            LevelManager.instance.SendMessageToObject(commandData[2], "DeactivateInputs");
                        }
                        else
                        {
                            LevelManager.instance.SendMessageToObject(commandData[2], "DeactivateInputsFor-" + commandData[1]);
                        }
                    }
                    else
                    {

                        if (commandData[1].Length == 0)
                        {
                            SendMessage("DeactivateInputs");
                        }
                        else
                        {
                            float timer;
                            if (float.TryParse(commandData[1], out timer))
                            {
                                SendMessage("DeactivateInputsFor", timer);
                            }
                            else
                            {
                                SendMessage("DeactivateInputs");
                            }
                        }
                    }
                    break;
                case "regaincontrol":
                    SendMessage("ActivateInputs");
                    break;
                case "log":
                    Debug.Log(commandData[1]);
                    break;
                case "timescale":
                    if (commandData[1] != "")
                    {
                        float result;
                        if (float.TryParse(commandData[1], out result))
                        {
                            Time.timeScale = result;
                        }
                    }
                    break;
                case "global-var":
                    if (commandData.Length > 2 && GameManager.instance != null)
                    {
                        float result;
                        if (float.TryParse(commandData[2], out result))
                        {
                            GameManager.instance.SaveVariable(commandData[1], result);
                        }
                        else
                        {
                            GameManager.instance.SaveVariable(commandData[1], commandData[2]);
                        }
                        Debug.Log(commandData[1] + " is now " + commandData[2]);
                    }
                    else
                    {
                        Debug.Log("failed to change data");
                    }
                    break;
                case "camstate":
                case "cam":
                case "cam-state":
                    if (commandData.Length > 1)
                    {
                        int result;
                        if (int.TryParse(commandData[1], out result))
                        {
                            Cameras.CameraStateManager.instance.ChangeCameraState(result);
                        }
                    }
                    break;
                case "rumble":
                    if (curRumble != null)
                    {
                        StopCoroutine(curRumble);
                        curRumble = null;
                    }
                    if (gamepad == null)
                    {
                        gamepad = Gamepad.current;
                    }
                    if (gamepad != null)
                    {
                        float time = -1;
                        if (commandData.Length > 3 && !float.TryParse(commandData[3], out time))
                        {
                            time = -1;
                        }
                        float minSpeed;
                        if (float.TryParse(commandData[1], out minSpeed))
                        {
                            float maxSpeed;
                            if (commandData.Length > 2 && float.TryParse(commandData[2], out maxSpeed))
                            {
                                Debug.Log("Rumble of " + minSpeed.ToString() + "/" + maxSpeed.ToString());
                                curRumble = StartCoroutine(SetRumble(minSpeed, maxSpeed, time));
                            }
                            else
                            {
                                Debug.Log("Rumble of " + minSpeed.ToString());
                                curRumble = StartCoroutine(SetRumble(minSpeed, minSpeed, time));
                            }
                        }
                        else
                        {
                            gamepad.SetMotorSpeeds(0, 0);
                        }
                    }
                    else
                    {
                        Debug.Log("No controller connected");
                    }
                    break;
            }
            return true;
        }

        public virtual void Update()
        {
            if (tracker != null)
            {
                target = tracker.GetTarget();
            }
        }

        public virtual bool ConditionCheck(string condition, bool useTarget)
        {
            if (condition.StartsWith("<") && condition.EndsWith(">"))
            {
                string[] splitCondition = condition.Split(':');
                splitCondition[0] = splitCondition[0].Remove(0, 1);
                if (splitCondition[splitCondition.Length - 1].Length > 0)
                {
                    splitCondition[splitCondition.Length - 1] = splitCondition[splitCondition.Length - 1].Remove(splitCondition[splitCondition.Length - 1].Length - 1, 1);
                }

                switch (splitCondition[0].ToLower())
                {
                    default:
                        return true;
                    case "target":
                        if (splitCondition.Length > 1)
                        {
                            if (splitCondition[1].ToLower().Contains("null"))
                            {
                                return target == null;
                            }
                            else
                            {
                                return target != null;
                            }
                        }
                        else
                        {
                            return target != null;
                        }
                    case "distance":
                        if (target == null)
                        {
                            return false;
                        }
                        else
                        {
                            float dist;
                            bool desResult = true;
                            if (splitCondition.Length > 2 && splitCondition[2].ToLower().Contains("null"))
                            {
                                desResult = false;
                            }

                            if (splitCondition.Length > 1 && float.TryParse(splitCondition[1], out dist))
                            {
                                if (Vector2.Distance(transform.position, target.position) < dist)
                                {
                                    return desResult;
                                }
                                else
                                {
                                    return !desResult;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    case "resource":
                        if (splitCondition.Length > 1)
                        {
                            if (useTarget)
                            {
                                if (target != null)
                                {
                                    foreach (Resource r in target.GetComponents<Resource>())
                                    {
                                        if (r.NeedResource(splitCondition[1]))
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    needsResource = false;
                                    foundResource = false;

                                    SendMessage("NeedResource", splitCondition[1]);

                                    if (!needsResource && foundResource)
                                    {
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                needsResource = false;
                                foundResource = false;
                                SendMessage("NeedResource", splitCondition[1]);

                                if (!needsResource && foundResource)
                                {
                                    return true;
                                }
                            }
                        }
                        return false;
                    case "noresource":
                        if (splitCondition.Length > 1)
                        {
                            if (useTarget)
                            {
                                if (target != null)
                                {
                                    foreach (Resource r in target.GetComponents<Resource>())
                                    {
                                        if (r.IfLessThanResource(splitCondition[1]))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                needsResource = false;
                                SendMessage("NeedResource", splitCondition[1]);

                                if (needsResource)
                                {
                                    return true;
                                }
                            }
                        }
                        return false;
                    case "global-var":
                        if (GameManager.instance != null)
                        {
                            if (splitCondition.Length > 1)
                            {
                                string[] conditionParameters = splitCondition[1].Split(',');
                                bool checkForLower = conditionParameters.Length > 3 && conditionParameters[3] == "less";

                                Debug.Log("Checking for " + (checkForLower ? "lower" : "higher") + " numbers");

                                if (conditionParameters.Length > 1)
                                {

                                    float valueToCheck;
                                    if (!float.TryParse(conditionParameters[1], out valueToCheck))
                                    {
                                        valueToCheck = 0;
                                        Debug.Log("Checking for value: " + valueToCheck);
                                    }

                                    if (conditionParameters.Length > 2)
                                    {
                                        float defValue;
                                        if (float.TryParse(conditionParameters[2], out defValue))
                                        {
                                            if (GameManager.instance.AddOrGetVariable(conditionParameters[0], defValue) >= valueToCheck)
                                            {
                                                Debug.Log(conditionParameters[0] + " is higher than " + valueToCheck);
                                                return (!checkForLower) ? false : true;
                                            }
                                            else
                                            {
                                                Debug.Log("numer lower than " + valueToCheck);
                                                return (!checkForLower) ? true : false;
                                            }
                                        }
                                        else
                                        {
                                            if (GameManager.instance.AddOrGetVariable(conditionParameters[0], defValue) >= valueToCheck)
                                            {
                                                Debug.Log(conditionParameters[0] + " is " + ((checkForLower) ? "not " : "") + "lower than " + valueToCheck);
                                                return !checkForLower;
                                            }
                                            else
                                            {
                                                return !checkForLower;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (GameManager.instance.AddOrGetVariable(conditionParameters[0], 0) >= valueToCheck)
                                        {
                                            Debug.Log(conditionParameters[0] + " is " + ((checkForLower) ? "not " : "") + "lower than " + valueToCheck);
                                            return !checkForLower;
                                        }
                                        else
                                        {
                                            return !checkForLower;
                                        }
                                    }
                                }
                                else
                                {
                                    if (GameManager.instance.AddOrGetVariable(splitCondition[1], 0) >= 0)
                                    {
                                        return !checkForLower;
                                    }
                                }
                                return checkForLower;
                            }
                        }
                        return false;
                    case "collision":
                        if (splitCondition.Length > 1)
                        {
                            Vector2 dir = Vector2.zero;

                            if (splitCondition[1] == "dir")
                            {
                                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                                if (rb != null)
                                {
                                    dir = -oldVelocity.normalized;

                                    float minCheckVel;
                                    
                                    if(splitCondition.Length==1 || !float.TryParse(splitCondition[2],out minCheckVel)){
                                        minCheckVel = -1;
                                    }

                                    if (oldVelocity.magnitude < minCheckVel)
                                    {
                                        oldVelocity = rb.velocity;
                                        return true;
                                    }

                                }
                            }
                            else
                            {
                                string[] splitVector = splitCondition[1].Split(',');
                                if (splitVector.Length > 1)
                                {
                                    dir = ParseVector(splitVector[0], splitVector[1]);
                                }
                                else
                                {
                                    dir = ParseVector(splitVector[0]);
                                }
                            }
                            float threshold = -1, result;
                            if (splitCondition.Length > 2 && float.TryParse(splitCondition[2], out result))
                            {
                                threshold = result;
                            }
                            if (collisionPoints.Length > 0)
                            {
                                foreach (Vector2 point in collisionPoints)
                                {
                                    if (threshold > 0)
                                    {
                                        if (Vector2.Dot(point, dir.normalized) > threshold)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (point == dir)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (collisionPoints.Length > 0)
                            {
                                return false;
                            }
                        }
                        return true;
                    case "velocity":
                    case "vel":
                        if (splitCondition.Length > 1)
                        {
                            Rigidbody2D _rb = (useTarget ? target.GetComponent<Rigidbody2D>() : rb);

                            if (_rb == null)
                            {
                                Debug.Log("No rb");
                                return false;
                            }

                            float neededSpeed;
                            if (!float.TryParse(splitCondition[1],out neededSpeed)){
                                neededSpeed = 0;
                            }

                            if (neededSpeed < 0)
                            {
                                Debug.Log("needed speed less than 0");
                                return false;
                            }

                            float checkedResource =Mathf.Abs(_rb.velocity.magnitude);
                            if (splitCondition.Length > 2)
                            {

                                if (splitCondition[2].ToLower().Contains("checkx"))
                                {
                                    checkedResource = Mathf.Abs(_rb.velocity.x);
                                }else if (splitCondition[2].ToLower().Contains("checky"))
                                {
                                    checkedResource = Mathf.Abs(_rb.velocity.y);
                                }

                                if (splitCondition[2].ToLower().Contains("less"))
                                {
                                    return checkedResource <= neededSpeed;
                                }
                            }
                            return checkedResource >= neededSpeed;

                        }
                        Debug.Log("split conditions too low");
                        return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            List<Vector2> newCollisions = new List<Vector2>();
            for (int i = 0; i < collision.contactCount; i++)
            {
                newCollisions.Add(collision.GetContact(i).normal);
            }
            collisionPoints = newCollisions.ToArray();
        }

        public IEnumerator SetRumble(float speedX = 0, float speedY = 0, float time = -1)
        {
            float timer = 0;
            while (time < 0 || timer < time)
            {
                gamepad.SetMotorSpeeds(speedX, speedY);
                timer += Time.deltaTime;
                yield return null;
            }
            gamepad.SetMotorSpeeds(0, 0);
        }

        public IEnumerator MoveInSine(Rigidbody2D rb, Vector2 strength)
        {
            while (true)
            {
                if (rb != null)
                {
                    rb.velocity = new Vector2(Mathf.Cos(Time.time * strength.x), Mathf.Sin(Time.time * strength.y));
                }
                else
                {
                    transform.position += new Vector3(Mathf.Cos(Time.time * strength.x), Mathf.Sin(Time.time * strength.y));
                }


                yield return null;
            }
        }

        public IEnumerator ChangeVelocity(Vector2 newVel, Rigidbody2D rb, float lerpTime)
        {
            Vector2 oldVel = rb.velocity;
            float timer = lerpTime;
            while (timer > 0)
            {

                rb.velocity = Vector2.Lerp(oldVel, newVel, 1f - (timer / lerpTime));

                timer -= Time.deltaTime;
                yield return null;
            }
            rb.velocity = newVel;
        }

        public static Vector2 ParseVector(string x = "", string y = "", float? defX = null, float? defy = null)
        {
            Vector2 output = new Vector2();
            float parsedValue;
            if (float.TryParse(x, out parsedValue))
            {
                output.x = parsedValue;
            }
            else if (defX != null)
            {
                output.x = defX.Value;
            }
            if (float.TryParse(y, out parsedValue))
            {
                output.y = parsedValue;
            }
            else if (defy != null)
            {
                output.y = defy.Value;
            }
            return output;
        }

        public void SelectTransform(Transform t)
        {
            selectedTransform = t;
        }

        public virtual Transform SpecialTransform()
        {
            return (selectedTransform != null ? selectedTransform : transform);
        }

        public void HasResource()
        {
            needResource = false;
        }

        public void FoundResource()
        {
            foundResource = true;
        }
    }
}