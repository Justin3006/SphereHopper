using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Get rid of most of these methods. Abilities should get a reference to that stuff via the constructor (if needed at al, mostly not), only position (maybe rotation too in the future?) is relevant to enemies and should remain here 
public class PlayerManager : MonoBehaviour
{
    private static GameObject player;
    
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

    public static Rigidbody GetRigidbody() 
    {
        return player.GetComponent<Rigidbody>();
    }

    public static Collider GetCollider() 
    {
        return player.GetComponent<CapsuleCollider>();
    }
}
