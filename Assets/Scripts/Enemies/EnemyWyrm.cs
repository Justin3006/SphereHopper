using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWyrm : MonoBehaviour
{
    //TODO: add more tails and save them in an array
    [SerializeField]
    GameObject head;
    [SerializeField]
    GameObject tail1;
    [SerializeField]
    GameObject tail2;
    [SerializeField]
    float radius = 7f;
    [SerializeField]
    float minDistance = 3f;

    [SerializeField]
    float hideCDMax = 3f;
    [SerializeField]
    float riseCDMax = 5f;
    [SerializeField]
    float ugaCDMax = 3f;
    float stateChangeCDRemaining;

    [SerializeField]
    float projectileCDMax = 3f;
    float projectileCDRemaining;
    GameObject projectileOrigin;
    [SerializeField]
    float projectileStartHeight = 2;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    private float spreadAngleMax = 2.5f;
    [SerializeField]
    private float barrageCDMax = 2;
    private float barrageCD;
    [SerializeField]
    private int projectilesInBarrage = 1;
    private int projectilesToFire;

    [SerializeField]
    private float attackCDMax = 2f;
    private float attackCDRemaining;
    [SerializeField]
    private float attackRange = 5f;

    WyrmState currentState;

    //TODO: spawn rocks on start to hide behind during storms
    //TODO: implement sandstorm attacks, where the player gets damaged, if he doesn't hide behind a rock
    //TODO. implement warnings before attacks

    // Start is called before the first frame update
    void Start()
    {
        do {
            head.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            tail1.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            tail2.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        }
        while ((head.transform.localPosition - tail1.transform.localPosition).magnitude < minDistance
            || (head.transform.localPosition - tail2.transform.localPosition).magnitude < minDistance 
            || (tail1.transform.localPosition - tail2.transform.localPosition).magnitude < minDistance 
            || tail1.transform.localPosition.magnitude == 0 
            || tail2.transform.localPosition.magnitude == 0 
            || head.transform.localPosition.magnitude == 0);

        stateChangeCDRemaining = riseCDMax;
        currentState = WyrmState.OPEN;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateChangeCDRemaining -= Time.fixedDeltaTime;

        if (stateChangeCDRemaining <= 0) 
        {
            WyrmState previousState = currentState;
            do
            {
                currentState = (WyrmState)Random.Range(0, 4);
            }
            while (currentState == previousState);

            switch (currentState) 
            {
                case WyrmState.HIDING: 

                    stateChangeCDRemaining = hideCDMax;
                    head.SetActive(false);
                    tail1.SetActive(false);
                    tail2.SetActive(false);
                    break;

                case WyrmState.OPEN: 

                    stateChangeCDRemaining = riseCDMax;
                    projectileCDRemaining = projectileCDMax;

                    int ori = Random.Range(0, 3);
                    switch (ori) 
                    {
                        case 0: projectileOrigin = head; break;
                        case 1: projectileOrigin = tail1; break;
                        case 2: projectileOrigin = tail2; break;
                    }
                    projectilesToFire = projectilesInBarrage;

                    transform.position = PlayerManager.GetPosition() + 0.01f * Vector3.up;

                    do
                    {
                        head.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                        tail1.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                        tail2.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    }
                    while ((head.transform.localPosition - tail1.transform.localPosition).magnitude < minDistance
                        || (head.transform.localPosition - tail2.transform.localPosition).magnitude < minDistance
                        || (tail1.transform.localPosition - tail2.transform.localPosition).magnitude < minDistance
                        || tail1.transform.localPosition.magnitude == 0
                        || tail2.transform.localPosition.magnitude == 0
                        || head.transform.localPosition.magnitude == 0);

                    RaycastHit hit;
                    
                    head.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    Physics.Raycast(head.transform.position, Vector3.up, out hit);
                    if (hit.collider == null)
                        Physics.Raycast(head.transform.position + Vector3.up, -Vector3.up, out hit);
                    head.transform.localPosition += (hit.point.y - head.transform.position.y) * Vector3.up;

                    tail1.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized + (hit.point.y - transform.position.y) * Vector3.up;
                    Physics.Raycast(tail1.transform.position, Vector3.up, out hit);
                    if (hit.collider == null)
                        Physics.Raycast(tail1.transform.position + Vector3.up, -Vector3.up, out hit);
                    tail1.transform.localPosition += (hit.point.y - head.transform.position.y) * Vector3.up;

                    tail2.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized + (hit.point.y - transform.position.y) * Vector3.up;
                    Physics.Raycast(tail2.transform.position, Vector3.up, out hit);
                    if (hit.collider == null)
                        Physics.Raycast(tail2.transform.position + Vector3.up, -Vector3.up, out hit);
                    tail2.transform.localPosition += (hit.point.y - head.transform.position.y) * Vector3.up;

                    head.SetActive(true);
                    tail1.SetActive(true);
                    tail2.SetActive(true);

                    break;

                case WyrmState.UNDERGROUNDATTACK:
                    head.SetActive(false);
                    tail1.SetActive(false);
                    tail2.SetActive(false);
                    stateChangeCDRemaining = ugaCDMax;
                    attackCDRemaining = attackCDMax;
                    break;

                case WyrmState.SANDSTORM:
                    Debug.Log("idk how to implement");
                    break;
            }
        }

        if (projectileOrigin != null) 
        {
            if (projectilesToFire > 0) {
                if (projectileCDRemaining <= 0)
                {
                    Vector3 dir = (PlayerManager.GetPosition() + Vector3.up - (projectileOrigin.transform.position + projectileStartHeight * Vector3.up)).normalized;
                    GameObject o = Instantiate(projectile, projectileOrigin.transform.position + projectileStartHeight * Vector3.up + dir, Quaternion.Euler(0, 0, 0));
                    o.transform.LookAt(PlayerManager.GetPosition() + Vector3.up);
                    float randX = Random.Range(-spreadAngleMax, spreadAngleMax);
                    float randY = Random.Range(-spreadAngleMax, spreadAngleMax);
                    o.transform.eulerAngles += new Vector3(randX, randY, 0);

                    projectilesToFire--;
                    projectileCDRemaining = projectileCDMax;
                    if (projectilesToFire <= 0)
                        barrageCD = barrageCDMax;
                }
                else
                {
                    projectileCDRemaining -= Time.fixedDeltaTime;
                }
            }
            else
                projectileOrigin = null;
        }

        if (attackCDRemaining > 0) 
        {
            attackCDRemaining -= Time.fixedDeltaTime;
            if (attackCDRemaining <= 0) 
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, attackRange);
                foreach (Collider coll in colls) 
                {
                    Vulnerable v = coll.gameObject.GetComponent<Vulnerable>();
                    if (v != null && coll.gameObject != this.gameObject) 
                    {
                        v.Hit(transform.position, 1, 0, 0, 0);
                    }
                }
            }
        }
    }
}

public enum WyrmState 
{ 
    HIDING, OPEN, UNDERGROUNDATTACK, SANDSTORM
}
