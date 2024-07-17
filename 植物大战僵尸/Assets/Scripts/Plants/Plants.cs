using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantSpace
{
    public class Plants : MonoBehaviour
    {
        public enum Stats
        {
            Normal,
            Stop,
            Affected,
            Death
        }

        public Stats currentStats;
        public bool isSpecialAttack = false;
        public float currentHealth;
        
        public float maxHealth;
        
        public float bulletSpeed;
        
        public float attackTime = 1f;
        protected float startAttackTime;
        public GameObject bullet;
        [SerializeField]
        protected float damage;
        protected Transform fireBullet;
        protected Animator anim;
        protected string bulletName;
        protected bool isSoundPlay;

        protected GameObject bulletPool;
        private GameObject soil;
        private AudioSource plantAudio;

        public SpriteRenderer sr;
        public Color startColor;

        // Start is called before the first frame update
        public virtual void Start()
        {

            if(transform.Find("fireBullet") != null)
            {
                fireBullet = transform.Find("fireBullet");
            }
            soil = transform.Find("Soil").gameObject;
            bulletName = gameObject.name + "_Bullet";
            bulletPool = GameObject.Find("BulletPool");
            if (Resources.Load("Prefabs/Bullets/" + bulletName) != null)
            {
                bullet = Resources.Load("Prefabs/Bullets/" + bulletName) as GameObject;
            }
            anim = gameObject.GetComponent<Animator>();
            startAttackTime = attackTime;
            currentHealth = maxHealth;
            currentStats = Stats.Stop;
            plantAudio = soil.GetComponent<AudioSource>();
            sr = gameObject.GetComponent<SpriteRenderer>();
            startColor = new Color(1f,1f,1f,1);
        }
        public virtual void Init()
        {
            if (soil == null) return;
            soil.SetActive(true);

            plantAudio.Play();
            StartCoroutine(Plant());
        }
        
        // Update is called once per frame
        void Update()
        {

            if(currentStats == Stats.Normal)
            {
                if (bullet == null)
                    return;
                attackTime -= Time.deltaTime;
                if(attackTime <= 0)
                {
                    Attack();
                    attackTime = startAttackTime;
                }
            }
            else if(currentStats == Stats.Stop)
            {
                anim.speed = 0;
            }
        }
        public virtual void Attack()
        {
            //生成子弹
            if (!isSpecialAttack)
            {

                GameObject Bullet = PoolControl.Instance.GetFromPool<GameObject>(bulletName, bullet);

                Bullet.GetComponent<PlantBullet>().spawnGameObject = gameObject;
                Bullet.GetComponent<PlantBullet>().fireBulletPosition = fireBullet;
                Bullet.GetComponent<PlantBullet>().Init();
                Bullet.GetComponent<PlantBullet>().InitSpeedAndDamage(bulletSpeed, damage);

                Bullet.transform.SetParent(bulletPool.transform);
                //初始位置另行加载

                Bullet.SetActive(true);
            }

        }
        public virtual void TakeDamage(float damage)
        {
            if (currentStats == Stats.Normal)
            {
                currentHealth -= damage;
                if (!isSoundPlay)
                {
                    isSoundPlay = true;
                    AudioSourceManager.Instance.PlayEatPlantSound();
                    StartBlink();
                    StartCoroutine(PlaySoundAndBlink());
                }

                if (currentHealth <= 0)
                {
                    currentStats = Stats.Death;
                    Destroy(gameObject);
                    //PoolControl.Instance.PlaceInPool(gameObject);
                }
            }
        }
        public virtual IEnumerator PlaySoundAndBlink()
        {
            yield return new WaitForSeconds(1f);

            isSoundPlay = false;
        }
        public virtual void StartBlink()//设置闪烁
        {

            sr.color = new Color(181 / 255f, 181 / 255f, 181 / 255f, 1f);
            StartCoroutine(Blink());
        }
        public virtual IEnumerator Blink()
        {

            yield return new WaitForSeconds(0.08f);
            sr.color = startColor;

        }
        public virtual void TakeNormal()
        {
            currentStats = Stats.Normal;
            if (anim == null) return;
            anim.speed = 1;
        }
       public virtual IEnumerator Plant()
        {
            yield return new WaitForSeconds(0.3f);
            soil.SetActive(false);
        }
    }
}
