using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPage : MonoBehaviour {


    public GameObject Photos;//滚动条

    public GameObject[] BGs;//背景图片
    private float BGsSpeed;//背景渐变速度

    private float UvSpeed;//uv速度
    private int BGsIndex;//索引
    private bool ToChange;//

    private Vector2 Photos_uvRect;//组件
    private float x;

    private void OnEnable()
    {
        print("OnEnable");
        StartCoroutine(ChangeTime());//计时协程
    }

    // Use this for initialization
    void Start () {
        BGsIndex = 0;
        BGsSpeed = 1;
        UvSpeed = 0.05f;
        x = Photos.GetComponent<RawImage>().uvRect.x;
        print(Photos.GetComponent<RawImage>().uvRect);
        //
        //StartCoroutine(UVRun());//调用协程
        //StartCoroutine(ChangeTime());//计时协程
    }



    private void FixedUpdate()
    {
        UVRun();

        //print(BGs[0].GetComponent<Image>().color);
       
        BGsChange();

    }
    // Update is called once per frame
    void Update () {

        //var _RawImage = Photos.GetComponent<RawImage>();
        //_RawImage.uvRect = new Rect(x+= UvSpeed*Time.deltaTime, _RawImage.uvRect.y, _RawImage.uvRect.width, _RawImage.uvRect.height);
    }


    public void UVRun()//uv滚动
    {

        var _RawImage = Photos.GetComponent<RawImage>();//获取RawImage
        _RawImage.uvRect = new Rect(x -= UvSpeed * Time.deltaTime, _RawImage.uvRect.y, _RawImage.uvRect.width, _RawImage.uvRect.height);//滚动uv
    }

    public void BGsChange()//背景渐变
    {
        if (ToChange == false)
        {
            return;
        }
        else
        {
            var a1 = BGs[BGsIndex].GetComponent<Image>().color.a;
            if (a1 >= 0)
            {
                BGs[BGsIndex].GetComponent<Image>().color = new Color(255, 255, 255, a1 -= BGsSpeed * Time.deltaTime);
            }
            else
            {
                BGsIndex++;
                if (BGsIndex == 5)//索引超限
                {
                    BGsIndex = 0;//重置索引
                }
                ToChange = false;//渐变结束就把状态改回去
            }

            if (BGsIndex == 4)//如果到最后一张了
            {
                var a2 = BGs[BGsIndex - 4].GetComponent<Image>().color.a;
                if (a2 <= 1)
                {
                    BGs[BGsIndex - 4].GetComponent<Image>().color = new Color(255, 255, 255, a2 += BGsSpeed * Time.deltaTime);
                }
            }
            else
            {
                var a2 = BGs[BGsIndex + 1].GetComponent<Image>().color.a;
                if (a2 <= 1)
                {
                    BGs[BGsIndex + 1].GetComponent<Image>().color = new Color(255, 255, 255, a2 += BGsSpeed * Time.deltaTime);
                }
            }
            //print("透明度"+BGs[BGsIndex].GetComponent<Image>().color.a);//
        }
        
        
    }


    IEnumerator ChangeTime()
    {
        yield return new WaitForSeconds(4);
        ToChange = true;//可以切换
       


        StartCoroutine(ChangeTime());//调自己


        print(BGsIndex);
       
    }
}
