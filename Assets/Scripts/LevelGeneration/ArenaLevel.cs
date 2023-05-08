using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLevel : MonoBehaviour, ILevel
{
    List<GameObject[]> enemies = new List<GameObject[]>();

    public ArenaLevel() 
    {
        GameObject enemy = (GameObject)Resources.Load("Enemies/Frenchman", typeof(GameObject));
        GameObject[] wave1 = { enemy };
        GameObject[] wave2 = { enemy, enemy };
        enemies.Add(wave1);
        enemies.Add(wave2);
    }

    public void LoadLevel()
    {
        GameObject o = Instantiate((GameObject)Resources.Load("LevelGenerator/ArenaBaseTile", typeof(GameObject)));
        o.GetComponent<ArenaSpawn>().ArenaSettings(enemies, (GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)));
        Instantiate((GameObject)Resources.Load("LevelGenerator/LowerBorderTile", typeof(GameObject)), - 20 * transform.up, Quaternion.Euler(0,0,0));
    }
}
