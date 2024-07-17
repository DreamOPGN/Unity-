using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image progressGrass;//进度条草皮
    public Image grass;//滚动的草团
    public Button continueBtn;
    public Text text;
    float ty;//草团调整高度

    float fullTime = 4f;//模拟量,模拟加载的总时间

    bool isFinish;
    float timer;//计时器

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
        text.text = "加载中.......";
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
        //加载进度
        operation = SceneManager.LoadSceneAsync(Config.SCENE_MAIMANU);
        //设置加载完毕,不进行马上跳转
        operation.allowSceneActivation = false;
        while(operation.progress < 0.9f)
        {
            OnProgress(operation.progress);
            yield return null;
        }
        Debug.Log("异步加载,加载完毕....");
        OnProgress(1);
        isFinish = true;
        grass.gameObject.SetActive(false);
        text.text = "点击进入游戏";
    }

    // Update is called once per frame
    void Update()
    {
        //异步加载,不进行模拟加载
        if (isAsync) return;

        //模拟进度条的运行
        timer += Time.deltaTime;//时间累计
        float p = timer / fullTime;//
        if(p>1)
        {
            //加载完毕
            isFinish = true;
            grass.gameObject.SetActive(false);
            text.text = "点击进入游戏";
            return;
        }

        OnProgress(p);
    }

    void OnProgress(float p)//0---1
    {
        progressGrass.fillAmount = p;
        
        if (p < 0.4f)//加载到60%以后,不再缩放
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
