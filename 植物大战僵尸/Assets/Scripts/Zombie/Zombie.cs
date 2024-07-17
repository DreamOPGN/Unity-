using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlantSpace;

public class Zombie : MonoBehaviour
{
    public enum stats
    {
        Normal,
        Attack,//π•ª˜
        Frost,//±˘∂≥
        Death
    }
    public stats currentStats;
    public bool isCommonMove;
    public float speed;
    public float startSpeed;
    public float currentHealth;
    public float maxHealth;
    public float damage;
    public float attackCDTime;
    public bool isAleadyCloseHead;
    public Animator anim;
    public SpriteRenderer sr;

    public Color startColor;
    public GameObject head;
    public GameObject currentAttackPlant;
    public float timer;
    public bool isAttack;
    void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        timer = 0;
        currentStats = stats.Normal;
        currentHealth = maxHealth;
        sr = gameObject.GetComponent<SpriteRenderer>();
        //speed = startSpeed;
        startColor = sr.color;
        anim = GetComponent<Animator>();
        if(transform.Find("ZombieHead") != null)
        {
            head = transform.Find("ZombieHead").gameObject;
        }
        head.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        isAleadyCloseHead = false;
        currentAttackPlant = null;


    }
    
    void Update()
    {
        if (isCommonMove)
        {
            CommonMove();
        }
    }
    private void CommonMove()
    {

        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    public virtual void Damage(float damage)
    {

        currentHealth -= damage;

        StartBlink();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            speed = 0;
            anim.SetTrigger("Die");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ReturnPool(0.8f));
            anim.SetBool("CommonAttack", false);
            anim.SetBool("NoEquipAttack", false);
            anim.SetBool("NoHeadMove", false);
            anim.SetBool("NoHeadAttack", false);
            anim.SetBool("NoEquipMove", false);
            currentStats = stats.Death;
        }
    }
    public virtual IEnumerator ReturnPool(float time)
    {
        yield return new WaitForSeconds(time);
        PoolControl.Instance.PlaceInPool(gameObject);
    }
    public virtual void StartBlink()//…Ë÷√…¡À∏
    {

        sr.color = new Color(214f / 255f, 197f / 255f, 197f / 255f, 1f);
        StartCoroutine(Blink());
    }
    public virtual IEnumerator Blink()
    {

        yield return new WaitForSeconds(0.05f);
        sr.color = startColor;
        
    }
    public virtual void StartSnowPea()
    {
        StartCoroutine(SnowPeaEffect());
    }
    public virtual IEnumerator SnowPeaEffect()
    {
        sr.color = new Color(115f / 255f, 150f / 255f, 255f / 255f, 1f);
        speed = startSpeed / 2;
        yield return new WaitForSeconds(0.1f);

        sr.color = startColor;

        yield return new WaitForSeconds(1f);
        if (isAttack || currentHealth <= 0)
        {
            speed = 0;
        }
        else
        {
            speed = startSpeed;
        }
    }
    public virtual void BoomDie()
    {
        anim.SetTrigger("BoomDie");
        StartCoroutine(ReturnPool(1.6f));
        speed = 0;
    }
    public virtual void Attack()
    {
        StartCoroutine(AttackInterval());
    }
    public float attackDamage;
    public virtual IEnumerator AttackInterval()
    {
        while (timer < 40)
        {
            timer += Time.deltaTime;
           // float currentPlantHealth = currentAttackPlant.GetComponent<Plants>().currentHealth;
            attackDamage = 100 * Time.deltaTime;
            if(currentAttackPlant != null)
            {
                currentAttackPlant.GetComponent<Plants>().TakeDamage(attackDamage);
            }

            yield return null;
        }
        timer = 0;
    }

}
