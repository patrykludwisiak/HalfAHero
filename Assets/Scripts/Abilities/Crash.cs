using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : AbilityTest
{
    // Start is called before the first frame update
    [SerializeField] float animLength;
    public float remainingTime = 0;
    protected List<GameObject> attackedEnemies = new List<GameObject>();
    protected Animator animator;

    // Update is called once per frame
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override AbilityReturn AbilityScript(WeaponTest weapon)
    {
        if(remainingTime <= 0)
        {
            remainingTime = animLength;
        }
        animator.enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        animator.Play("ExplosionAnimation");
        remainingTime -= Time.deltaTime;
        if (remainingTime > 0)
        {
            return AbilityReturn.False;
        }
        else
        {
            if (attackedEnemies.Capacity > 0)
            {
                attackedEnemies.Clear();
            }
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        animator.enabled = false;
        return AbilityReturn.True;
    }
    /*
    public virtual bool Cast(bool trigger)
    {
        if (trigger)
        {
            remainingTime = animLength;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<Animator>().Play("ExplosionAnimation");
        }
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            return false;
        }
        else
        {
            if(attackedEnemies.Capacity > 0)
            {
                attackedEnemies.Clear();
            }
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        return true;
    }
    */
}
