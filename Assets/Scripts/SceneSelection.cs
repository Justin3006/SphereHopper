using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{
    [SerializeField]
    GameObject homeSphere;
    [SerializeField]
    GameObject defaultSphere;
    GameObject[] spheres;

    void Start()
    {
        //TODO: Update the Overworld Layout according to data saved in the LevelManager class
        if (LevelManager.GetNumberOfLevels() == 0)
            LevelManager.GenerateNewLevels();

        spheres = new GameObject[LevelManager.GetNumberOfLevels()];
        for (int i = 0; i < spheres.Length; i++) 
        {
            spheres[i] = Instantiate(defaultSphere, LevelManager.GetLevelPosition(i), Quaternion.Euler(0,0,0));
        }

        //TODO: Draw Lines between adjacent spheres with a LineRenderer
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit scene;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool success = false;

            if (Physics.Raycast(ray, out scene)) {
                if (scene.collider.gameObject == homeSphere)
                {
                    SceneManager.LoadScene(0);
                    success = true;
                }
                else
                    for (int i = 0; i < spheres.Length; i++)
                    {
                        if (scene.collider.gameObject == spheres[i])
                        {
                            SceneManager.LoadScene(2);
                            LevelManager.SelectLevel(i);
                            success = true;
                        }
                    }

                if (success == true)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }            
    }
}

