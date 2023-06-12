using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertLevel : MonoBehaviour, ILevel
{
    private int seed;

    public DesertLevel() 
    {
        seed = (int)Time.time;
    }

    public void LoadLevel()
    {
        GameObject o = Instantiate((GameObject)Resources.Load("LevelGenerator/LandscapeGenerator", typeof(GameObject)));
        MapGenerator m = o.GetComponentInChildren<MapGenerator>();
        m.seed = seed;
        m.GenerateMap();
        
        Mesh mesh = o.GetComponentInChildren<MeshFilter>().mesh;
        o.GetComponentInChildren<MeshCollider>().sharedMesh = mesh;

        RaycastHit hit;
        Physics.Raycast(Vector3.forward, Vector3.up, out hit);
        if (hit.collider == null)
            Physics.Raycast(Vector3.forward, -Vector3.up, out hit);
        if (hit.collider != null)
            Instantiate((GameObject)Resources.Load("Enemies/EnemyWyrm", typeof(GameObject)), hit.point, Quaternion.Euler(0, 0, 0));
    }
}
