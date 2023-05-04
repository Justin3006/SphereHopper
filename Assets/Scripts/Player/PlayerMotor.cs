using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : Vulnerable
{
    Rigidbody rb;
    Camera cam;

    //SECTION: Movement Attributes
    [SerializeField]
    float movSpeed = 7;
    Vector3 movDir;

    [SerializeField]
    float rotSpeed = 360;
    float rotDir;

    [SerializeField]
    float camRotSpeed = 360;
    float camRotDir;

    float generalSpeedModifier = 1;

    bool grounded;
    [SerializeField]
    float untilUngroundedMax = 0.25f;
    float untilUngrounded;

    [SerializeField]
    float jumpPower = 7.5f;
    bool jump;
    [SerializeField]
    float bonusJumpPower = 6;
    [SerializeField]
    int bonusJumpsMax = 2;
    int bonusJumpsRemaining;

    [SerializeField]
    float dashSpeed = 30;
    [SerializeField]
    float dashDurationMax = 0.035f;
    float dashDuration;
    [SerializeField]
    float dashCDMax = 0.6f;
    float dashCD;
    bool dash;

    bool walking;

    float movLockTime = 0;

    [SerializeField]
    float lockOnRange = 15;

    //TODO: change "Vulnerable" to something more suitable
    Vulnerable lockOnTarget;


    //SECTION: Melee Combat Attributes
    [SerializeField]
    float attackCDMax = 0.4f;
    [SerializeField]
    float attackDurationMax = 0.1f;
    float attackDuration;
    const float attackImpact = 10;
    float attackImpactModifier = 1;
    const float attackStun = 0.025f;
    float attackStunModifier = 1;

    [SerializeField]
    float parryCDMax = 0.5f;
    [SerializeField]
    float parryDurationMax = 0.25f;
    float parryDuration;
    float swordCD;

    const float ATTACK_RANGE = 3;

    IAbility[] equippedAbilities = new IAbility[3];
    int selectedAbility;
    

    //SECTION: UI Attributes
    [SerializeField]
    GameObject distanceIndicator;
    [SerializeField]
    GameObject dashIndicator;
    [SerializeField]
    GameObject swordIndicator;
    [SerializeField]
    GameObject hitIndicator;
    [SerializeField]
    GameObject abilityText;


    //SECTION: Initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        hp = maxHp;
        equippedAbilities[0] = gameObject.AddComponent<AbilityDischarge>();
        equippedAbilities[1] = gameObject.AddComponent<AbilityGrapplingHook>();
        equippedAbilities[2] = gameObject.AddComponent<AbilityGear>();
        abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility].GetName() + "\n" + equippedAbilities[selectedAbility].GetUsesMax() + " uses left";
        bonusJumpsRemaining = bonusJumpsMax;
    }

    //SECTION: Regular Updates
    void FixedUpdate()
    {
        //SUBSECTION: Physics
        if (movLockTime <= 0)
        {
            rb.velocity = rb.velocity.y * Vector3.up;
            rb.useGravity = true;
        }

        //SUBSECTION: Direct Inputs
        if (movLockTime <= 0)
        {
            // move position
            float dist = Time.fixedDeltaTime * movSpeed * generalSpeedModifier;
            Vector3 desPos = transform.position + dist * movDir;
            if (walking && !Physics.Raycast(desPos, -transform.up, dist))
            {
                Collider[] coll = Physics.OverlapSphere((desPos + transform.position) / 2, 0.05f + dist / 2);
                foreach (Collider c in coll) 
                {
                    if (c.gameObject.name != "player")
                    {
                        desPos = c.ClosestPoint(desPos);
                    }
                }
            }
            rb.MovePosition(desPos);

            // rotate body
            if (lockOnTarget == null)
            {
                float targetRotationY = transform.rotation.eulerAngles.y + Time.fixedDeltaTime * rotSpeed * rotDir;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotationY, 0), Time.fixedDeltaTime * rotSpeed);

                // rotate cam 
                float camRotChange = -Time.fixedDeltaTime * camRotSpeed * camRotDir;
                float newCamRot = camRotChange + cam.transform.rotation.eulerAngles.x;

                if (newCamRot < 270 && newCamRot >= 180)
                    cam.transform.LookAt(cam.transform.position + transform.up, -transform.forward);
                else if (newCamRot > 90 && newCamRot < 180)
                    cam.transform.LookAt(cam.transform.position - transform.up, transform.forward);
                else
                {
                    float targetCamRotationX = cam.transform.rotation.eulerAngles.x + camRotChange;
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(targetCamRotationX, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z), Time.fixedDeltaTime * camRotSpeed);
                }
            }

            // jump
            if (Physics.Raycast(transform.position + 0.01f * transform.up, -transform.up, 0.1f))
            {
                grounded = true;
                bonusJumpsRemaining = bonusJumpsMax;
            }
            else if (untilUngrounded <= 0)
                untilUngrounded = untilUngroundedMax;

            if (jump)
            {
                if (grounded)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
                }
                else if (bonusJumpsRemaining > 0)
                {
                    bonusJumpsRemaining--;
                    rb.velocity = new Vector3(rb.velocity.x, bonusJumpPower, rb.velocity.z);
                }
            }

            // dash
            if (dash)
            {
                dashDuration = dashDurationMax;
                movLockTime = dashDurationMax;
                dashCD = dashCDMax;
                dashIndicator.SetActive(false);
                cam.fieldOfView = 61;
            }
        }

        //SUBSECTION: Dash
        if (dashDuration > 0)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * dashSpeed * generalSpeedModifier * movDir);
            dashDuration -= Time.fixedDeltaTime;
            if (dashDuration <= 0)
            {
                cam.fieldOfView = 60;
            }
        }


        //SUBSECTION: Combat Actions
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, ATTACK_RANGE * 2.5f);

        // distance indicator
        float distancePercent = 0;
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Vulnerable>() != null)
        {
            distancePercent = hit.distance / ATTACK_RANGE;
        }
        distanceIndicator.GetComponent<RectTransform>().localScale = new Vector3(distancePercent, distancePercent, 1);

        //TODO: Fix keeping the correct distance to the locked-on target when moving sideways as well as compatibility with dash (possibly the same thing)
        if (lockOnTarget != null)
        {
            float prev_y = cam.transform.eulerAngles.y;
            cam.transform.LookAt(lockOnTarget.gameObject.GetComponent<Collider>().ClosestPoint(cam.transform.position));
            transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            movDir = Quaternion.Euler(0, cam.transform.eulerAngles.y - prev_y, 0) * movDir;
        }   
        
        // attack
        if (attackDuration > 0) 
        {
            attackDuration -= Time.deltaTime;
            if (attackDuration <= 0) 
            {
                if (hit.collider != null && hit.distance <= ATTACK_RANGE)
                {
                    Vulnerable target = hit.collider.gameObject.GetComponent<Vulnerable>();
                    if (target != null)
                    {
                        hitIndicator.SetActive(true);
                        target.Hit(transform.position, 1, 0, attackImpactModifier * attackImpact, attackStunModifier * attackStun);
                    }
                }
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, -200, 0);
            }
        }

        // parry
        if (parryDuration > 0)
        {
            parryDuration -= Time.deltaTime;

            // reset cooldowns, if you blocked someone (for counter attacks)
            if (shieldedAttacks == 1) 
            {
                swordCD = 0;
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(500, -200, 0);
            }
            // reset when parry is done
            if (parryDuration <= 0)
            {
                shielded = false;
                shieldedAttacks = 0;
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, -200, 0);
            }
        }


        //SUBSECTION: Lock On
        //TODO: Fix keeping the correct distance to the locked on target when moving sideways as well as compatibility with dash (possibly the same thing)
        if (lockOnTarget != null)
        {
            float prev_y = cam.transform.eulerAngles.y;
            cam.transform.LookAt(lockOnTarget.gameObject.GetComponent<Collider>().ClosestPoint(cam.transform.position));
            transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            movDir = Quaternion.Euler(0, cam.transform.eulerAngles.y - prev_y, 0) * movDir;
        }


        //SUBSECTION: Cooldowns
        if (untilUngrounded > 0) 
        {
            untilUngrounded -= Time.fixedDeltaTime;
            if (untilUngrounded <= 0)
                grounded = false;
        }

        if (dashCD > 0) 
        {
            dashCD -= Time.fixedDeltaTime;
            if (dashCD <= 0)
            {
                dashIndicator.SetActive(true);
            }
        }

        if (movLockTime > 0) 
        {
            movLockTime -= Time.fixedDeltaTime;
        }

        if (swordCD > 0)
        {
            swordCD -= Time.fixedDeltaTime;
            if (swordCD <= 0) 
            {
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(500, -200, 0);
                hitIndicator.SetActive(false);
            }
        }

        handleDmgIndicator();

        //SUBSECTION: Reset Notifications
        jump = false;
        dash = false;
    }


    //SECTION: Inputs
    //SUBSECTION: Movement
    public void Move(Vector3 movDir)
    {
        if (movLockTime <= 0)
            this.movDir = transform.rotation * movDir;
    }

    public void Rotate(float rotDir) 
    {
        this.rotDir = rotDir;
    }

    public void RotateCam(float camRotDir) 
    {
        this.camRotDir = camRotDir;
    }

    public void Jump() 
    {
        jump = true;
    }

    public void Dash() 
    {
        if (dashCD <= 0)
            dash = true;
    }

    public void Walk() 
    {
        if (!walking)
            movSpeed /= 1.9f;
        else
            movSpeed *= 1.9f;
        walking = !walking;
    }

    public void LockOn()
    {
        if (lockOnTarget != null)
        {
            lockOnTarget = null;
            return;
        }
        
        //TODO: Lock on to target, even if it was slightly missed

        RaycastHit hit;

        // Raycast from the camera's position, only in the horizontal direction.
        Vector3 horizontalDirection = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
                
        if (Physics.Raycast(cam.transform.position, horizontalDirection, out hit, lockOnRange))
        {
            Vulnerable newTarget = hit.collider.gameObject.GetComponent<Vulnerable>();

            if (newTarget != lockOnTarget)
            {
                lockOnTarget = newTarget;
            }
            
        }

        //TODO: Maybe just straight up rework the entire mechanic? Attach empty Objects to each enemy, where you lock the camera onto?
                
    }

    //SUBSECTION: Combat
    public void Attack()
    {
        foreach (IAbility a in equippedAbilities)
        {
            if (a.IsUsed())
                return;
        }

        if (swordCD <= 0)
        {
            swordCD = attackCDMax;
            attackDuration = attackDurationMax;
            swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(500, 200, 0);
        }
    }

    public void Parry()
    {
        foreach (IAbility a in equippedAbilities)
        {
            if (a.IsUsed())
                return;
        }

        if (swordCD <= 0)
        {
            shielded = true;
            swordCD = parryCDMax;
            parryDuration = parryDurationMax;
            swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, 200, 0);
        }
    }

    public void ActivateAbility()
    {
        foreach (IAbility a in equippedAbilities)
        {
            if (a.IsUsed())
                return;
        }

        if (swordCD <= 0)
        {
            equippedAbilities[selectedAbility].Use();
            abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility].GetName() + "\n" + equippedAbilities[selectedAbility].GetUsesRemaining() + " uses left";
        }
    }

    public void SwitchAbility(int dir)
    {
        selectedAbility = (selectedAbility + dir) % equippedAbilities.GetLength(0);
        if (selectedAbility < 0)
            selectedAbility = equippedAbilities.GetLength(0) - 1;
        abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility].GetName() + "\n" + equippedAbilities[selectedAbility].GetUsesRemaining() + " uses left";
    }

    //SUBSECTION: Misceallaneous
    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position + 0.5f * cam.transform.forward, cam.transform.forward, out hit, ATTACK_RANGE))
        {
            IInteractable t = hit.collider.gameObject.GetComponent<IInteractable>();
            if (t != null)
                t.Interact();
        }
    }


    //SECTION: Support Methods
    public void LockMovement(float time) 
    {
        movLockTime = time;
    }

    public void SetGeneralSpeedModifier(float modifier) 
    {
        generalSpeedModifier = modifier;
    }
    public void SetAttackImpactModifier(float modifier) 
    {
        attackImpactModifier = modifier;
    }
    public void SetAttackStunModifier(float modifier) 
    {
        attackStunModifier = modifier;
    }
}

