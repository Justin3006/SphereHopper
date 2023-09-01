using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour, ILevel
{
    Dictionary<float, float[]> xyLayer = new Dictionary<float, float[]>();
    Dictionary<float, float[]> zyLayer = new Dictionary<float, float[]>();
    List<Vector3> jumpPads = new List<Vector3>();
    List<Vector3> enemies = new List<Vector3>();
    Vector3 endPos;
    
    [SerializeField]
    int maxHeight = 50;
    [SerializeField]
    int maxDepth = 75;

    [SerializeField]
    float minHeightDifference = 7;
    [SerializeField]
    float maxHeightDifference = 12;

    [SerializeField]
    int minNumberOfTIlesPerHeight = 3;
    [SerializeField]
    int maxNumberOfTilesPerHeight = 7;

    public GridLevel() 
    {
        BuildLevel(Vector3.zero, 0);
    }

    private void BuildLevel(Vector3 startPos, float currentHeight) 
    {
        float heightToAdd = Random.Range(minHeightDifference, maxHeightDifference);
        currentHeight += heightToAdd;

        int numberOfTiles = Random.Range(minNumberOfTIlesPerHeight, maxNumberOfTilesPerHeight + 1);
        float[] xyTilesAtCurrentHeight = new float[numberOfTiles];
        
        xyTilesAtCurrentHeight[0] = startPos.x - 2 + Random.Range(0, 2) * 4;
        for (int i = 1; i < numberOfTiles; i++)
            xyTilesAtCurrentHeight[i] = Random.Range(-maxDepth+10, maxDepth-10);

        xyLayer.Add(currentHeight, xyTilesAtCurrentHeight);


        numberOfTiles = Random.Range(minNumberOfTIlesPerHeight, maxNumberOfTilesPerHeight + 1);
        float[] zyTilesAtCurrentHeight = new float[numberOfTiles];

        zyTilesAtCurrentHeight[0] = startPos.z - 2 + Random.Range(0, 2) * 4;
        for (int i = 1; i < numberOfTiles; i++)
            zyTilesAtCurrentHeight[i] = Random.Range(-maxDepth+10, maxDepth-10);

        zyLayer.Add(currentHeight, zyTilesAtCurrentHeight);

        enemies.Add(new Vector3(xyTilesAtCurrentHeight[2], currentHeight, zyTilesAtCurrentHeight[2]));
        Vector3 newPos = new Vector3(xyTilesAtCurrentHeight[1], currentHeight, zyTilesAtCurrentHeight[1]);

        if (currentHeight <= maxHeight)
        {
            jumpPads.Add(newPos);
            BuildLevel(newPos + heightToAdd * Vector3.up, currentHeight);
        }
        else 
        {
            endPos = newPos;
        }
    }

    public void LoadLevel()
    {
        GameObject o = Instantiate((GameObject)Resources.Load("LevelGenerator/GroundTile1x1x20", typeof(GameObject)));
        o.transform.localScale = new Vector3(1.5f, 1, 2*maxDepth/20);
        o = Instantiate((GameObject)Resources.Load("LevelGenerator/GroundTile1x1x20", typeof(GameObject)), 3*Vector3.forward, Quaternion.Euler(0,90,0));
        o.transform.localScale = new Vector3(1.5f, 1, 2 * maxDepth / 20);
        Instantiate((GameObject)Resources.Load("LevelGenerator/JumpPad", typeof(GameObject)), 3*Vector3.forward, Quaternion.Euler(0, 0, 0));

        foreach (float height in xyLayer.Keys) 
        {
            foreach (float depth in xyLayer[height]) 
            {
                o = Instantiate((GameObject)Resources.Load("LevelGenerator/GroundTile1x1x20", typeof(GameObject)), new Vector3(depth, height, 0), Quaternion.Euler(0, 0, 0));
                o.transform.localScale = new Vector3(1.5f, 1, 2 * maxDepth / 20);
            }

            foreach (float depth in zyLayer[height])
            {
                o = Instantiate((GameObject)Resources.Load("LevelGenerator/GroundTile1x1x20", typeof(GameObject)), new Vector3(0, height, depth), Quaternion.Euler(0, 90, 0));
                o.transform.localScale = new Vector3(1.5f, 1, 2 * maxDepth / 20);
            }
        }

        foreach (Vector3 v in jumpPads) 
        {
            Instantiate((GameObject)Resources.Load("LevelGenerator/JumpPad", typeof(GameObject)), v, Quaternion.Euler(0, 0, 0));
        }

        foreach (Vector3 v in enemies) 
        {
            Instantiate((GameObject)Resources.Load("Enemies/EnemySentry", typeof(GameObject)), v, Quaternion.Euler(0, 0, 0));
        }

        Instantiate((GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)), endPos, Quaternion.Euler(0, 0, 0));
        Instantiate((GameObject)Resources.Load("LevelGenerator/LowerBorderTile", typeof(GameObject)), new Vector3(0, -50, 0), Quaternion.Euler(0, 0, 0));
    }
}
