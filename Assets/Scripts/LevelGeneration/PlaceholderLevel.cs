using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : MonoBehaviour, ILevel
{
    int number;

    public PlaceholderLevel(int number) 
    {
        GameObject levelLayout = (GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject));
        this.number = number;
    }

    public void LoadLevel()
    {
        Debug.Log("Placeholder Level Number: " + number);
        Instantiate((GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject)));
    }
}
