using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuControl : MonoBehaviour
{
    public GameObject BGMSlider;
    public GameObject soundEffectsSlider;
    public GameObject BGMToggle;
    public GameObject soundEffectsToggle;

    public GameObject restartGame;
    public GameObject returnMainMenu;
    public GameObject returnGame;
    public GameObject waigua;
    void Start()
    {
        BGMSlider = transform.Find("BGMSlider").gameObject;
        soundEffectsSlider = transform.Find("SoundEffectsSlider").gameObject;
        BGMToggle = transform.Find("BGMToggle").gameObject;
        soundEffectsToggle = transform.Find("SoundEffectsToggle").gameObject;
        restartGame = transform.Find("RestartGame").gameObject;
        returnGame = transform.Find("ReturnGame").gameObject;
        returnMainMenu = transform.Find("ReturnMainMenu").gameObject;


        BGMToggle.GetComponent<Toggle>().onValueChanged.AddListener(BGMToggleChange);
        soundEffectsToggle.GetComponent<Toggle>().onValueChanged.AddListener(SoundEffectsToggle);
        BGMSlider.GetComponent<Slider>().onValueChanged.AddListener(BGMSliderChange);
        soundEffectsSlider.GetComponent<Slider>().onValueChanged.AddListener(SoundEffectsSlider);
        restartGame.GetComponent<Button>().onClick.AddListener(RestartGameClick);
        returnMainMenu.GetComponent<Button>().onClick.AddListener(ReturnMenuClick);
        returnGame.GetComponent<Button>().onClick.AddListener(ReturnGameClick);
        waigua.GetComponent<Button>().onClick.AddListener(WaiGua);

        BGMSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
        soundEffectsSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("soundVolume");

    }
    public void BGMToggleChange(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.IsMusic = true;
            PlayerPrefs.SetInt("music", 0);

            BGMSlider.GetComponent<Slider>().value = 1;
        }
        else
        {
            AudioManager.Instance.IsMusic = false;
            BGMSlider.GetComponent<Slider>().value = 0;
            PlayerPrefs.SetInt("music", 1);
        }
        AudioSourceManager.Instance.OnClickBtnSound();
    }
    public void SoundEffectsToggle(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.isSound = true;

            soundEffectsSlider.GetComponent<Slider>().value = 1;
            PlayerPrefs.SetInt("sound", 0);
            
        }
        else
        {
            AudioManager.Instance.isSound = false;
            soundEffectsSlider.GetComponent<Slider>().value = 0;
            PlayerPrefs.SetInt("sound", 1);
        }
        AudioSourceManager.Instance.OnClickBtnSound();
    }
    public void BGMSliderChange(float arg)
    {
        AudioManager.Instance.MusicChange(arg);
    }
    public void SoundEffectsSlider(float arg)
    {
        AudioManager.Instance.SoundChange(arg);
    }
    public void RestartGameClick()
    {
        Time.timeScale = 1;
        AudioSourceManager.Instance.OnClickBtnSound();
        SceneManager.LoadScene(Config.SCENE_GAME);
    }
    public void ReturnMenuClick()
    {
        Time.timeScale = 1;
        AudioSourceManager.Instance.OnClickBtnSound();
        SceneManager.LoadScene(Config.SCENE_MAIMANU);
    }
    public void ReturnGameClick()
    {
        Time.timeScale = 1;
        AudioSourceManager.Instance.OnClickBtnSound();
        gameObject.SetActive(false);
        RayCheckControl.Instance.isCheckSun = true;
    }
    public void WaiGua()
    {
        SunModle.Instance.ChangeSunNum(10000);
        ZombieSpawn.Instance.currentWave = 9;
        
    }
}
