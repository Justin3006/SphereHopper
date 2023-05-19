using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static List<ILevel> levels = new List<ILevel>(); // 0 != Home, but first level
    private static bool[,] connected; // 0 = Home
    private static Dictionary<int, Vector3> levelPositions = new Dictionary<int, Vector3>();
    private static int minNumberOfLevels = 10;
    private static int maxNumberOfLevels = 20;
    private static int currentLevel = -1;
    private static int maxDepth = 4;
    private static float minDistance = 2;
    private static float maxDistance = 3.5f;
    

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public static void GenerateNewLevels() 
    {
        levels.Clear();
        levelPositions.Clear();
        int numberOfLevels = Random.Range(minNumberOfLevels, maxNumberOfLevels);
        connected = new bool[numberOfLevels + 1, numberOfLevels + 1];
        connected[0, 0] = true;

        Vector3 currentPos = Vector3.zero;
        for (int i = 0; i < maxDepth; i++) 
        {
            int rn = Random.Range(0, 4);
            switch (rn) 
            {
                case 0: levels.Add(new PlaceholderLevel()); break;
                case 1: levels.Add(new LabyrinthLevel()); break;
                case 2: levels.Add(new ArenaLevel()); break;
                case 3: levels.Add(new DropLevel()); break;
            }

            Vector3 oldPos = currentPos;
            bool tooClose;
            do
            {
                tooClose = false;
                currentPos += new Vector3(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance), 0);
                
                if (currentPos.magnitude < minDistance)
                {
                    tooClose = true;
                    currentPos = oldPos;
                }

                for (int j = 0; j < levelPositions.Count; j++)
                {
                    if ((levelPositions[j] - currentPos).magnitude < minDistance)
                    {
                        tooClose = true;
                        currentPos = oldPos;
                    }
                }

            }
            while (tooClose);
            levelPositions.Add(i, currentPos);

            connected[i, i + 1] = true;
            connected[i + 1,i] = true;
            connected[i + 1, i + 1] = true;
        }

        for (int i = maxDepth; i < numberOfLevels; i++) 
        {
            int rn = Random.Range(0, 4);
            switch (rn)
            {
                case 0: levels.Add(new PlaceholderLevel()); break;
                case 1: levels.Add(new LabyrinthLevel()); break;
                case 2: levels.Add(new ArenaLevel()); break;
                case 3: levels.Add(new DropLevel()); break;
            }

            int priorLevel = Random.Range(0, levelPositions.Count + 1);

            bool tooClose;
            do
            {
                tooClose = false;
                if (priorLevel == 0)
                    currentPos = new Vector3(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance), 0);
                else
                    currentPos = levelPositions[priorLevel - 1] + new Vector3(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance), 0);

                if (currentPos.magnitude < minDistance) 
                {
                    tooClose = true;
                }

                for (int j = 0; j < levelPositions.Count; j++)
                {
                    if ((levelPositions[j] - currentPos).magnitude < minDistance)
                        tooClose = true;
                }
            }
            while (tooClose);
            levelPositions.Add(i, currentPos);

            connected[priorLevel, i+1] = true;
            connected[i+1, priorLevel] = true;
            connected[i+1, i+1] = true;
        }

        for (int i = 0; i < levelPositions.Count; i++) 
        {
            for (int j = 0; j < levelPositions.Count; j++) 
            {
                if ((levelPositions[i] - levelPositions[j]).magnitude <= maxDistance) 
                {
                    connected[j + 1, i + 1] = true;
                    connected[i + 1, j + 1] = true;
                }    
            }

            if (levelPositions[i].magnitude <= maxDistance) 
            {
                connected[0, i + 1] = true;
                connected[i + 1, 0] = true;
            }
        }
    }

    public static void LoadLevel(int i) 
    {
        levels[i].LoadLevel();    
    }

    public static void ResetLevel() 
    {
        currentLevel = -1;
    }

    public static bool SelectLevel(int i) 
    {
        if (connected[currentLevel + 1, i + 1])
        {
            currentLevel = i;
            return true;
        }
        return false;
    }

    public static int GetNumberOfLevels() 
    {
        return levels.Count;
    }

    public static Vector3 GetLevelPosition(int level) 
    {
        return levelPositions[level];
    }

    public static bool[,] GetConnections() 
    {
        return connected;
    }

    public static int GetCurrentLevel() 
    {
        return currentLevel;
    }
}
