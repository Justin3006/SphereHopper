using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLevel : MonoBehaviour, ILevel
{
    List<GameObject[]> enemies = new List<GameObject[]>();

    public ArenaLevel() 
    {
        GameObject[] wave1 = new GameObject[1];
        GameObject[] wave2 = new GameObject[2];
        GameObject[] wave3 = new GameObject[3];

        for (int i = 0; i < wave1.Length; i++) {
            int rn = Random.Range(0, 4);
            bool added = false;
            while (!added) {
                switch (rn)
                {
                    case 0: added = true; wave1[i] = (GameObject)Resources.Load("Enemies/EnemySwordRotColl", typeof(GameObject)); break;
                    case 1: added = true; wave1[i] = (GameObject)Resources.Load("Enemies/Frenchman", typeof(GameObject)); break;
                }
                rn--;
            }
        }

        for (int i = 0; i < wave2.Length; i++)
        {
            int rn = Random.Range(0, 4);
            bool added = false;
            while (!added)
            {
                switch (rn)
                {
                    case 0: added = true; wave2[i] = (GameObject)Resources.Load("Enemies/EnemySwordRotColl", typeof(GameObject)); break;
                    case 1: added = true; wave2[i] = (GameObject)Resources.Load("Enemies/Frenchman", typeof(GameObject)); break;
                }
                rn--;
            }
        }

        for (int i = 0; i < wave3.Length; i++)
        {
            int rn = Random.Range(0, 4);
            bool added = false;
            while (!added)
            {
                switch (rn)
                {
                    case 0: added = true; wave3[i] = (GameObject)Resources.Load("Enemies/EnemySwordRotColl", typeof(GameObject)); break;
                    case 1: added = true; wave3[i] = (GameObject)Resources.Load("Enemies/Frenchman", typeof(GameObject)); break;
                }
                rn--;
            }
        }

        enemies.Add(wave1);
        enemies.Add(wave2);
        enemies.Add(wave3);
    }

    public void LoadLevel()
    {
        GameObject o = Instantiate((GameObject)Resources.Load("LevelGenerator/ArenaBaseTile", typeof(GameObject)));
        o.GetComponent<ArenaSpawn>().ArenaSettings(new List<GameObject[]>(enemies), (GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)));
        Instantiate((GameObject)Resources.Load("LevelGenerator/LowerBorderTile", typeof(GameObject)), - 50 * Vector3.up, Quaternion.Euler(0,0,0));
    }
}
