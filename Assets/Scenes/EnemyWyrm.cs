using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWyrm : MonoBehaviour
{
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
    float stateChangeCDRemaining;

    [SerializeField]
    float projectileCDMax = 3f;
    float projectileCDRemaining;
    GameObject projectileOrigin;
    [SerializeField]
    float projectileStartHeight = 2;
    [SerializeField]
    GameObject projectile;

    WyrmState currentState;



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
                currentState = (WyrmState)Random.Range(0, 2);
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


                    head.SetActive(true);
                    tail1.SetActive(true);
                    tail2.SetActive(true);
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

                    break;
            }
        }

        if (projectileOrigin != null) 
        {
            projectileCDRemaining -= Time.fixedDeltaTime;
            if (projectileCDRemaining <= 0) 
            {
                GameObject p = Instantiate(projectile, projectileOrigin.transform.position + projectileStartHeight * Vector3.up, Quaternion.Euler(0, 0, 0));
                projectileOrigin = null;
                p.transform.LookAt(PlayerManager.GetPosition() + Vector3.up);
            }
        }
    }
}

public enum WyrmState 
{ 
    HIDING, OPEN
}
