using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{
    Ray ray;
    RaycastHit scene;
     
     void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out scene))
            {
                if (scene.collider.name == "HomeSphere")
                    SceneManager.LoadScene(0);
                if (scene.collider.name == "Sphere")
                    SceneManager.LoadScene(2);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
