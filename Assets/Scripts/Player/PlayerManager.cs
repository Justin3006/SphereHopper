using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static GameObject player;
    
    void Start()
    {
        player = this.gameObject;
    }
    public static Vector3 GetPosition() 
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
}
