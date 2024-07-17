using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreepeaterBullet : PlantBullet
{
    private Rigidbody2D rb2d;
    public Transform firePosition;
    private void Start()
    {

        IsCommonMove = false;
        bulletMoveSpeed = 5f;
        rb2d = GetComponent<Rigidbody2D>();
    }
    public new void Init()
    {

        IsCommonMove = false;
        bulletMoveSpeed = 5f;
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        transform.Translate(Vector3.right * bulletMoveSpeed * Time.deltaTime);
        if(transform.position.y >= firePosition.position.y + 1.8f || transform.position.y <= firePosition.position.y - 1.3f)
        {
            rb2d.Sleep();
        }
            if (transform.position.x > 9f)
        {
            //PoolControl.Instance.PlaceInPool(gameObject);
            Destroy(gameObject);
        }
    }
}
