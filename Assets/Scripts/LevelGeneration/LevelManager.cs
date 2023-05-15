using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static List<ILevel> levels = new List<ILevel>();
    private static Dictionary<int, Vector3> levelPositions = new Dictionary<int, Vector3>();
    private static int minNumberOfLevels = 10;
    private static int maxNumberOfLevels = 20;
    private static int currentLevel = -1;


    void Start()
    {
        LoadLevel(currentLevel);
    }

    public static void GenerateNewLevels() 
    {
        levels.Clear();
        levelPositions.Clear();
        int numberOfLevels = Random.Range(minNumberOfLevels, maxNumberOfLevels);

        for (int i = 0; i < numberOfLevels; i++) 
        {
            int rn = Random.Range(0, 4);
            /*switch (rn) 
            {
                case 0: levels.Add(new PlaceholderLevel()); break;
                case 1: levels.Add(new LabyrinthLevel()); break;
                case 2: levels.Add(new ArenaLevel()); break;
                case 3: levels.Add(new DropLevel()); break;
            }*/
            levels.Add(new DropLevel());

            //TODO: Find better way to place different levels
            levelPositions.Add(i, new Vector3(Random.Range(-10, 10), Random.Range(-10, 5), Random.Range(-3, 10)));

            //TODO: Define, which levels are connected
        }
    }

    public static void LoadLevel(int i) 
    {
        levels[i].LoadLevel();    
    }

    public static void SelectLevel(int i) 
    {
        currentLevel = i;
    }

    public static int GetNumberOfLevels() 
    {
        return levels.Count;
    }

    public static Vector3 GetLevelPosition(int level) 
    {
        return levelPositions[level];
    }
}
