using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorButton : MonoBehaviour, IInteractable
{
    public void Interact() 
    {
        LevelManager.GenerateNewLevels();
    }
}
