using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWyrmHead : Vulnerable
{
    protected override void Kill() 
    {
        Destroy(gameObject.transform.parent.gameObject);
        Instantiate((GameObject)Resources.Load("LevelGenerator/Transporter", typeof(GameObject)), transform.parent.position, Quaternion.Euler(0, 0, 0));
    }
}
