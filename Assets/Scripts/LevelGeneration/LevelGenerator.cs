using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: LevelManager statt LevelGenerator
public class LevelGenerator : MonoBehaviour
{
    private static List<ILevel> levels;
    private static int minNumberOfLevels = 10;
    private static int maxNumberOfLevels = 20;
    private static int currentLevel = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (currentLevel != -1)
            //TODO: Does unloading make any sense? You only enter it from the level selection screen anyway, the last opened Level wouldn't be loaded in anyway 
            levels[currentLevel].UnloadLevel();
        levels[i].LoadLevel();
        currentLevel = i;
    }
}
