using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyLabyrinthStalker : Vulnerable
{
    Rigidbody rb;
    float speed;
    [SerializeField]
    float normalSpeed = 2.5f;
    [SerializeField]
    float aggressionSpeed = 7.5f;
    [SerializeField]
    float maxRangeToTravel = 5f;
    float rangeToTravel;
    [SerializeField]
    float detectionAngle = 45;
    bool aggressive;
    [SerializeField]
    int blockLeftMax = 1;
    [SerializeField]
    int blockRightMax = 1;
    [SerializeField]
    int blockBackwardsMax = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = normalSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rangeToTravel > Time.fixedDeltaTime * speed)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * speed * transform.forward);
            rangeToTravel -= Time.fixedDeltaTime * speed;
        }
        else if (rangeToTravel > 0)
        {
            rb.MovePosition(transform.position + rangeToTravel * transform.forward);
            rangeToTravel = 0;
        }
        else if (!aggressive)
        {
            bool[] dir = new bool[4];
            RaycastHit hit;
            dir[0] = !Physics.Raycast(transform.position + transform.up, transform.forward, out hit, maxRangeToTravel);
            if (hit.collider != null && (hit.collider.gameObject.GetComponent<Vulnerable>() != null || hit.collider.gameObject.GetComponent<IInteractable>() != null))
            {
                dir[0] = true;
            }
            dir[1] = !Physics.Raycast(transform.position + transform.up, transform.right, out hit, maxRangeToTravel);
            if (hit.collider != null && (hit.collider.gameObject.GetComponent<Vulnerable>() != null || hit.collider.gameObject.GetComponent<IInteractable>() != null))
            {
                dir[1] = true;
            }
            dir[2] = !Physics.Raycast(transform.position + transform.up, -transform.forward, out hit, maxRangeToTravel);
            if (hit.collider != null && (hit.collider.gameObject.GetComponent<Vulnerable>() != null || hit.collider.gameObject.GetComponent<IInteractable>() != null))
            {
                dir[2] = true;
            }
            dir[3] = !Physics.Raycast(transform.position + transform.up, -transform.right, out hit, maxRangeToTravel);
            if (hit.collider != null && (hit.collider.gameObject.GetComponent<Vulnerable>() != null || hit.collider.gameObject.GetComponent<IInteractable>() != null))
            {
                dir[3] = true;
            }

            bool chosen = false;
            int blockLeft = blockLeftMax;
            int blockRight = blockRightMax;
            int blockBackwards = blockBackwardsMax;
            while (!chosen)
            {
                int rn = Random.Range(0, 4);
                switch (rn)
                {
                    case 0: if (dir[0]) { chosen = true; } break;
                    case 1: if (dir[1]) { if (blockRight > 0) { blockRight--; break; } chosen = true; transform.Rotate(0, 90, 0); } break;
                    case 2: if (dir[2]) { if (blockBackwards > 0) { blockBackwards--; break; } chosen = true; transform.Rotate(0, 180, 0); } break;
                    case 3: if (dir[3]) { if (blockLeft > 0) { blockLeft--; break; } chosen = true; transform.Rotate(0, -90, 0); } break;
                }
            }

            rangeToTravel = maxRangeToTravel;
        }
        else if((PlayerManager.GetPosition() - transform.position).magnitude >= maxRangeToTravel/2)
        {
            float[] distances = new float[4];
            distances[0] = Vector3.Dot(transform.forward, PlayerManager.GetPosition() - transform.position);
            distances[1] = Vector3.Dot(transform.right, PlayerManager.GetPosition() - transform.position);
            distances[2] = Vector3.Dot(-transform.forward, PlayerManager.GetPosition() - transform.position);
            distances[3] = Vector3.Dot(-transform.right, PlayerManager.GetPosition() - transform.position);

            int biggest = 0;
            for (int i = 1; i < distances.Length; i++) 
            {
                if (distances[i] >= distances[biggest])
                    biggest = i;
            }

            switch (biggest)
            {
                case 0: break;
                case 1: transform.Rotate(0, 90, 0); break;
                case 2: transform.Rotate(0, 180, 0); break;
                case 3: transform.Rotate(0, -90, 0); break;
            }

            rangeToTravel = maxRangeToTravel;
        }


        if (aggressive || Vector3.Angle(PlayerManager.GetPosition() - transform.position, transform.forward) <= detectionAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up, PlayerManager.GetPosition() - transform.position, out hit))
            {
                if (hit.collider.gameObject.name == "Player")
                {
                    speed = aggressionSpeed;
                    aggressive = true;
                }
                else
                {
                    speed = normalSpeed;
                    aggressive = false;
                }
            }
            else
            {
                speed = normalSpeed;
                aggressive = false;
            }
        }
        else
        {
            speed = normalSpeed;
            aggressive = false;
        }

        if (aggressive && (PlayerManager.GetPosition() - transform.position).magnitude <= maxRangeToTravel / 2)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + transform.up, PlayerManager.GetPosition() - transform.position, out hit);
            hit.collider.gameObject.GetComponent<Vulnerable>().Hit(transform.position, 0, 100 * Time.fixedDeltaTime, 0, 0);
        }
    }
}
