using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static GameObject player;
    
    void Awake()
    {
        player = this.gameObject;
    }

    public static Vector3 GetPosition() 
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    public static void AddUsesToAbilities(int uses)
    {
        IAbility[] abilities = player.GetComponents<IAbility>();
        foreach (IAbility a in abilities) 
        {
            a.AddUses(uses);
        }
    }
}
