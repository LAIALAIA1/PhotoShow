using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose : MonoBehaviour {

    public GameObject Content;//滑动节点
    private RectTransform _Rect;//组件
    private bool ToUp;//向上
    private bool ToDown;//向下

    private float TargetPoint;//目标位置
    public float Speed;//滑动速度

    public GameObject frame;//边框
    public GameObject[] LittlePhotos;//小图片集合

    public GameObject BigPhoto;//大照片

    public GameObject _PageMnager;//物体

    // Use this for initialization
    void Start () {
        Speed = 300;       
    }
	
	// Update is called once per frame
	void FixedUpdate () {
         
        if (ToUp || ToDown)
        {
            _Rect = Content.GetComponent<RectTransform>();
            var a = _Rect.anchoredPosition.y;

            //var b = Mathf.Clamp(a, 0, 173);//限制大小
            if (ToDown && a < TargetPoint)//点击过down并且y值小于TargetPoint
            {
                _Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x, a += Speed*Time.deltaTime);
                if (a >= TargetPoint)//y值到位
                {
                    ToDown = false;
                }
            }
            if (ToUp && a > TargetPoint)//点击过up并且y值大于TargetPoint
            {
                _Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x, a -= Speed * Time.deltaTime);
                if (a <= TargetPoint)//y值到位
                {
                    ToUp = false;
                }
            }
        }
    }

    public void Resetframe(int i)//刷新边框
    {
        _PageMnager.GetComponent<PageMnager>().MainTimingNum = 0;//初始化计时器
        frame.transform.SetParent(LittlePhotos[i].transform);//设置父物体
        frame.transform.position = LittlePhotos[i].transform.position;//刷新位置

        BigPhoto.GetComponent<Image>().sprite = LittlePhotos[i].GetComponent<Image>().sprite;//大小图片的精灵同步

    }

    //点击5张小照片
    public void InputLittlePhoto1()
    {
        Resetframe(0);
    }
    public void InputLittlePhoto2()
    {
        Resetframe(1);
    }
    public void InputLittlePhoto3()
    {
        Resetframe(2);
    }
    public void InputLittlePhoto4()
    {
        Resetframe(3);
    }
    public void InputLittlePhoto5()
    {
        Resetframe(4);
    }




    public void InputUp()//点击up回调
    {
        _PageMnager.GetComponent<PageMnager>().MainTimingNum = 0;//初始化计时器
        _Rect = Content.GetComponent<RectTransform>();
        print("UP");
        if (_Rect.anchoredPosition.y - 172 < 1)//可下移空间小于170
        {
            TargetPoint = 0;//目标值设置为0
        }
        else
        {
            TargetPoint = _Rect.anchoredPosition.y - 172;//目标值设置为当前y值减去172
        }

        //print(_Rect.anchoredPosition.y+ "TargetPoint :"+ TargetPoint);
        ToUp = true;
    }

    public void InputDown()//点击Down回调
    {
        _PageMnager.GetComponent<PageMnager>().MainTimingNum = 0;//初始化计时器
        _Rect = Content.GetComponent<RectTransform>();
        print("DOWN");
        
        
        //print(_Rect.anchoredPosition.y + "TargetPoint :" + TargetPoint);
        if (_Rect.anchoredPosition.y + 172 > 173)//可上移数值小于172
        {
            TargetPoint = 173;//目标值设置为173
        }
        else
        {
            TargetPoint = _Rect.anchoredPosition.y + 172;//目标值设置为当前y+172
        }
        ToDown = true;
    }
}
