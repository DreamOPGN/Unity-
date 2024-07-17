using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{

    protected float damage;

    protected float bulletMoveSpeed;
    public bool IsCommonMove;
    public GameObject spawnGameObject;
    public Transform fireBulletPosition;
    void Start()
    {
        IsCommonMove = true;
    }

    
    void Update()
    {
        if (IsCommonMove)
        {
            CommonMove();
        }
        
    }
    public virtual void Init()
    {
            transform.position = fireBulletPosition.position;
        bulletMoveSpeed = 5f;
    }
    public virtual void InitSpeedAndDamage(float speed, float damage)
    {
        bulletMoveSpeed = speed;
        this.damage = damage;
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.name == "Sun")
            return;

        if (collision.CompareTag("Zombie"))
        {

                Zombie z = collision.gameObject.GetComponent<Zombie>();
                z.Damage(damage);
               // z.StartBlink();
                PoolControl.Instance.PlaceInPool(gameObject);


            if(gameObject.name == "SnowPea_Bullet")
            {
                z.StartSnowPea();
            }
        }


    }
    private void CommonMove()
    {

        transform.Translate(Vector3.right * bulletMoveSpeed * Time.deltaTime);
        if(transform.position.x > 9f)
        {

            PoolControl.Instance.PlaceInPool(gameObject);
        }
    }
    public virtual void BulletSpawned()
    {

    }
}
