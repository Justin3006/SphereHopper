using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLevel : MonoBehaviour, ILevel
{ 
    List<GameObject> tiles = new List<GameObject>();
    List<Vector3> tilePositions = new List<Vector3>();
    Vector3 goalPost;

    float maxPercentageOfEnemies = 0.33f;
    List<int> enemyPositions = new List<int>();

    int maxLayer = 10;
    float avgDistanceBetweenLayers = 10;
    float maxDistanceBetweenTiles = 20;

    int minTilesPerLayer = 1;
    int maxTilesPerLayer = 2;


    public DropLevel() 
    {
        tiles.Add((GameObject)Resources.Load("LevelGenerator/GroundTile5x1x5", typeof(GameObject)));
        tilePositions.Add(Vector3.zero);

        BuildLevel(Vector3.zero, 1);

        int lowest = 0;
        for (int i = 1; i < tilePositions.Count; i++) 
        {
            if (tiles[i].GetComponent<Hazard>() == null && tilePositions[i].y < tilePositions[lowest].y)
                lowest = i;
        }
        goalPost = tilePositions[lowest];

        int numberOfEnemies = Random.Range(0, (int)(maxPercentageOfEnemies * tiles.Count));
        for (int i = 0; i < numberOfEnemies; i++) 
        {
            int tileIndex = Random.Range(1, tilePositions.Count);
            if (enemyPositions.Contains(tileIndex) || tiles[tileIndex].GetComponent<Hazard>() != null)
                continue;
            enemyPositions.Add(tileIndex);
        }
    }

    private void BuildLevel(Vector3 startPos, int layer) 
    {
        int numOfTIles = Random.Range(minTilesPerLayer, maxTilesPerLayer + 1);
        for (int i = 0; i < numOfTIles; i++) 
        {
            int tileType = Random.Range(0, 2);
            switch (tileType) 
            {
                case 0: tiles.Add((GameObject)Resources.Load("LevelGenerator/GroundTile5x1x5", typeof(GameObject))); break;
                case 1: tiles.Add((GameObject)Resources.Load("LevelGenerator/HazardTile5x1x5", typeof(GameObject))); break;
            }
            
            int rn = Random.Range(-45, 45);
            float heightOffset = Random.Range(-avgDistanceBetweenLayers / 2, avgDistanceBetweenLayers / 2);
            float distanceOffset = Random.Range(0, maxDistanceBetweenTiles);

            tilePositions.Add(startPos + distanceOffset * new Vector3(Mathf.Sin(rn), 0, Mathf.Cos(rn)) - (heightOffset + avgDistanceBetweenLayers) * Vector3.up);

            if (layer <= maxLayer)
            {
                BuildLevel(tilePositions[tilePositions.Count - 1], layer + 1);
            }
        }
    } 

    public void LoadLevel()
    {
        for (int i = 0; i < tiles.Count; i++) {
            GameObject o = Instantiate(tiles[i], tilePositions[i], Quaternion.Euler(0,0,0));
            o.transform.position = tilePositions[i];
            if (enemyPositions.Contains(i)) 
            {
                Instantiate((GameObject)Resources.Load("Enemies/EnemySentry", typeof(GameObject)), tilePositions[i], Quaternion.Euler(0, 0, 0));
            }
        }
        Instantiate((GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)), goalPost, Quaternion.Euler(0, 0, 0));
        Instantiate((GameObject)Resources.Load("LevelGenerator/LowerBorderTile", typeof(GameObject)), new Vector3(0, goalPost.y - 50, 0), Quaternion.Euler(0, 0, 0));
    }
}
