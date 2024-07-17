using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PotatoMineBullet : PlantBullet
{
    public GameObject ExplosionSpudow;
    void Start()
    {
        ExplosionSpudow = transform.Find("ExplosionSpudow").gameObject;

        ExplosionSpudow.transform.DOScale(1.5f, 1f);
    }
    public override void InitSpeedAndDamage(float speed, float damage)
    {
        base.InitSpeedAndDamage(speed, damage);
    }
    public new void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Zombie"))
        {

            Zombie z = collision.gameObject.GetComponent<Zombie>();
            z.Damage(damage);
            // z.StartBlink();
            Destroy(gameObject, 1.5f);
        }

    }

}
