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
