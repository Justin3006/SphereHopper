using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    float indicatorDurationMax = 0.2f;
    float indicatorDuration;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    protected void handleDmgIndicator()
    {
        if (damageIndicator != null && indicatorDuration > 0)
        {
            indicatorDuration -= Time.deltaTime;
            if (indicatorDuration <= 0)
                damageIndicator.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        handleDmgIndicator();
    }

    public bool Hit() 
    {
        if (shielded)
        {
            shieldedAttacks++;
            if (damageIndicator != null)
            {
                indicatorDuration = indicatorDurationMax;
                damageIndicator.GetComponent<Image>().color = Color.cyan;
                damageIndicator.SetActive(true);
            }
            return false;
        }

        hp--;
        Debug.Log("Hit " + gameObject.name);
        if (hp <= 0)
            Destroy(gameObject);
        if (damageIndicator != null)
        {
            indicatorDuration = indicatorDurationMax;
            damageIndicator.GetComponent<Image>().color = Color.red;
            damageIndicator.SetActive(true);
        }
        return true;
    }
}
