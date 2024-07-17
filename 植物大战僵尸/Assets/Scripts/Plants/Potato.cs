using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantSpace
{
    public class Potato : Plants
    {
        public float radius = 1f;
        private bool CanAttack;
        public override void Start()
        {
            base.Start();
            attackTime = 15f;

        }

        // Update is called once per frame
        IEnumerator Boom(float time)
        {
            yield return new WaitForSeconds(time);
            anim.SetBool("CanBoom", true);
            CanAttack = true;
        }
        public new void Attack()
        {

            GameObject Bullet = PoolControl.Instance.GetFromPool<GameObject>(bulletName, bullet);

            Bullet.transform.position = fireBullet.position;
            Bullet.transform.SetParent(bulletPool.transform);
            //初始位置另行加载

            Bullet.SetActive(true);
            Bullet.GetComponent<PlantBullet>().InitSpeedAndDamage(0, damage);
            AudioSourceManager.Instance.PotatoBomb();
            Destroy(gameObject);

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (CanAttack && collision.CompareTag("Zombie"))
            {
                Attack();
            }
        }
        public override void Init()
        {
            base.Init();
        }
        public override void TakeNormal()
        {
            base.TakeNormal();
            StartCoroutine(Boom(attackTime));
            //StartCoroutine(PlaySound());
        }
        IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(0.6f);
            AudioSourceManager.Instance.CherryBomb();
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
