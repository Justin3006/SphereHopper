using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceholderLevel : MonoBehaviour, ILevel
{
    int number;

    public PlaceholderLevel() 
    {
        number = Random.Range(0, 10);
    }

    public void LoadLevel()
    {
        Debug.Log("Placeholder Level Number: " + number);
        GameObject levelLayout = Instantiate((GameObject)Resources.Load("LevelGenerator/PlaceholderLevelLayout", typeof(GameObject)));
        levelLayout.GetComponent<TextMeshProUGUI>().text = number.ToString();
    }
}
