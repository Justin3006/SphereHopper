using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthLevel : MonoBehaviour, ILevel
{
    bool[,] visited = new bool[21,21];
    bool[,] deleteWallZ = new bool[21,20];
    bool[,] deleteWallX = new bool[20,21];
    float additionalDestrPerc = 10;
    Vector3 goalPost;
    Vector3 stalkerPosition1;
    Vector3 stalkerPosition2;
    Vector3 stalkerPosition3;
    Vector3 stalkerPosition4;


    public LabyrinthLevel() 
    {
        MazeBuilder(11, 11);

        deleteWallZ[10, 10] = true;
        for (int i = 0; i < 21; i++)
            for (int j = 0; j < 20; j++)
            {
                int destroyX = Random.Range(0, 100);
                int destroyZ = Random.Range(0, 100);

                if (destroyX < additionalDestrPerc)
                    deleteWallX[j, i] = true;

                if (destroyZ < additionalDestrPerc)
                    deleteWallZ[i, j] = true;
            }

        int rn = Random.Range(0, 4);
        switch (rn) 
        {
            case 0: goalPost = new Vector3(-50, 0, -50); break;
            case 1: goalPost = new Vector3(-50, 0, 50); break;
            case 2: goalPost = new Vector3(50, 0, -50); break;
            case 3: goalPost = new Vector3(50, 0, 50); break;
        }
        switch (rn)
        {
            case 0: stalkerPosition1 = new Vector3(50, 0, 50); break;
            case 1: stalkerPosition1 = new Vector3(50, 0, -50); break;
            case 2: stalkerPosition1 = new Vector3(-50, 0, 50); break;
            case 3: stalkerPosition1 = new Vector3(-50, 0, -50); break;
        }
        switch (rn)
        {
            case 0: stalkerPosition2 = new Vector3(-50, 0, 50); break;
            case 1: stalkerPosition2 = new Vector3(50, 0, 50); break;
            case 2: stalkerPosition2 = new Vector3(-50, 0, -50); break;
            case 3: stalkerPosition2 = new Vector3(50, 0, -50); break;
        }
        switch (rn)
        {
            case 0: stalkerPosition3 = new Vector3(50, 0, -50); break;
            case 1: stalkerPosition3 = new Vector3(-50, 0, -50); break;
            case 2: stalkerPosition3 = new Vector3(50, 0, 50); break;
            case 3: stalkerPosition3 = new Vector3(-50, 0, 50); break;
        }
        switch (rn)
        {
            case 0: stalkerPosition4 = new Vector3(-45, 0, -45); break;
            case 1: stalkerPosition4 = new Vector3(-45, 0, 45); break;
            case 2: stalkerPosition4 = new Vector3(45, 0, -45); break;
            case 3: stalkerPosition4 = new Vector3(45, 0, 45); break;
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
        Instantiate((GameObject)Resources.Load("Enemies/EnemyLabyrinthStalker", typeof(GameObject)), stalkerPosition1, Quaternion.Euler(0, 0, 0));
        Instantiate((GameObject)Resources.Load("Enemies/EnemyLabyrinthStalker", typeof(GameObject)), stalkerPosition2, Quaternion.Euler(0, 0, 0));
        Instantiate((GameObject)Resources.Load("Enemies/EnemyLabyrinthStalker", typeof(GameObject)), stalkerPosition3, Quaternion.Euler(0, 0, 0));
        Instantiate((GameObject)Resources.Load("Enemies/EnemyLabyrinthStalker", typeof(GameObject)), stalkerPosition4, Quaternion.Euler(0, 0, 0));
    }
}
