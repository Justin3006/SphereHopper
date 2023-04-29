using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transporter : MonoBehaviour, IInteractable
{
    public void Interact() 
    {
        SceneManager.LoadScene(1);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
