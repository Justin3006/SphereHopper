using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySentry : Vulnerable
{
    //TODO: put the head attribute into Vulnerable and change LockOn and distance calculations to heads
    [SerializeField]
    private Vector3 head;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float barrageCDMax = 2;
    private float barrageCD;
    [SerializeField]
    private int projectilesInBarrage = 1;
    private int projectilesToFire;
    [SerializeField]
    private float projectileCDMax = 0.1f;
    private float projectileCD;
    [SerializeField]
    private float spreadAngleMax = 2.5f;

    [SerializeField]
    private float attackRange = 30f;


    private void FixedUpdate()
    {
        if (projectilesToFire <= 0) 
        {
            if (barrageCD >= 0)
                barrageCD -= Time.fixedDeltaTime;

            RaycastHit hit;
            if (Physics.Raycast(head + transform.position, PlayerManager.GetPosition() + Vector3.up - (head + transform.position), out hit, attackRange)) 
            {
                if (hit.collider.gameObject.name == "Player" && barrageCD <= 0) 
                {
                    projectilesToFire = projectilesInBarrage;
                }
            }
        }
        else
        {
            if (projectileCD <= 0)
            {
                Vector3 dir = (PlayerManager.GetPosition() + Vector3.up - (head + transform.position)).normalized;
                GameObject o = Instantiate(projectile, head + transform.position + dir, Quaternion.Euler(0, 0, 0));
                o.transform.LookAt(PlayerManager.GetPosition() + Vector3.up);
                float randX = Random.Range(-spreadAngleMax, spreadAngleMax);
                float randY = Random.Range(-spreadAngleMax, spreadAngleMax);
                o.transform.eulerAngles += new Vector3(randX, randY, 0);

                projectilesToFire--;
                projectileCD = projectileCDMax;
                if (projectilesToFire <= 0)
                    barrageCD = barrageCDMax;
            }
            else 
            {
                projectileCD -= Time.fixedDeltaTime;
            }
        }
    }
}
