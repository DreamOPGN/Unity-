using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantSpace
{
    public class Threepeater : Plants
    {
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }
        private void Update()
        {
            if (currentStats == Stats.Normal)
            {
                attackTime -= Time.deltaTime;
                if (attackTime <= 0)
                {
                    Attack();
                    attackTime = startAttackTime;
                }
            }
            else if (currentStats == Stats.Stop)
            {
                anim.speed = 0;
            }
        }
        public new void Attack()
        {
            
            for (int i = 0; i < 3; i++)
            {
                GameObject Bullet = PoolControl.Instance.GetFromPool<GameObject>(bulletName, bullet);
                Rigidbody2D rb2d = Bullet.GetComponent<Rigidbody2D>();

                if (i == 2)
                {

                    rb2d.AddForce(Vector2.down * 125);
                }
                else if(i == 0)
                {

                    rb2d.AddForce(Vector2.up * 125);
                }

                Bullet.transform.position = fireBullet.position;
                Bullet.transform.SetParent(bulletPool.transform);
                Bullet.GetComponent<ThreepeaterBullet>().Init();
                Bullet.GetComponent<ThreepeaterBullet>().firePosition = fireBullet;
                Bullet.SetActive(true);
            }
        }
        public override void Init()
        {
            base.Init();
        }
        public override void TakeNormal()
        {
            base.TakeNormal();
        }
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
        }
        public override IEnumerator Plant()
        {
            return base.Plant();
        }
        public override IEnumerator Blink()
        {
            return base.Blink();
        }
        public override void StartBlink()
        {
            base.StartBlink();
        }
        public override IEnumerator PlaySoundAndBlink()
        {
            return base.PlaySoundAndBlink();
        }
    }
}

