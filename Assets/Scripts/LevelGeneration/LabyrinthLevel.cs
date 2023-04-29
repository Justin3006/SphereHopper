using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthLevel : MonoBehaviour, ILevel
{
    bool[,] visited = new bool[21,21];
    bool[,] deleteWallZ = new bool[21,20];
    bool[,] deleteWallX = new bool[20,21];
    Vector3 goalPost;


    public LabyrinthLevel() 
    {
        MazeBuilder(11, 11);
        int rn = Random.Range(0, 4);
        switch (rn) 
        {
            case 0: goalPost = new Vector3(-50, 0, -50); break;
            case 1: goalPost = new Vector3(-50, 0, 50); break;
            case 2: goalPost = new Vector3(50, 0, -50); break;
            case 3: goalPost = new Vector3(50, 0, 50); break;
        }
    }

    private void MazeBuilder(int x, int z) 
    {
        visited[x, z] = true;

        while (!((x == 20 || visited[x + 1, z])
            && (x == 0 || visited[x - 1, z])
            && (z == 20 || visited[x, z + 1])
            && (z == 0 || visited[x, z - 1])))
        {
            int rn = Random.Range(0, 4);
            switch (rn)
            {
                case 0:
                    if (x != 20 && !visited[x + 1, z])
                    {
                        MazeBuilder(x + 1, z);
                        deleteWallX[x, z] = true;
                    }
                    break;
                case 1:
                    if (x != 0 && !visited[x - 1, z])
                    {
                        MazeBuilder(x - 1, z);
                        deleteWallX[x - 1, z] = true;
                    }
                    break;
                case 2:
                    if (z != 20 && !visited[x, z + 1])
                    {
                        MazeBuilder(x, z + 1);
                        deleteWallZ[x, z] = true;
                    }
                    break;
                case 3:
                    if (z != 0 && !visited[x, z - 1])
                    {
                        MazeBuilder(x, z - 1);
                        deleteWallZ[x, z - 1] = true;
                    }
                    break;
            }
        }
    }


    public void LoadLevel() 
    {
        GameObject tile = (GameObject)Resources.Load("LevelGenerator/LabyrinthBaseTile", typeof(GameObject));
        for (int x = -10; x <= 10; x++)
            for (int z = -10; z <= 10; z++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(5 * x, 0, 5 * z), Quaternion.Euler(0, 0, 0));
                if (x != 10 && deleteWallX[x+10,z+10])
                    newTile.transform.Find("WallE").gameObject.SetActive(false);
                if (x != -10 && deleteWallX[x+9, z+10])
                    newTile.transform.Find("WallW").gameObject.SetActive(false);
                if (z != 10 && deleteWallZ[x+10,z+10])
                    newTile.transform.Find("WallN").gameObject.SetActive(false);
                if (z != -10 && deleteWallZ[x+10,z+9])
                    newTile.transform.Find("WallS").gameObject.SetActive(false);
            }
        Instantiate((GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)), goalPost, Quaternion.Euler(0, 0, 0));
    }
}
