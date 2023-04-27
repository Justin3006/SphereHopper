using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: LevelManager statt LevelGenerator
public class LevelGenerator : MonoBehaviour
{
    private static List<ILevel> levels = new List<ILevel>();
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
        int numberOfLevels = Random.Range(minNumberOfLevels, maxNumberOfLevels);

        for (int i = 0; i < numberOfLevels; i++) 
        {
            levels.Add(new PlaceholderLevel(Random.Range(0, 10)));
        }
    }

    public static void LoadLevel(int i) 
    {
        GameObject newO = null;
        foreach (GameObject o in levels[i].LoadLevel()) 
        {
            newO = Instantiate(o);
        }
        currentLevel = i;
    }

    public static void SelectLevel(int i) 
    {
        currentLevel = i;
    }
}
