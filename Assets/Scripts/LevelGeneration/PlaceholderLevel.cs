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
        //levelLayout.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }

    public List<GameObject> LoadLevel() 
    {
        return currentState;
    }
}
