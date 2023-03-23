using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
    }

    public static Transform GetTransform() 
    {
        return player.transform;
    }

    public static PlayerMotor GetMotor() 
    {
        return player.GetComponent<PlayerMotor>();
    }
}
