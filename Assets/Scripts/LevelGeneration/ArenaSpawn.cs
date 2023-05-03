using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawn : MonoBehaviour
{
    bool active;
    bool cleared;
    float size = 15;
    List<GameObject[]> enemies;
    List<GameObject> currentEnemies = new List<GameObject>();
    GameObject reward;
    int spawnerDistance = 21;

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((PlayerManager.GetPosition() - transform.position).magnitude < size)
            active = true;

        if (active && !cleared) 
        {
            bool noEnemies = true;
            foreach (GameObject enemy in currentEnemies) 
            {
                if (enemy != null)
                    noEnemies = false;
            }

            if (noEnemies) {
                if (enemies.Count == 0)
                {
                    cleared = true;
                    Instantiate(reward, Vector3.zero, Quaternion.Euler(0, 0, 0));
                    return;
                }

                currentEnemies.Clear();
                GameObject[] enemiesToSpawn = enemies[0];
                bool[] spawnUsed = new bool[4];

                foreach (GameObject enemy in enemiesToSpawn)
                {
                    bool spawned = false;

                    while (!spawned)
                    {
                        int rn = Random.Range(0, 4);
                        if (spawnUsed[rn])
                            continue;

                        spawned = true;
                        spawnUsed[rn] = true;

                        switch (rn)
                        {
                            case 0: currentEnemies.Add(Instantiate(enemy, new Vector3(-spawnerDistance, 0, 0), Quaternion.Euler(0, 0, 0))); break;
                            case 1: currentEnemies.Add(Instantiate(enemy, new Vector3(spawnerDistance, 0, 0), Quaternion.Euler(0, 0, 0))); break;
                            case 2: currentEnemies.Add(Instantiate(enemy, new Vector3(0, 0, -spawnerDistance), Quaternion.Euler(0, 0, 0))); break;
                            case 3: currentEnemies.Add(Instantiate(enemy, new Vector3(0, 0, spawnerDistance), Quaternion.Euler(0, 0, 0))); break;
                        }
                    }
                }

                foreach (GameObject enemy in currentEnemies) 
                {
                    //TODO: generalize (probably create a base class for all enemy AIs)
                    enemy.GetComponent<EnemySwordMotor>().SetChasing(true);    
                }

                enemies.RemoveAt(0);
            }
        }
    }

    public void ArenaSettings(List<GameObject[]> enemies, GameObject reward) 
    {
        this.enemies = enemies;
        this.reward = reward;
    } 
}
