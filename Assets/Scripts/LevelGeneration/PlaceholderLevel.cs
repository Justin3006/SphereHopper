using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : MonoBehaviour, ILevel
{
    GameObject levelLayout;
    List<GameObject> currentState;

    public PlaceholderLevel(int number) 
    {
        levelLayout = (GameObject)Resources.Load("LevelGenerator\\PlaceholderLevelLayout", typeof(GameObject));
        GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }

    public void LoadLevel() 
    {
        PlayerManager.GetTransform().position = Vector3.zero;
        PlayerManager.GetTransform().eulerAngles = Vector3.zero;
        currentState.Add(Instantiate(levelLayout));
    }

    public void UnloadLevel() 
    {
        foreach (GameObject o in currentState) 
        {
            Destroy(o);
        }
        currentState.Clear();
    }
}
