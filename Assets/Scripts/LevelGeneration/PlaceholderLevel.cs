using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : ILevel
{
    List<GameObject> currentState = new List<GameObject>();
    int number;

    public PlaceholderLevel(int number) 
    {
        GameObject levelLayout = (GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject));
        currentState.Add(levelLayout);
        this.number = number;
        
    }

    public List<GameObject> LoadLevel()
    {
        Debug.Log(number);
        return currentState;
    }
}
