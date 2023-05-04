using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Vulnerable : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 3;
    [SerializeField]
    bool damageImmunity;
    protected int hp;

    protected bool shielded;
    protected int shieldedAttacks;

    [SerializeField]
    GameObject damageIndicator;
    [SerializeField]
    float indicatorDurationMax = 0.2f;
    float indicatorDuration;

    [SerializeField]
    bool executeImmunity;
    float executePercentage = 0;

    [SerializeField]
    bool knockbackImmunity;
    protected Vector3 directedImpact;
    protected float stun;

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

    public virtual bool Hit(Vector3 origin, int dmg, float addExecPerc, float impact, float stun) 
    {
        if (shielded && Vector3.Dot(transform.forward, origin - transform.position) > 0)
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

        if (damageIndicator != null)
        {
            indicatorDuration = indicatorDurationMax;
            damageIndicator.GetComponent<Image>().color = Color.red;
            damageIndicator.SetActive(true);
        }

        Damage(dmg);
        Execute(addExecPerc);
        Knockback(origin, impact, stun);
        
        return true;
    }

    private void Damage(int dmg) 
    {
        if (damageImmunity)
            return;

        hp -= dmg;
        if (hp <= 0)
            Kill();
    }

    private void Knockback(Vector3 origin, float impact, float stun) 
    {
        if (knockbackImmunity)
            return;

        directedImpact = impact * (transform.position - origin).normalized;
        this.stun = stun;
        
    }

    private void Execute(float addPercentage) 
    {
        if (executeImmunity)
            return;

        executePercentage += addPercentage;
        if (executePercentage >= 100)
            Kill();
    }

    private void Kill()
    {
        if (gameObject.name != "Player")
            Destroy(gameObject);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
