using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameInterfaceControl : MonoBehaviour
{
    public GameObject menuBtn;
    public GameObject menuPanel;

    public GameObject plantGrid;
    public GameObject win;
    public GameObject lose;
    public GameObject gameOver;

    public Animator anim;
    public GameObject potato;
    public static GameInterfaceControl Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        menuBtn = transform.Find("Menu_btn").gameObject;
        plantGrid = transform.Find("PlantGrid").gameObject;
        anim = Camera.main.GetComponent<Animator>();
        //menuPanel = GameObject.Find("/Canvas/MenuPanel").gameObject;  //一开始物体为失活GameObject.Find找不到
        menuBtn.GetComponent<Button>().onClick.AddListener(MenuBtnClick);
        win.transform.Find("btn").GetComponent<Button>().onClick.AddListener(NextLevelClick);
        lose.transform.Find("restart").GetComponent<Button>().onClick.AddListener(RestartGame);
        lose.transform.Find("backMain").GetComponent<Button>().onClick.AddListener(BackMenu);

    }

    public void MenuBtnClick()
    {
        RayCheckControl.Instance.isCheckSun = false;
        AudioSourceManager.Instance.OnClickBtnSound();
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        potato.SetActive(true);
        win.SetActive(true);
    }
    public void LoseGame()
    {
        // Time.timeScale = 0;
        anim.SetTrigger("lost");
        gameOver.SetActive(true);
        gameOver.transform.DOScale(1, 2f);
        StartCoroutine(loseSetTrue());

    }
    IEnumerator loseSetTrue()
    {
        yield return new WaitForSeconds(2.5f);
        gameOver.SetActive(false);
        lose.SetActive(true);
    }
    public void BackMenu()
    {
        Time.timeScale = 1;
        AudioSourceManager.Instance.OnClickBtnSound();
        SceneManager.LoadScene(Config.SCENE_MAIMANU);
    }
    public void RestartGame()
    {
        AudioSourceManager.Instance.OnClickBtnSound();
        SceneManager.LoadScene(Config.SCENE_GAME);
    }
    public void NextLevelClick()
    {
        LevelDataModel.AddLevel();
        SceneManager.LoadScene(Config.SCENE_GAME);
    }
}
