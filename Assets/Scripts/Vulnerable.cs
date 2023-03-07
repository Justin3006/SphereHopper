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

    [SerializeField]
    GameObject damageIndicator;
    [SerializeField]
    float indicatorDurationMax = 0.1f;
    float indicatorDuration;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if (damageIndicator != null && indicatorDuration > 0)
        {
            indicatorDuration -= Time.deltaTime;
            if (indicatorDuration <= 0)
                damageIndicator.SetActive(false);
        }
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
        if (damageIndicator != null)
        {
            indicatorDuration = indicatorDurationMax;
            damageIndicator.SetActive(true);
        }
        return true;
    }
}
