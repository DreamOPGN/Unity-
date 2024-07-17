using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
//using System.Net.NetworkInformation;
using System;

public class HotUpdateUIControl : MonoBehaviour
{
    

    private Slider slider;
    private Text tip;
    private Text intoText;
    private Button startDownLoadBtn;
    private Button noDownLoadBtn;
    private Button intoGameBtn;
    private GameObject promptBox; //��ʾ��

    private GameObject selectUpdate;
    private GameObject intoGame;
    void Start()
    {
        //PingNetAddress();
        slider = transform.Find("Slider").GetComponent<Slider>();
        promptBox = transform.Find("promptBox").gameObject;
        selectUpdate = promptBox.transform.Find("SelectUpdate").gameObject;
        intoGame = promptBox.transform.Find("IntoGame").gameObject;
        tip = transform.Find("tip").GetComponent<Text>();
        intoText = intoGame.transform.Find("intoText").GetComponent<Text>();
        startDownLoadBtn = promptBox.transform.Find("SelectUpdate/StartDownLoadBtn").GetComponent<Button>();
        startDownLoadBtn.onClick.AddListener(StartDownLoadBtn);
        noDownLoadBtn = promptBox.transform.Find("SelectUpdate/noDownLoadBtn").GetComponent<Button>();
        noDownLoadBtn.onClick.AddListener(IntoGame);
        intoGameBtn = promptBox.transform.Find("IntoGame/IntoGameBtn").GetComponent<Button>();
        intoGameBtn.onClick.AddListener(IntoGame);
        promptBox.SetActive(false);
        intoGame.SetActive(false);
        ABUpdateMgr.Instance.CheckUpdate((isOver) =>
        {

            if (isOver)
            {
                Debug.Log("�����½��������ؽ�����");
                startDownLoadBtn.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("���������С�ϵ�");
            }
        }, (str) =>
        {
            //�����ﴦ����½����ϵ���ʾ��Ϣ��ص��߼�
            tip.text = str;
            CompareTip(str);
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value < 1)
        {
            slider.value = Mathf.Lerp(slider.value, 1, 0.5f * Time.deltaTime);
        }
    }
    public void StartDownLoadBtn()
    {
        slider.value = 0;
        ABUpdateMgr.Instance.StartDownLoad((isOver) => 
        {
            //���سɹ�
            if (isOver)
            {
                //Debug.Log("��Դ�������!");
                tip.text = "��Դ�������!";
                slider.value = 1;
                selectUpdate.SetActive(false);
                intoGame.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("��Դ����ʧ�ܣ����Լ�Ѱ��ԭ��");
            }
        },
        (str) =>
        {
            tip.text = str;
        });
    }
    public void CompareTip(string str)
    {
        switch (str)
        {
            case "���������...":
                slider.value = 1;
                promptBox.gameObject.SetActive(true);
                selectUpdate.SetActive(true);
                promptBox.transform.DOScale(1, 0.5f);
                break;
            case "���������Դ...":

                slider.value = 1;
                promptBox.gameObject.SetActive(true);
                selectUpdate.SetActive(false);
                intoText.text = "�������°汾��";
                intoGame.SetActive(true);
                promptBox.transform.localScale = Vector3.one;
                break;
            default:
                break;
        }
    }

    public void IntoGame()
    {
        SceneManager.LoadScene(1);
    }

    //�жϵ�ǰ�Ƿ�����
/*    private void PingNetAddress()
    {
        try
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            PingReply pr = ping.Send("www.baidu.com", 3000);
            if (pr.Status == IPStatus.Success)
            {
                Debug.Log("���������ź�����");
            }
            else
            {
                Debug.Log("�����������ź�");
            }
        }
        catch (Exception e)
        {
            Debug.Log("���������ź��쳣" + e.Message);
        }
    }*/
}

