using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : Vulnerable
{
    Rigidbody rb;
    Camera cam;

    [SerializeField]
    float movSpeed = 7;
    Vector3 movDir;

    [SerializeField]
    float rotSpeed = 360;
    float rotDir;

    [SerializeField]
    float camRotSpeed = 360;
    float camRotDir;

    bool grounded;
    [SerializeField]
    float untilUngroundedMax = 0.25f;
    float untilUngrounded;

    [SerializeField]
    float jumpPower = 7.5f;
    bool jump;

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
    float attackCDMax = 0.4f;
    [SerializeField]
    float attackDurationMax = 0.1f;
    float attackDuration;
    [SerializeField]
    float parryCDMax = 0.5f;
    [SerializeField]
    float parryDurationMax = 0.25f;
    float parryDuration;
    float swordCD;

    const float ATTACK_RANGE = 3;

    [SerializeField]
    float lockOnRange = 15;
    //TODO: change "Vulnerable" to something more suitable
    Vulnerable lockOnTarget;


    IAbility[] equippedAbilities = new IAbility[3];
    int selectedAbility;
    

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        hp = maxHp;
        equippedAbilities[0] = gameObject.AddComponent<AbilityDischarge>();
        equippedAbilities[1] = gameObject.AddComponent<AbilityPlaceholder>();
        equippedAbilities[2] = gameObject.AddComponent<AbilityPlaceholder>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle direct inputs.
        if (movLockTime <= 0)
        {
            // move position
            float dist = Time.fixedDeltaTime * movSpeed;
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
            // TODO: Change Rotate to Lerp for smoother controlls
            if (lockOnTarget == null)
            {
                transform.Rotate(0, Time.fixedDeltaTime * rotSpeed * rotDir, 0);


                // rotate cam 
                float camRotChange = -Time.fixedDeltaTime * camRotSpeed * camRotDir;
                float newCamRot = camRotChange + cam.transform.rotation.eulerAngles.x;

                if (newCamRot < 270 && newCamRot >= 180)
                    cam.transform.LookAt(cam.transform.position + transform.up, -transform.forward);
                else if (newCamRot > 90 && newCamRot < 180)
                    cam.transform.LookAt(cam.transform.position - transform.up, transform.forward);
                else
                    //TODO: Change Rotate to Lerp for smoother controlls
                    cam.transform.Rotate(camRotChange, 0, 0);
            }
            

            // jump
            if (Physics.Raycast(transform.position + 0.01f * transform.up, -transform.up, 0.1f))
                grounded = true;
            else if(untilUngrounded <= 0)
                untilUngrounded = untilUngroundedMax;

            if (jump)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
            }

            // dash
            if (dash)
            {
                dashDuration = dashDurationMax;
                movLockTime = dashDurationMax;
                dashCD = dashCDMax;
                dashIndicator.SetActive(false);
            }
        }

        // Handle continuation of dash.
        if (dashDuration > 0)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * dashSpeed * movDir);
            dashDuration -= Time.fixedDeltaTime;
        }


        // Handle execution of combat actions.
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, ATTACK_RANGE * 2.5f);

        // distance indicator
        float distancePercent = 0;
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Vulnerable>() != null)
        {
            distancePercent = hit.distance / ATTACK_RANGE;
        }
        distanceIndicator.GetComponent<RectTransform>().localScale = new Vector3(distancePercent, distancePercent, 1);

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
                        //TODO: Parametrize
                        target.Hit(transform.position, 10, 0.025f);
                    }
                }
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, -200, 0);
            }
        }

        // parry
        if (parryDuration > 0)
        {
            parryDuration -= Time.deltaTime;
            if (parryDuration <= 0)
            {
                shielded = false;
                if (shieldedAttacks >= 1) 
                {
                    swordCD /= 2;
                }
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, -200, 0);
            }
        }

        // Handle Lock On
        // TODO: Fix keeping the correct distance to the locked on target when moving sideways as well as compatibility with dash (possibly the same thing)
        if (lockOnTarget != null)
        {
            float prev_y = cam.transform.eulerAngles.y;
            cam.transform.LookAt(lockOnTarget.gameObject.GetComponent<Collider>().ClosestPoint(cam.transform.position));
            transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            movDir = Quaternion.Euler(0, cam.transform.eulerAngles.y - prev_y, 0) * movDir;
        }


        // Handle cooldowns.
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

        // Reset notifications.
        jump = false;
        dash = false;
    }

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
        if (grounded)
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

    public void Attack()
    {
        if (swordCD <= 0)
        {
            swordCD = attackCDMax;
            attackDuration = attackDurationMax;
            swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(500, 200, 0);
        }
    }

    public void Parry()
    {
        if (swordCD <= 0)
        {
            shielded = true;
            swordCD = parryCDMax;
            parryDuration = parryDurationMax;
            swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, 200, 0);
        }
    }

    public void LockOn() 
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lockOnRange))
        {
            Vulnerable newTarget = hit.collider.gameObject.GetComponent<Vulnerable>();
            if(newTarget != lockOnTarget)
                lockOnTarget = newTarget;
            else
                lockOnTarget = null;
        }
    }

    public void Interact() 
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, ATTACK_RANGE))
        {
            Transporter t = hit.collider.gameObject.GetComponent<Transporter>();
            if (t != null)
                t.Interact();
        }
    }

    public void ActivateAbility()
    {
        equippedAbilities[selectedAbility].Use();
        abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility].GetUsesRemaining() + " uses left\n Ability " + selectedAbility;
    }

    public void SwitchAbility(int dir)
    {
        selectedAbility = (selectedAbility + dir) % equippedAbilities.GetLength(0);
        if (selectedAbility < 0)
            selectedAbility = equippedAbilities.GetLength(0) - 1;
        abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility].GetUsesRemaining() + " uses left\n Ability " + selectedAbility;
    }
}
