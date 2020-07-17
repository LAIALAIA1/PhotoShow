using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageMnager : MonoBehaviour {

    public int SettingTime;

    public GameObject[] Panels;//

    public Camera MainCamera;

    private int TimingNum;
    public Text TimingText;//倒计时文本

    public Text Path;//路径文本
    public RawImage lastrawimage;//拍照成功展示
    public GameObject Panel3_BigPhoto;//拍照背景
    public GameObject qujingkuang;//取景框
    public Animation BigPhotoAnim;//拍照界面动画
    public GameObject TackaPicture;//拍照按钮

    public Image Choose_BigPhoto;//选择界面大照片


    public int MainTimingNum;//全场计时变量


    // Use this for initialization
    void Start () {
        TimingNum = 5;
        MainTimingNum = 0;
        StartCoroutine(MinTiming());//启动计时器
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //界面1点击回调
    public void Input_Panel1()
    {
        Panels[0].SetActive(false);//1关
        Panels[1].SetActive(true);//2开
        MainTimingNum = 0;//重置计数器
    }

    //界面2
    public void InputYes()//yes按钮回调
    {
        Panels[1].SetActive(false);//2关
        Panels[2].SetActive(true);//3开
        ResetPanel3();//初始化拍照界面
        MainTimingNum = 0;//重置计数器
    }
    public void InputNo()//No按钮回调
    {
        Panels[1].SetActive(false);//2关
        Panels[0].SetActive(true);//1开
        MainTimingNum = 0;//重置计数器
    }

    //界面3
    public void Input_ReSelect()//点击ReSelect(重新选景)
    {
        print("重新选景");
        Panels[2].SetActive(false);//3关
        Panels[1].SetActive(true);//2开
        MainTimingNum = 0;//重置计数器
    }

    public void Input_GoBack()//点击GoBack(返回)
    {
        Panels[2].SetActive(false);//3关
        Panels[0].SetActive(true);//1开
        MainTimingNum = 0;//重置计数器
    }
    
    public void Inptu_TakeaPicture()//点击拍照
    {
        print("拍照");
        //CaptureCamera(MainCamera, new Rect(0, 0, Screen.width, Screen.height));//调用截屏方法
        BigPhotoAnim.Play();//播放照片动画
        qujingkuang.SetActive(false);//隐藏取景框
        StartCoroutine(Timing());//倒计时
    }
    public void Inptu_ReMake()//点击重新拍照
    {
        print("重拍");
        ResetPanel3();//初始化拍照界面
        MainTimingNum = 0;//重置计数器
    }

    IEnumerator MinTiming()//全场计时器
    {
        yield return new WaitForSeconds(1);
        if (Panels[0].activeSelf)//界面一显示
        {
            MainTimingNum = 0;//重置计数器
        }
        else
        {
            MainTimingNum++;

            if (MainTimingNum >= SettingTime)//无操作30秒
            {
                if (Panels[1].activeSelf)//界面2正在显示
                {
                    Panels[1].SetActive(false);//2关
                    Panels[0].SetActive(true);//1开                    
                }
                else if (Panels[2].activeSelf)//界面3正在显示           
                {
                    Panels[2].SetActive(false);//3关
                    Panels[0].SetActive(true);//1开
                }               
            }
        }
        print(MainTimingNum);
        StartCoroutine(MinTiming());//调自己
    }

    public void ResetPanel3()//初始化拍照界面
    {
        qujingkuang.SetActive(true);//显示取景框
        lastrawimage.gameObject.SetActive(false);//隐藏截屏成功
        TackaPicture.SetActive(true);//显示拍照按钮
        TimingNum = 5;//计时器初始化
        TimingText.text = TimingNum.ToString();//文本赋值
        Panel3_BigPhoto.GetComponent<Image>().sprite = Choose_BigPhoto.GetComponent<Image>().sprite;//界面3拍照背景图与界面2同步
    }

    IEnumerator Timing()//拍照倒计时
    {
        MainTimingNum = 0;//重置计数器
        TimingText.gameObject.SetActive(true);//显示
        yield return new WaitForSeconds(1);
        TimingNum--;//计时减一
        TimingText.text = TimingNum.ToString();//文本赋值
        if (TimingNum > 0)
        {
            StartCoroutine(Timing());//递归
        }
        else if(TimingNum == 0)
        {
            yield return new WaitForSeconds(1);//1秒后
            TimingText.gameObject.SetActive(false);//隐藏
            CaptureCamera(MainCamera, new Rect(0, 0, Screen.width, Screen.height));//调用截屏方法           
            yield return null;
        }
    }


    //截屏
    //定义一个byte数组用于下边接收截屏读取的图片流
    byte[] bytes;
    Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        //Debug.Log(Application.dataPath);
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示          
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件  
        bytes = screenShot.EncodeToPNG();
        //string filename = Application.dataPath + "/Screenshot.png";
        //定义图片保存路径（下边这句中的System.DateTime.Now.ToString("yyyyMMddHHmmssffff"是获取系统时间，年月日时分秒）
        //这句是另外保存的一张图片，项目需求，一般用上边那句就可以了
        //string filename1 = "D://Important/DownLoad/ScreenShot/" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";

        //获取桌面路径
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        print(dir);
        string filename1 = dir+"/" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
        //string filename = "C:/Users/Administrator/Desktop/今日资源" + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename1, bytes);//创建一个新文件，在其中写入指定的字节数组，然后关闭该文件。 如果目标文件已存在，则覆盖该文件。        
        Debug.Log(string.Format("截屏成功: {0}", filename1));
        //到这里为止呢你已经成功截到一张图片啦W

        MainTimingNum = 0;//重置计数器
        ResetAni(BigPhotoAnim, "BigPhotoAnim");//初始化动画
        lastrawimage.GetComponent<RawImage>().texture = screenShot;//把截好的图显示到ui上
        lastrawimage.gameObject.SetActive(true);//显示截屏成功
        Path.text = filename1;//路径更新
        TackaPicture.SetActive(false);//隐藏拍照按钮
        
        return screenShot;

    }

    public void ResetAni(Animation ani, string name)//初始化动画到第一帧
    {
        AnimationState state = ani[name];
        ani.Play(name);
        state.time = 0;
        ani.Sample();
        state.enabled = false;
    }
}
