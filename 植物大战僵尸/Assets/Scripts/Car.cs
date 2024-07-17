using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool isStart;
    public float speed = 2f;
    private bool isCanPlaySound;

    private void Start()
    {
        isCanPlaySound = true;
    }
    void Update()
    {
        if (isStart)
        {
            if (isCanPlaySound)
            {
                AudioSourceManager.Instance.PlayCarSound(gameObject);
                isCanPlaySound = false;
            }
            
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if(transform.position.x >= 8.4f)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<Zombie>().Damage(5000);
            //PoolControl.Instance.PlaceInPool(collision.gameObject);
            isStart = true;
        }
    }
}
