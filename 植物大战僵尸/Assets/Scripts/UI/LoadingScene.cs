using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image progressGrass;//��������Ƥ
    public Image grass;//�����Ĳ���
    public Button continueBtn;
    public Text text;
    float ty;//���ŵ����߶�

    float fullTime = 4f;//ģ����,ģ����ص���ʱ��

    bool isFinish;
    float timer;//��ʱ��

   // public Image tips;

    public bool isAsync = false;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(800, 600, false);
        timer = 0;
        progressGrass.fillAmount = 0;
        ty = 0;
        isFinish = false;
        text.text = "������.......";
        grass.transform.localPosition = new Vector3(-277f, 40f, 0);
        grass.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        continueBtn.onClick.AddListener(ToNextScene);

        if(isAsync)
        {
            StartCoroutine(asyncChangeScene());
        }

       // tips.gameObject.SetActive(false);
        
       // AudioSource ads = GetComponent<AudioSource>();
       // AudioManager.Instance.PlayBGM("bgm4");
    }

    AsyncOperation operation;
    public IEnumerator asyncChangeScene()
    {
        //���ؽ���
        operation = SceneManager.LoadSceneAsync(Config.SCENE_MAIMANU);
        //���ü������,������������ת
        operation.allowSceneActivation = false;
        while(operation.progress < 0.9f)
        {
            OnProgress(operation.progress);
            yield return null;
        }
        Debug.Log("�첽����,�������....");
        OnProgress(1);
        isFinish = true;
        grass.gameObject.SetActive(false);
        text.text = "���������Ϸ";
    }

    // Update is called once per frame
    void Update()
    {
        //�첽����,������ģ�����
        if (isAsync) return;

        //ģ�������������
        timer += Time.deltaTime;//ʱ���ۼ�
        float p = timer / fullTime;//
        if(p>1)
        {
            //�������
            isFinish = true;
            grass.gameObject.SetActive(false);
            text.text = "���������Ϸ";
            return;
        }

        OnProgress(p);
    }

    void OnProgress(float p)//0---1
    {
        progressGrass.fillAmount = p;
        
        if (p < 0.4f)//���ص�60%�Ժ�,��������
        {
            grass.transform.localScale = new Vector3(1.2f - p, 1.2f - p, 1 - p);
            ty = 60.0f * p;
        }
        if(p > 0.5f)
        {
            grass.transform.localPosition = new Vector3(-277f + 570.0f * p, 40f - ty, 0);
        }
        else
        {
            grass.transform.localPosition = new Vector3(-277f + 547.0f * p, 40f - ty, 0);
        }


        grass.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -p * 360));
    }

    void ToNextScene()
    {
       // tips.gameObject.SetActive(true);

        if (isFinish)
        {
            if(isAsync)
            {
                operation.allowSceneActivation = true;
            }
            else
            {
                SceneManager.LoadScene(Config.SCENE_MAIMANU);
            }
        }
            
    }
}
