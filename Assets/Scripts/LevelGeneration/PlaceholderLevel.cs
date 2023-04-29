using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : ILevel
{
    List<GameObject> currentState = new List<GameObject>();

    public PlaceholderLevel(int number) 
    {
        GameObject levelLayout = (GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject));
        currentState.Add(levelLayout);
        //TODO: Find out why the numbers are the same for one GenerateNewLevels()
        levelLayout.GetComponentInChildren<TextMeshPro>().text = number.ToString();
    }

    public List<GameObject> LoadLevel() 
    {
        return currentState;
    }
}
