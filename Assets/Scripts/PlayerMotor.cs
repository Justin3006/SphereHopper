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

    bool walk;

    float movLockTime;


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


    int[,] equippedAbilities = { { 0, 3 }, { 3, 3 }, { 2, 3 } };
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle direct inputs.
        if (movLockTime <= 0)
        {
            // move position
            rb.MovePosition(transform.position + transform.rotation * (Time.fixedDeltaTime * movSpeed * movDir));

            // rotate body
            // TODO: Change Rotate to Lerp for smoother controlls
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

            // jump
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
            rb.MovePosition(transform.position + transform.rotation * (Time.fixedDeltaTime * dashSpeed * movDir));
            dashDuration -= Time.fixedDeltaTime;
        }


        //Handle execution of combat actions.
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, ATTACK_RANGE * 2.5f);

        //distance indicator
        float distancePercent = 0;
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Vulnerable>() != null)
        {
            distancePercent = hit.distance / ATTACK_RANGE;
        }
        distanceIndicator.GetComponent<RectTransform>().localScale = new Vector3(distancePercent, distancePercent, 1);

        //attack
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
                        target.Hit();
                    }
                }
                swordIndicator.GetComponent<RectTransform>().localPosition = new Vector3(-500, -200, 0);
            }
        }

        //parry
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


        // Handle cooldowns.
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
        this.movDir = movDir;
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
        if (Physics.Raycast(transform.position + 0.01f * transform.up, -transform.up, 0.1f))
            jump = true;
    }

    public void Dash() 
    {
        if (dashCD <= 0)
            dash = true;
    }

    public void Walk() 
    {
        if (!walk)
            movSpeed /= 1.9f;
        else
            movSpeed *= 1.9f;
        walk = !walk;
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

    public void Interact() 
    {
        //TODO: handle in FixedUpdate?
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, ATTACK_RANGE))
        {
            Transporter t = hit.collider.gameObject.GetComponent<Transporter>();
            if (t != null)
                t.Interact();
        }
    }

    private void IntToAbility(int ability) 
    {
        switch (ability) 
        {
            case 0: PlaceHolderAbility1(); break;
            case 1: Debug.Log("Ability 1"); break;
            case 2: PlaceHolderAbility2(); break;
            case 3: PlaceHolderAbility3(); break;
        }
    }

    public void ActivateAbility()
    {
        if (equippedAbilities[selectedAbility, 1] > 0)
        {
            equippedAbilities[selectedAbility, 1]--;
            IntToAbility(equippedAbilities[selectedAbility, 0]);
            abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility, 1] + " uses left\n Ability " + equippedAbilities[selectedAbility, 0];
        }
    }

    public void SwitchAbility(int dir) 
    {
        selectedAbility = (selectedAbility + dir) % equippedAbilities.GetLength(0);
        if (selectedAbility < 0)
            selectedAbility = equippedAbilities.GetLength(0) - 1;
        abilityText.GetComponent<TextMeshProUGUI>().text = equippedAbilities[selectedAbility, 1] + " uses left\n Ability " + equippedAbilities[selectedAbility, 0];
    }

    public void PlaceHolderAbility1() 
    {
        rb.AddForce(0,5000,0);
    }

    public void PlaceHolderAbility2()
    {
        transform.Rotate(0,180,0);
    }

    public void PlaceHolderAbility3()
    {
        rb.AddForce(100, 100, 100);
    }
}
