using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemySwordMotor : Vulnerable
{
    Rigidbody rb;

    [SerializeField]
    float movSpeed = 5;
    [SerializeField]
    float walkSpeed = 3;

    [SerializeField]
    float rotSpeed = 360;

    [SerializeField]
    float movementChangeCDMax = 0.1f;
    float movementChangeCD;
    [SerializeField]
    float movementChangeProbabilityZ = 0.3f;
    [SerializeField]
    int movementZDurationMax = 1;
    int movementZDuration;
    [SerializeField]
    bool movementZContinue = true;
    [SerializeField]
    float movementChangeProbabilityX = 0.1f;
    [SerializeField]
    int movementXDurationMax = 5;
    int movementXDuration;
    [SerializeField]
    bool movementXContinue = false;

    float movLockTime;

    Vector3 displacementDir;
    float dicplacementTime;

    Vector3 lastMovDir;

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

    [SerializeField]
    float attackProbability = 0.4f;
    [SerializeField]
    float parryProbability = 0.25f;

    const float ATTACK_RANGE = 3;
    const float ACTIVE_RANGE = 7;
    const float VISION_RANGE = 15;

    [SerializeField]
    GameObject weaponGFX;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastMovDir = new Vector3(0, 0, 0);
        hp = maxHp;
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Set up variables
        Vector3 playerPosition = PlayerManager.GetTransform().position;
        Vector3 toPlayer = playerPosition - this.transform.position;
        Vector3 movDir = Quaternion.Inverse(transform.rotation) * toPlayer;
        float dist = movDir.magnitude;
        float speed = movSpeed;

        // How to move when player within radius
        if (toPlayer.magnitude <= VISION_RANGE)
        {
            transform.LookAt(playerPosition);

            // Calculate fitting movement
            if (toPlayer.magnitude <= ACTIVE_RANGE)
            {
                speed = walkSpeed;
                movDir = lastMovDir;

                if (movementChangeCD <= 0)
                {
                    int movDirX;
                    int movDirZ;

                    // Calculate fitting movement in z direction
                    float rand = Random.Range(0f, 1f);
                    if (movementZDuration <= 0)
                    {
                        if (rand < movementChangeProbabilityZ)
                        {
                            // Try moving around attack range
                            float rand2 = dist / ATTACK_RANGE - 1 + Random.Range(-1f, 1f);
                            if (Mathf.Abs(rand2) <= 0.33f)
                                movDirZ = 0;
                            else
                                movDirZ = (int)Mathf.Sign(rand2);
                            movementZDuration = movementZDurationMax;
                        }
                        else
                        {
                            if (movementZContinue)
                                movDirZ = (int)lastMovDir.z;
                            else
                                movDirZ = 0;
                        }
                    }
                    else 
                    {
                        movDirZ = (int)lastMovDir.z;
                        movementZDuration--;
                    }

                    // Calculate fitting movement in x direction
                    if (movementXDuration <= 0)
                    {
                        rand = Random.Range(0f, 1f);
                        if (rand < movementChangeProbabilityX)
                        {
                            movDirX = Random.Range(-1, 2);
                            movementXDuration = movementXDurationMax;
                        }
                        else
                        {
                            if (movementXContinue)
                                movDirX = (int)lastMovDir.x;
                            else
                                movDirX = 0;
                        }
                    }
                    else
                    {
                        movDirX = (int)lastMovDir.x;
                        movementXDuration--;
                    }

                    // Save result
                    movDir = new Vector3(movDirX, 0, movDirZ);
                    lastMovDir = movDir;
                    movementChangeCD = movementChangeCDMax;
                }
            }

            // Execute movement
            float diff = (playerPosition - transform.position + speed * Time.fixedDeltaTime * (transform.rotation * movDir.normalized)).magnitude;
            if (diff < ATTACK_RANGE / 1.25f && movDir.z > 0 || diff > ATTACK_RANGE * 2 && movDir.z < 0)
                movDir.z = 0;
            rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * (transform.rotation * movDir.normalized));
        }


        // Calculate fitting combat action
        if (dist <= ATTACK_RANGE * 1.1f) 
        {
            if (swordCD <= 0)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < attackProbability)
                {
                    swordCD = attackCDMax;
                    attackDuration = attackDurationMax;
                    weaponGFX.transform.localEulerAngles = new Vector3(-30, 0, 0);
                }
                else 
                {
                    rand = Random.Range(0f, 1f);
                    if (rand < parryProbability) 
                    {
                        shielded = true;
                        swordCD = parryCDMax;
                        parryDuration = parryDurationMax;
                        weaponGFX.transform.localEulerAngles = new Vector3(45, -45, 0);
                    }
                }
            }
        }

        // Attack
        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
            if (attackDuration <= 0)
            {
                if (dist <= ATTACK_RANGE)
                {
                    PlayerManager.GetMotor().Hit(transform.position, 1, 0.1f);
                }
                weaponGFX.transform.localEulerAngles = new Vector3(75, 0, 0);
            }
        }

        // Parry
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
                weaponGFX.transform.localEulerAngles = new Vector3(45, 75, 0);
            }
        }


        // Handle Cooldowns
        if (movementChangeCD > 0)
        {
            movementChangeCD -= Time.fixedDeltaTime;
        }

        if (swordCD > 0)
        {
            swordCD -= Time.fixedDeltaTime;
            if(swordCD <= 0)
                weaponGFX.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override bool Hit(Vector3 origin, float impact, float stun)
    {
        if (shielded && Vector3.Dot(transform.forward, origin - transform.position) > 0)
        {
            shieldedAttacks++;
            return false;
        }

        hp--;
        if (hp <= 0)
            Destroy(gameObject);
        attackDuration = 0;
        weaponGFX.transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.Translate(0, 0, -impact);
            
        return true;
    }
}
