using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public virtual bool Hit(Vector3 origin, float impact, float stun) 
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

        hp--;
        if (hp <= 0)
            if (gameObject.name != "Player")
                Destroy(gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (damageIndicator != null)
        {
            indicatorDuration = indicatorDurationMax;
            damageIndicator.GetComponent<Image>().color = Color.red;
            damageIndicator.SetActive(true);
        }
        return true;
    }
}
