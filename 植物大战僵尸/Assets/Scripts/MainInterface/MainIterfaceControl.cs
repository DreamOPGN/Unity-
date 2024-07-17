using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainIterfaceControl : MonoBehaviour
{
    public GameObject help;
    public GameObject quit;
    public GameObject dialog;
    public GameObject optionPanel;

    public GameObject achievement;
    public GameObject illustratedHandbook;

    public GameObject changeName;
    public new Text name;
    public Text input;
    public GameObject shop;



    void Start()
    {
        AudioManager.Instance.PlayBGM("bgm6");

        if (PlayerPrefs.GetString("name") == "")
        {
            name.text = "²»°®×öÓÎÏ·";
        }
        else
        {
            name.text = PlayerPrefs.GetString("name");
        }
    }


    public void QuitClick()
    {

        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
        Application.Quit();
    }
    public void HelpClick()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
        help.SetActive(true);
        help.transform.DOScale(0.5f, 0.5f);
    }
    public void HelpGoBack()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
        StartCoroutine(StartScana(help));
/*        help.SetActive(false);*/
    }
    public void OptionClick()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");

        dialog.SetActive(true);
        dialog.transform.DOScale(0.5f, 0.5f);
    }
    public void OptionGoBack()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
        StartCoroutine(StartScana(dialog));
/*        dialog.SetActive(false);*/
    }
    public void AchievementClick()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
    }
    public void IllustratedHandbookClick()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
    }
    public void ShopClick()
    {
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
    }
    public void BGMToggleChange(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.IsMusic = true;
            AudioManager.Instance.resumBGM();
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            AudioManager.Instance.IsMusic = false;
            AudioManager.Instance.pauseBGM();
            PlayerPrefs.SetInt("music", 1);
        }
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
    }
    public void SoundEffectsToggle(bool isOn)
    {
        if (isOn)
        {
            AudioManager.Instance.isSound = true;
            PlayerPrefs.SetInt("sound", 0);

        }
        else
        {
            AudioManager.Instance.isSound = false;
            PlayerPrefs.SetInt("sound", 1);
        }
        AudioManager.Instance.PlaySound(GameObject.Find("BackGround").GetComponent<AudioSource>(), "buttonclick");
    }

    public void ChangeName()
    {
        changeName.SetActive(true);
        changeName.transform.DOScale(1, 0.5f);
    }
    public void ConfirmNameClick()
    {
        if (input.text == "") return;
        name.text = input.text;
        PlayerPrefs.SetString("name", input.text);
        StartCoroutine(StartScana(changeName));
/*        changeName.transform.DOScale(0, 0.5f);
        changeName.SetActive(false);*/
    }

    public void BackClick()
    {
        StartCoroutine(StartScana(changeName));

    }
    IEnumerator StartScana(GameObject go)
    {
        go.transform.DOScale(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
    }
}
