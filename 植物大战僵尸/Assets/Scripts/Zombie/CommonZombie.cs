using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonZombie : Zombie
{

    
    void Start()
    {
        Init();

    }

    
    void Update()
    {

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if(isAttack)
        {

            if (currentHealth > 70)
            {
                anim.SetBool("CommonAttack", true);

                anim.SetBool("NoHeadMove", false);
            }
            else
            {
                if (!isAleadyCloseHead)
                {
                    head.SetActive(true);
                    StartCoroutine(CloseHead());
                }

                anim.SetBool("CommonAttack", false);
                anim.SetBool("NoHeadAttack", true);

                anim.SetBool("NoHeadMove", false);
            }
        }
        if (!isAttack)
        {

            if (currentHealth > 70)
            {
                anim.SetBool("CommonAttack", false);
            }
            else
            {
                if (!isAleadyCloseHead)
                {
                    head.SetActive(true);
                    StartCoroutine(CloseHead());
                }
                
                anim.SetBool("NoHeadAttack", false);
                anim.SetBool("NoHeadMove", true);
            }
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant"))
        {
            speed = 0;
            isAttack = true;
            currentAttackPlant = collision.gameObject;
            Attack();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Plant"))
        {
            StopCoroutine(AttackInterval());
            currentAttackPlant = null;
            timer = 0;
            isAttack = false;
            if (currentStats != stats.Death)
            {
                speed = startSpeed;
            }

        }
        
    }
    public override void Attack()
    {
        base.Attack();

    }
    public override void Damage(float damage)
    {
        base.Damage(damage);
        AudioSourceManager.Instance.PlantShootSound();
    }
    public override void StartBlink()
    {
        base.StartBlink();
    }
    public override IEnumerator ReturnPool(float time)
    {
        return base.ReturnPool(time);
    }
    public override IEnumerator Blink()
    {
        return base.Blink();
    }
    public override void Init()
    {
        base.Init();
    }
    public override IEnumerator AttackInterval()
    {
        return base.AttackInterval();
    }
    private IEnumerator CloseHead()
    {
        yield return new WaitForSeconds(0.8f);
        head.SetActive(false);
        isAleadyCloseHead = true;
    }
    public override void BoomDie()
    {
        base.BoomDie();
        currentStats = stats.Death;
        speed = 0;
        anim.SetTrigger("BoomDie");
    }
    public override void StartSnowPea()
    {
        base.StartSnowPea();
    }
    public override IEnumerator SnowPeaEffect()
    {
        return base.SnowPeaEffect();
    }
}
