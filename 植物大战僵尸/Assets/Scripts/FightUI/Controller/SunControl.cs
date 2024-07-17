using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SunControl : MonoBehaviour
{
    public static SunControl Instance;

    public float spawnTime;
    public float startSpawnTime;
    public float alreadyDownSunNum;

    private float a;
    private float b;
    private GameObject sun;
    private Vector2 minPosition = new Vector2(-8.1f, -3.4f);
    private Vector2 maxPosition = new Vector2(7.1f, 4f);
    public Vector3 ranDomPosition;

    public bool isStartSpawnSun = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //startSpawnTime = spawnTime;
        alreadyDownSunNum = 0;
        sun = Resources.Load("Prefabs/Bullets/SunFlower_Bullet") as GameObject;


    }
    // Update is called once per frame
    void Update()
    {
        if (sun == null)
            return;

        if (isStartSpawnSun)
        {
            a = 10 * alreadyDownSunNum + 425;
            if (a <= 950)
            {
                b = Random.Range(0, 274);

                StartCoroutine(SpawnSunTimer((a + b) / 100));
            }
            else
            {
                StartCoroutine(SpawnSunTimer((950 + b) / 100));
            }
            isStartSpawnSun = false;
        }
/*        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            spawnTime = startSpawnTime;
            ranDomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), 64);
            GameObject Sun = PoolControl.Instance.GetFromPool<GameObject>("Sun", sun);
            Sun.transform.position = new Vector3(ranDomPosition.x, 6.7f, 64f);
            Sun.GetComponent<SunView>().Init();
            Sun.SetActive(true);

            Sun.GetComponent<SunView>().FinalPosition(ranDomPosition);
            a = 10 * alreadyDownSunNum + 425;
            if (a <= 950)
            {
                b = Random.Range(0, 274);

                spawnTime = (a + b) / 100;
            }
            else
            {
                spawnTime = (950 + b) / 100;
            }
        }*/
    }
    IEnumerator SpawnSunTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnSun();

    }
    public void SpawnSun()
    {
        ranDomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y), 64);
        GameObject Sun = PoolControl.Instance.GetFromPool<GameObject>("Sun", sun);
        Sun.transform.position = new Vector3(ranDomPosition.x, 6.7f, 64f);
        alreadyDownSunNum++;

        Sun.SetActive(true);
        Sun.GetComponent<SunView>().FinalPosition(ranDomPosition);
        Sun.GetComponent<SunView>().Init();
        /*        StartCoroutine(DestorySun(Sun));
                StartCoroutine(PlaceInPoolSun(Sun));*/

        a = 10 * alreadyDownSunNum + 425;
        if (a <= 950)
        {

            b = Random.Range(0, 274);
            StartCoroutine(SpawnSunTimer((a + b) / 100));
        }
        else
        {

            StartCoroutine(SpawnSunTimer((950 + b) / 100));
        }
    }

/*    IEnumerator DestorySun(GameObject sun)
    {
        yield return new WaitForSeconds(23.5f);
        sun.GetComponent<SpriteRenderer>().DOFade(0, 1.5f);

    }
    IEnumerator PlaceInPoolSun(GameObject sun)
    {
        yield return new WaitForSeconds(25f);
        PoolControl.Instance.PlaceInPool(sun);
    }*/
}
