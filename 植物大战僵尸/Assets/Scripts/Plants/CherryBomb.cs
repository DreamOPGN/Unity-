using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantSpace
{
    public class CherryBomb : Plants
    {
        public float radius = 2f;

        public override void Start()
        {
            base.Start();
            attackTime = 1f;

        }

        // Update is called once per frame
        IEnumerator Boom(float time)
        {
            yield return new WaitForSeconds(time);
            Attack();
        }
        public new void Attack()
        {

            GameObject Bullet = PoolControl.Instance.GetFromPool<GameObject>(bulletName, bullet);

            Bullet.transform.position = fireBullet.position;
            Bullet.transform.SetParent(bulletPool.transform);
            //��ʼλ�����м���

            Bullet.SetActive(true);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(fireBullet.position, radius);

            foreach (Collider2D collider in colliders)
            {
                // ��Բ�η�Χ�ڵ�����
                if (collider.CompareTag("Zombie"))
                {
                    collider.gameObject.GetComponent<Zombie>().BoomDie();
                }
                // ���������ִ����Ӧ���߼�����
            }

            Destroy(gameObject);
            Destroy(Bullet, 0.6f);
        }
        public override void Init()
        {
            base.Init();
        }
        public override void TakeNormal()
        {
            base.TakeNormal();
            StartCoroutine(Boom(attackTime));
            StartCoroutine(PlaySound());
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

