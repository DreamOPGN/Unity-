using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        
        /*ABUpdateMgr.Instance.CheckUpdate((isOver) => 
        {

            if (isOver)
            {
                Debug.Log("�����½��������ؽ�����");
            }
            else
            {
                Debug.Log("���������С�ϵ�");
            }
        }, (str) => 
        {
            //�����ﴦ����½����ϵ���ʾ��Ϣ��ص��߼�
            Debug.Log(str);
        });*/



        /*ABUpdateMgr.Instance.DownLoadABCompareFile((isOver) => {
            //���ǰѽ�������������AB���Ա��ļ�д����һ������
            //һ�������سɹ����� ��ȥ������
            if (isOver)
            {
                //����AB���Ա��ļ�
                ABUpdateMgr.Instance.GetRemoteABCompareFileInfo();
                //����AB��
                ABUpdateMgr.Instance.DownLoadABFile((isOver) => {
                    if (isOver)
                    {
                        print("����AB�����ؽ��� �����������߼�");
                    }
                    else
                    {
                        print("����ʧ�ܣ�����Ӧ�ó��������ˣ��Լ�����");
                    }
                }, (nowNum, maxNum) => {
                    print("���ؽ���" + nowNum + "/" + maxNum);
                });
            }
            //����ʧ�� ��ȥ����ʧ�ܵ��߼�
            else
            {
                print("����ʧ�ܣ�����Ӧ�ó��������ˣ��Լ�����");
            }
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
