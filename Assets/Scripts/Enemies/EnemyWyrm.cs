using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWyrm : MonoBehaviour
{
    [SerializeField]
    GameObject head;
    [SerializeField]
    GameObject[] tails;

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
    [SerializeField]
    GameObject attackIndicator;
    [SerializeField]
    private float attackIndicatorTime = 0.5f;

    WyrmState currentState;

    [SerializeField]
    private GameObject rock;
    [SerializeField]
    private int numberOfRocks = 5;
    private GameObject[] rocks;
    [SerializeField]
    private float minDistBetweenRocks = 15;
    [SerializeField]
    private float maxDistanceBetweenRocks = 25;

    [SerializeField]
    private GameObject sandStorm;
    [SerializeField]
    private float sandStormTime = 12f;

    [SerializeField]
    private float probabilitySandstorm = 5;
    [SerializeField]
    private float probabilityProjectile = 50;
    [SerializeField]
    private float probabilityAttack = 35;
    [SerializeField]
    private float probabilityHide = 10;

    private WyrmState[] actionArray = new WyrmState[100];

    //TODO: implement attack, that keeps the player away from rocks
    //TODO. implement warnings before attacks

    // Start is called before the first frame update
    void Start()
    {
        shufflePositions();

        stateChangeCDRemaining = riseCDMax;
        currentState = WyrmState.OPEN;

        rocks = new GameObject[numberOfRocks];
        for (int i = 0; i < numberOfRocks; i++) 
        {
            GameObject newRock = Instantiate(rock);
            bool fits;
            do
            {
                fits = true;
                newRock.transform.position += maxDistanceBetweenRocks * new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
                for (int j = 0; j < i; j++)
                {
                    float dist = (rocks[j].transform.position - newRock.transform.position).magnitude;
                    if (dist < minDistBetweenRocks)
                    {
                        fits = false;
                    }
                }
            }
            while (fits == false);
            rocks[i] = newRock;
        }

        int pointer = 0;
        for (int i = 0; i < probabilityHide; i++, pointer++)
            actionArray[pointer] = WyrmState.HIDING;
        for (int i = 0; i < probabilityProjectile; i++, pointer++)
            actionArray[pointer] = WyrmState.OPEN;
        for (int i = 0; i < probabilityAttack; i++, pointer++)
            actionArray[pointer] = WyrmState.UNDERGROUNDATTACK;
        for (int i = 0; i < probabilitySandstorm; i++, pointer++)
            actionArray[pointer] = WyrmState.SANDSTORM;
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
                currentState = actionArray[Random.Range(0, 100)];
            }
            while (currentState.Equals(previousState));

            switch (currentState) 
            {
                case WyrmState.HIDING: 

                    stateChangeCDRemaining = hideCDMax;
                    head.SetActive(false);
                    foreach (GameObject tail in tails)
                        tail.SetActive(false);
                    break;


                case WyrmState.OPEN: 

                    stateChangeCDRemaining = riseCDMax;
                    projectileCDRemaining = projectileCDMax;

                    int ori = Random.Range(0, tails.Length + 1);
                    if (ori < tails.Length)
                        projectileOrigin = tails[ori];
                    else
                        projectileOrigin = head;
                    projectilesToFire = projectilesInBarrage;

                    transform.position = PlayerManager.GetPosition() + 0.01f * Vector3.up;
                    shufflePositions();

                    RaycastHit hit;
                    
                    head.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    Physics.Raycast(head.transform.position, Vector3.up, out hit);
                    if (hit.collider == null)
                        Physics.Raycast(head.transform.position + Vector3.up, -Vector3.up, out hit);
                    head.transform.localPosition += (hit.point.y - head.transform.position.y) * Vector3.up;

                    foreach (GameObject tail in tails)
                    {
                        tail.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized + (hit.point.y - transform.position.y) * Vector3.up;
                        Physics.Raycast(tail.transform.position, Vector3.up, out hit);
                        if (hit.collider == null)
                            Physics.Raycast(tail.transform.position + Vector3.up, -Vector3.up, out hit);
                        tail.transform.localPosition += (hit.point.y - head.transform.position.y) * Vector3.up;
                    }

                    head.SetActive(true);
                    foreach (GameObject tail in tails)
                        tail.SetActive(true);

                    break;


                case WyrmState.UNDERGROUNDATTACK:
                    head.SetActive(false);
                    foreach (GameObject tail in tails)
                        tail.SetActive(false);
                    stateChangeCDRemaining = ugaCDMax;
                    attackCDRemaining = attackCDMax;
                    break;


                case WyrmState.SANDSTORM:
                    head.SetActive(false);
                    foreach (GameObject tail in tails)
                        tail.SetActive(false);
                    stateChangeCDRemaining = sandStormTime;
                    Destroy(Instantiate(sandStorm, transform.position, transform.rotation), sandStormTime);
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
                        Destroy(Instantiate(attackIndicator, transform.position, transform.rotation), attackIndicatorTime);
                    }
                }
            }
        }
    }

    private void shufflePositions() 
    {
        bool tooClose;
        do
        {
            tooClose = false;

            head.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            if (head.transform.localPosition.magnitude == 0)
            {
                tooClose = true;
                continue;
            }

            foreach (GameObject tail in tails)
            {
                tail.transform.localPosition = radius * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            }

            for (int i = 0; i < tails.Length; i++)
            {
                if (tails[i].transform.localPosition.magnitude == 0)
                {
                    tooClose = true;
                    break;
                }

                if ((tails[i].transform.localPosition - head.transform.localPosition).magnitude < minDistance)
                {
                    tooClose = true;
                    break;
                }

                for (int j = 0; j < i; j++)
                {
                    if ((tails[i].transform.localPosition - tails[j].transform.localPosition).magnitude < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }
            }
        }
        while (tooClose);
    }
}

public enum WyrmState 
{ 
    HIDING, OPEN, UNDERGROUNDATTACK, SANDSTORM
}
