using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTween : MonoBehaviour
{
    private float speed = 200;
    private float destroyTime = 1f;
    void Start()
    {
        GameManager.instance.tipsSpawnCount++;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, speed * Time.deltaTime);
    }
    private void OnDestroy()
    {
        GameManager.instance.tipsSpawnCount--;
    }
}
