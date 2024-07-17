using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class GameStartControl : MonoBehaviour
{

    public GameObject shovelMove;
  //  public GameObject gameInterface;
    public GameObject gridPlant;
    public GameObject sunNum;
    public GameObject shovel;
    public GameObject levelProgress;
    public GameObject menubtn;

    public GameObject selectPlants;
    public GameObject car;

    public GameObject starrt;
    // public Camera mainCamera;
    public Animator animCamera;
    void Start()
    {
        // gameInterface.SetActive(false);
        Screen.SetResolution(1920,1080, true);
        Time.timeScale = 1;
        shovelMove.SetActive(false);
        gridPlant.SetActive(false);
        sunNum.SetActive(false);
        shovel.SetActive(false);
        levelProgress.SetActive(false);
        menubtn.SetActive(false);
        car.SetActive(false);

        StartCoroutine(startAnim());
        AudioSourceManager.Instance.PlayPreBattleBGM();
    }
/*    private void OnEnable()
    {
        Screen.SetResolution(1920, 1080, true);
    }*/
    IEnumerator startAnim()
    {
        yield return new WaitForSeconds(0.5f);
        animCamera.SetTrigger("move");
        StartCoroutine(select());
    }
    IEnumerator select()
    {
        yield return new WaitForSeconds(3f);

        selectPlants.SetActive(true);
        selectPlants.transform.DOLocalMoveY(0, 0.5f);
        gridPlant.SetActive(true);
        selectPlants.transform.GetChild(0).transform.Find("click").GetComponent<Button>().onClick.AddListener(selectPlantClick);
    }
    public void selectPlantClick()
    {
        if (PlantCardsControl.Instance.combom != 6) return;
        animCamera.SetTrigger("return");
        
        StartCoroutine(carMove());
        for(int i = 0; i < GameObject.Find("ZombiePool").transform.childCount; i++)
        {
         Destroy(GameObject.Find("ZombiePool").transform.GetChild(i).gameObject);
        }
        PlantCardsControl.Instance.LoadPlantCards();
        PlantCardsControl.Instance.PlantCardsMaskOpen();
        selectPlants.transform.DOLocalMoveY(-889, 0.5f);
    }
    IEnumerator carMove()
    {
        yield return new WaitForSeconds(2f);
        GC.Collect();
        car.SetActive(true);
        car.transform.DOLocalMoveX(-1.3f, 0.5f);
        StartCoroutine(readyGame());
    }
    IEnumerator readyGame()
    {
        yield return new WaitForSeconds(1.5f);
        starrt.SetActive(true);

        AudioSourceManager.Instance.PlayReadlySetPlantSound();
        StartCoroutine(beginGame());
        StartCoroutine(StartSpawnZombie());

        Destroy(starrt, 1.5f);
    }
    IEnumerator beginGame()
    {
        yield return new WaitForSeconds(1f);

        SunControl.Instance.isStartSpawnSun = true;
        ZombieSpawn.Instance.isStartGame = true;
        selectPlants.SetActive(false);
        PlantCardsControl.Instance.StartGame();

        car.SetActive(true);
       
        sunNum.SetActive(true);
        shovel.SetActive(true);
        //levelProgress.SetActive(true);
        menubtn.SetActive(true);


        StartCoroutine(startGame());
    }
    IEnumerator StartSpawnZombie()
    {
        yield return new WaitForSeconds(20f);
        AudioSourceManager.Instance.PlayZombieCommingSound();
    }
    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1f);
        AudioSourceManager.Instance.PlayBattleBGM();
    }


}
