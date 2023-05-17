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
    GameObject spawnIndicator;
    List<GameObject> activeIndicators = new List<GameObject>();
    int spawnerDistance = 21;
    float spawnTimerMax = 2f;
    float spawnTimerRemaining;

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

            if (noEnemies)
            {
                if (spawnTimerRemaining <= 0)
                {
                    spawnTimerRemaining = spawnTimerMax;

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
                                case 0: activeIndicators.Add(Instantiate(spawnIndicator, new Vector3(-spawnerDistance, 0, 0), Quaternion.Euler(0, 0, 0))); break;
                                case 1: activeIndicators.Add(Instantiate(spawnIndicator, new Vector3(spawnerDistance, 0, 0), Quaternion.Euler(0, 0, 0))); break;
                                case 2: activeIndicators.Add(Instantiate(spawnIndicator, new Vector3(0, 0, -spawnerDistance), Quaternion.Euler(0, 0, 0))); break;
                                case 3: activeIndicators.Add(Instantiate(spawnIndicator, new Vector3(0, 0, spawnerDistance), Quaternion.Euler(0, 0, 0))); break;
                            }
                        }
                    }
                }
                else
                {
                    spawnTimerRemaining -= Time.fixedDeltaTime;
                    if (spawnTimerRemaining <= 0)
                    {
                        if (enemies.Count == 0)
                        {
                            cleared = true;
                            Instantiate(reward, Vector3.zero, Quaternion.Euler(0, 0, 0));
                            return;
                        }

                        GameObject[] enemiesToSpawn = enemies[0];
                        for (int i = 0; i < enemiesToSpawn.Length; i++)
                        {
                            currentEnemies.Add(Instantiate(enemiesToSpawn[i], activeIndicators[0].transform.position, Quaternion.Euler(0, 0, 0)));
                            Destroy(activeIndicators[0]);
                            activeIndicators.RemoveAt(0);
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
        }
    }

    public void ArenaSettings(List<GameObject[]> enemies, GameObject reward, GameObject spawnIndicator) 
    {
        this.enemies = enemies;
        this.reward = reward;
        this.spawnIndicator = spawnIndicator;
    } 
}
