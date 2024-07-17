using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunModle : MonoBehaviour
{
    private static SunModle instance;
    public static SunModle Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<SunModle>();
            }
            return instance;
        }
    }

    private Text sunText;
    private float sunNum;
    //private AudioSource getSun;
    private void Awake()
    {
        instance = this;
        sunText = gameObject.GetComponent<Text>();
        sunNum = 50f;
        sunText.text = sunNum.ToString();
        //getSun = GameObject.Find("GetSun").GetComponent<AudioSource>();
    }

    public void ChangeSunNum(float num)
    {
/*        if(num > 0)
        {
            getSun.Play();
        }*/
        sunNum += num;
        SunIsChange();
        sunText.text = sunNum.ToString();
    }
    public float GetSunNum()
    {
        return sunNum;
    }
    public void SunIsChange()
    {
        PlantCardsControl.Instance.SunIsChange();
    }
}
