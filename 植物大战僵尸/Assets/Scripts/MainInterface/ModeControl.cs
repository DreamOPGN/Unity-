using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;

public class ModeControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    private PolygonCollider2D pc2d;
    private bool startCheck = false;
    private bool canCheck = true;

    public GameObject firstMode;
    public GameObject secondMode;
    public GameObject thirdMode;
    public GameObject fourthMode;

    public GameObject videoPlayer;
    public GameObject options;
    public GameObject shank;
    public GameObject playerScreen;
    public GameObject bgIm;

    bool isStartAddSpeed;
    float t;

    public void OnPointerEnter(PointerEventData eventData)
    {
        startCheck = true;
        // 创建一个PointerEventData
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        startCheck = false;
        firstMode.GetComponent<ModeView>().ChangeSpriteDark();
        secondMode.GetComponent<ModeView>().ChangeSpriteDark();
        thirdMode.GetComponent<ModeView>().ChangeSpriteDark();
        fourthMode.GetComponent<ModeView>().ChangeSpriteDark();
    }

    void Start()
    {
        Screen.SetResolution(800, 600, false);
        pc2d = gameObject.GetComponent<PolygonCollider2D>();
        raycaster = GameObject.Find("/Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("/EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {
        if (startCheck && canCheck)
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 将屏幕坐标转换为世界坐标
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero); // 在2D平面上进行射线检测
            if (hit.collider != null) // 检测射线是否与碰撞体相交
            {
                if (hit.collider.gameObject.name == "FirstMode")
                {
                    firstMode.GetComponent<ModeView>().ChangeSprite();
                    secondMode.GetComponent<ModeView>().ChangeSpriteDark();
                    thirdMode.GetComponent<ModeView>().ChangeSpriteDark();
                    fourthMode.GetComponent<ModeView>().ChangeSpriteDark();
                    if (Input.GetMouseButtonUp(0))
                    {
                        //firstMode.GetComponent<ModeView>().ChangeScene();
                        StartCoroutine(IntoAdventure());
                        firstMode.GetComponent<ModeView>().StartBlinkSprite();
                        startCheck = false;
                        canCheck = false;
                    }
                }
                else if (hit.collider.gameObject.name == "SecondMode")
                {
                    //secondMode.GetComponent<ModeView>().ChangeSprite();
                    firstMode.GetComponent<ModeView>().ChangeSpriteDark();
                    //thirdMode.GetComponent<ModeView>().ChangeSpriteDark();
                    //fourthMode.GetComponent<ModeView>().ChangeSpriteDark();
/*                    if (Input.GetMouseButtonUp(0))
                    {
                        secondMode.GetComponent<ModeView>().ChangeScene();
                        startCheck = false;
                        canCheck = false;

                    }*/
                }
                else if (hit.collider.gameObject.name == "ThirdMode")
                {
                    //thirdMode.GetComponent<ModeView>().ChangeSprite();
                    firstMode.GetComponent<ModeView>().ChangeSpriteDark();
/*                    secondMode.GetComponent<ModeView>().ChangeSpriteDark();
                    fourthMode.GetComponent<ModeView>().ChangeSpriteDark();*/
/*                    if (Input.GetMouseButtonUp(0))
                    {
                        thirdMode.GetComponent<ModeView>().ChangeScene();
                        startCheck = false;
                        canCheck = false;

                    }*/
                }
                else if (hit.collider.gameObject.name == "FourthMode")
                {
                    //fourthMode.GetComponent<ModeView>().ChangeSprite();
                    firstMode.GetComponent<ModeView>().ChangeSpriteDark();
                    //secondMode.GetComponent<ModeView>().ChangeSpriteDark();
                    //thirdMode.GetComponent<ModeView>().ChangeSpriteDark();
/*                    if (Input.GetMouseButtonUp(0))
                    {
                        fourthMode.GetComponent<ModeView>().ChangeScene();
                        startCheck = false;
                        canCheck = false;

                    }*/
                }
            }
            else
            {
                firstMode.GetComponent<ModeView>().ChangeSpriteDark();
/*                secondMode.GetComponent<ModeView>().ChangeSpriteDark();
                thirdMode.GetComponent<ModeView>().ChangeSpriteDark();
                fourthMode.GetComponent<ModeView>().ChangeSpriteDark();*/
            }
        }
        if (isStartAddSpeed)
        {
            t += Time.deltaTime / 2; // 根据持续时间计算插值系数
            float newSpeed = Mathf.Lerp(2, 5, t); // 计算插值结果
            videoPlayer.GetComponent<VideoPlayer>().playbackSpeed = newSpeed; // 应用插值结果

            // 可选：在达到结束值后重置插值参数
            if (videoPlayer.GetComponent<VideoPlayer>().playbackSpeed == 5)
            {
                isStartAddSpeed = false;
            }
        }

    }
    AsyncOperation operation;
    IEnumerator IntoAdventure()
    {
        operation = SceneManager.LoadSceneAsync(Config.SCENE_GAME);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {

            yield return null; 
        }
        StartCoroutine(IntoAdventureScene());
      
    }
    IEnumerator IntoAdventureScene()
    {
        options.transform.DOLocalMove(new Vector3(-455f, -379f, 0f), 1f);
        playerScreen.transform.DOLocalMove(new Vector3(-560f, 209f, 0f), 1f);
        bgIm.SetActive(false);
        transform.DOLocalMove(new Vector3(609f, -361f, 0f), 1f);
        yield return new WaitForSeconds(1f);
        shank.SetActive(true);
        //shank.transform.DOScale(1, 3f);
        isStartAddSpeed = true;
        yield return new WaitForSeconds(2f);
        shank.transform.DOScale(1, 2f);
        shank.GetComponent<Image>().DOFade(1, 1f);
        yield return new WaitForSeconds(2f);
        firstMode.GetComponent<ModeView>().StopBlink();
        Screen.SetResolution(1920, 1080, true);
        operation.allowSceneActivation = true;
    }
}

