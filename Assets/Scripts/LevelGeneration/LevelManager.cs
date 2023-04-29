using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static List<ILevel> levels = new List<ILevel>();
    private static Dictionary<ILevel, Vector3> levelPositions = new Dictionary<ILevel, Vector3>();
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
            levels.Add(new PlaceholderLevel(Random.Range(0, 10)));
            //TODO: Find better way to place different levels
            levelPositions.Add(levels[i], new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)));
        }
    }

    public static void LoadLevel(int i) 
    {
        GameObject newO = null;
        foreach (GameObject o in levels[i].LoadLevel()) 
        {
            newO = Instantiate(o);
        }
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
        return levelPositions[levels[level]];
    }
}
