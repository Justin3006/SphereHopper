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
    List<GameObject> connections = new List<GameObject>();

    void Start()
    {
        if (LevelManager.GetNumberOfLevels() == 0)
            LevelManager.GenerateNewLevels();

        spheres = new GameObject[LevelManager.GetNumberOfLevels()];
        for (int i = 0; i < spheres.Length; i++)
        {
            spheres[i] = Instantiate(defaultSphere, LevelManager.GetLevelPosition(i), Quaternion.Euler(0, 0, 0));
        }


        //TODO: change line colors
        for (int i = 0; i < LevelManager.GetNumberOfLevels(); i++) 
        {
            if (LevelManager.GetConnections()[0, i + 1])
            {
                GameObject o = Instantiate(new GameObject());
                LineRenderer line = o.AddComponent<LineRenderer>();
                Vector3[] positions = { Vector3.zero, spheres[i].transform.position };
                line.SetPositions(positions);
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                line.startColor = Color.magenta;
                line.endColor = Color.magenta;
                if (LevelManager.GetCurrentLevel() == -1)
                    line.startColor = Color.green;
                if (LevelManager.GetCurrentLevel() == i)
                    line.endColor = Color.green;
                connections.Add(o);
            }
        }

        for (int i = 0; i < LevelManager.GetNumberOfLevels(); i++) 
        {
            for (int j = 0; j < LevelManager.GetNumberOfLevels(); j++) 
            {
                if (LevelManager.GetConnections()[i + 1, j + 1]) 
                {
                    GameObject o = Instantiate(new GameObject());
                    LineRenderer line = o.AddComponent<LineRenderer>();
                    Vector3[] positions = { spheres[i].transform.position, spheres[j].transform.position };
                    line.SetPositions(positions);
                    line.startWidth = 0.1f;
                    line.endWidth = 0.1f;
                    line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                    line.startColor = Color.magenta;
                    line.endColor = Color.magenta;
                    if (LevelManager.GetCurrentLevel() == i)
                        line.startColor = Color.green;
                    else if (LevelManager.GetCurrentLevel() == j)
                        line.endColor = Color.green;
                    connections.Add(o);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit scene;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool success = false;

            if (Physics.Raycast(ray, out scene)) {
                if (scene.collider.gameObject == homeSphere && LevelManager.SelectLevel(-1))
                {
                    SceneManager.LoadScene(0);
                    success = true;
                }
                else
                    for (int i = 0; i < spheres.Length; i++)
                    {
                        if (scene.collider.gameObject == spheres[i] && LevelManager.SelectLevel(i))
                        {
                            SceneManager.LoadScene(2);
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

