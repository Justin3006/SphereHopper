using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulnerable : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 3;
    protected int hp;

    protected bool shielded;
    protected int shieldedAttacks;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public bool Hit() 
    {
        if (shielded)
        {
            shieldedAttacks++;
            return false;
        }

        hp--;
        Debug.Log("Hit " + gameObject.name);
        if (hp <= 0)
            Destroy(gameObject);
        return true;
    }
}
