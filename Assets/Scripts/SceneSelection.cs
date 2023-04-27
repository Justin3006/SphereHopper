using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{
    void Start()
    {
        //TODO: Update the Overworld Layout according to data saved in the LevelGenerator class
        LevelGenerator.GenerateNewLevels(); //for testing, delete later
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit scene;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out scene))
            {
                if (scene.collider.name == "HomeSphere")
                    SceneManager.LoadScene(0);
                else if (scene.collider.name == "Sphere0")
                {
                    SceneManager.LoadScene(2);
                    LevelGenerator.SelectLevel(0);
                }
                else if (scene.collider.name == "Sphere1")
                {
                    SceneManager.LoadScene(2);
                    LevelGenerator.LoadLevel(1);
                }
                else if (scene.collider.name == "Sphere2")
                {
                    SceneManager.LoadScene(2);
                    LevelGenerator.LoadLevel(2);
                }
                else if (scene.collider.name == "Sphere3")
                {
                    SceneManager.LoadScene(2);
                    LevelGenerator.LoadLevel(3);
                }
                else
                    return;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
