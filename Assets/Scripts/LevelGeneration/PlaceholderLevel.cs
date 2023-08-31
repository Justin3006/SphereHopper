using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : MonoBehaviour, ILevel
{
    public PlaceholderLevel() 
    {
    }

    public void LoadLevel()
    {
        Instantiate((GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject)));
    }
}
