using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLevel : MonoBehaviour, ILevel
{
    //TODO: Add ranged enemies (the player should have to dodge a bullet rain)
    List<GameObject> tiles = new List<GameObject>();
    List<Vector3> tilePositions = new List<Vector3>();
    //TODO: generate a Transporter at the end of the level
    Vector3 goalPost;
    //TODO: cap the total amount of tiles in one layer
    int maxLayer = 10;
    int minTilesPerLayer = 1;
    int maxTilesPerLayer = 2;

    public DropLevel() 
    {
        tiles.Add((GameObject)Resources.Load("LevelGenerator/GroundTile5x1x5", typeof(GameObject)));
        tilePositions.Add(Vector3.zero);

        BuildLevel(Vector3.zero, 1);
    }

    private void BuildLevel(Vector3 startPos, int layer) 
    {
        int numOfTIles = Random.Range(minTilesPerLayer, maxTilesPerLayer + 1);
        for (int i = 0; i < numOfTIles; i++) 
        {
            //TODO: Add different types of tiles
            tiles.Add((GameObject)Resources.Load("LevelGenerator/GroundTile5x1x5", typeof(GameObject)));

            //TODO: give the tiles a tendency for the direction
            int rn = Random.Range(0, 360);
            //TODO: vary the y coordinate for new tiles
            tilePositions.Add(startPos + 10 * new Vector3(Mathf.Sin(rn), 0, Mathf.Cos(rn)) - 10 * Vector3.up);

            if (layer <= maxLayer)
            {
                Debug.Log(layer);
                BuildLevel(tilePositions[tilePositions.Count - 1], layer + 1);
            }
        }
    } 

    public void LoadLevel()
    {
        for (int i = 0; i < tiles.Count; i++) {
            GameObject o = Instantiate(tiles[i], tilePositions[i], Quaternion.Euler(0,0,0));
            o.transform.position = tilePositions[i];
        }
    }
}
