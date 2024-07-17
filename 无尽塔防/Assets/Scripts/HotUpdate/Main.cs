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
                Debug.Log("检查更新结束，隐藏进度条");
            }
            else
            {
                Debug.Log("网络出错了小老弟");
            }
        }, (str) => 
        {
            //在这里处理更新界面上的显示信息相关的逻辑
            Debug.Log(str);
        });*/



        /*ABUpdateMgr.Instance.DownLoadABCompareFile((isOver) => {
            //我们把解析下载下来的AB包对比文件写成了一个方法
            //一定是下载成功过后 再去解析它
            if (isOver)
            {
                //解析AB包对比文件
                ABUpdateMgr.Instance.GetRemoteABCompareFileInfo();
                //下载AB包
                ABUpdateMgr.Instance.DownLoadABFile((isOver) => {
                    if (isOver)
                    {
                        print("所有AB包下载结束 ，继续处理逻辑");
                    }
                    else
                    {
                        print("下载失败，网络应该出现问题了，自己处理");
                    }
                }, (nowNum, maxNum) => {
                    print("下载进度" + nowNum + "/" + maxNum);
                });
            }
            //下载失败 就去处理失败的逻辑
            else
            {
                print("下载失败，网络应该出现问题了，自己处理");
            }
        });*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
