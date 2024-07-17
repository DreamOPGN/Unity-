using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SunView : PlantBullet
{
    private float num = 25f;
    private float moveSpeed;
    public float startSpeed;
    public Vector3 finalPosition;
    private bool canAddSun;

    private bool IsSunFlowerSpawnMove = true;

    private Rigidbody2D rb2d;

    private int forceMagnitude;
    private float sectorAngle = 15f;
    private Transform startPosition;
    private Transform firePosition;
    private Vector3 stopPosition;
    private AudioSource getSun;
    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        getSun = GameObject.Find("GetSun").GetComponent<AudioSource>();
        if (IsSunFlowerSpawnMove)
        {

            BulletSpawned();
        }
    }
    private void Update()
    {
        if (IsSunFlowerSpawnMove)
        {
            SunFlowerSpawn();
        }
        else
        {
            if (Vector3.Distance(transform.position, finalPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = finalPosition; // 确保在目标位置停止
                if (canAddSun)
                {
                    GetSun();
                }
            }
        }

       
    }
    public new void  Init()
    {
        StopCoroutine(DestorySun(gameObject));
        StopCoroutine(PlaceInPoolSun(gameObject));

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        moveSpeed = startSpeed;
        canAddSun = false;
        IsSunFlowerSpawnMove = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        StartCoroutine(DestorySun(gameObject));
        StartCoroutine(PlaceInPoolSun(gameObject));
    }
    private void BulletSpawnInit()
    {

        rb2d.gravityScale = 1;
        startPosition = spawnGameObject.transform.Find("startPosition");
        firePosition = spawnGameObject.transform.Find("fireBullet");
        transform.position = firePosition.position;
        float randomAngle = Random.Range(-sectorAngle / 2f, sectorAngle / 2f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        forceMagnitude = Random.Range(150, 200);
        Vector2 force = randomDirection * forceMagnitude;

        rb2d.AddForce(force, ForceMode2D.Force);
        IsSunFlowerSpawnMove = true;
    }
    public void OnEnable()
    {
        if (gameObject.name == "SunFlower_Bullet")
        {
            BulletSpawnInit();
        }
    }
    public void FinalPosition(Vector3 position)
    {
        finalPosition = position;

    }
    public void Detected()
    {
        finalPosition = GameObject.Find("/GetSun").transform.position;
        rb2d.gravityScale = 0;
        canAddSun = true;
        IsSunFlowerSpawnMove = false;
        moveSpeed = 10f;
    }
    public void GetSun()
    {
        SunModle.Instance.ChangeSunNum(num);
        PoolControl.Instance.PlaceInPool(gameObject);
        canAddSun = false;

    }
    public  void SunFlowerSpawn()
    {
        if (startPosition == null) return;
       if(transform.position.y <= startPosition.position.y)
       {
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            stopPosition = new Vector3(transform.position.x, startPosition.position.y, transform.position.z);
            transform.position = stopPosition;
            rb2d.gravityScale = 0;
       }


    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GetSun") && canAddSun)
        {
            getSun.Play();
        }
    }
    public override void BulletSpawned()
    {
        base.BulletSpawned();
        BulletSpawnInit();

    }
    IEnumerator DestorySun(GameObject sun)
    {
        yield return new WaitForSeconds(23.5f);
        sun.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);

    }
    IEnumerator PlaceInPoolSun(GameObject sun)
    {
        yield return new WaitForSeconds(25f);
        PoolControl.Instance.PlaceInPool(sun);
    }
}
