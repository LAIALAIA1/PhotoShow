using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MT_Camera : MonoBehaviour
{
    public GameObject raw_Image;
    RawImage cam_Video;
    //Toggle tog_togCamera;
    WebCamTexture camTexture;
    public int w_cam = 640;
    public int h_cam = 480;

    private void Awake()
    {
        //uiRoot = GameObject.Find("Canvas_UI").transform;
        //camRoot = GameObject.Find("Canvas_WebCam").transform;
    }
    private void OnEnable()
    {
        //cam_Video = raw_Image.GetComponent<RawImage>();
        //changeCam(false);
    }
    private void Start()
    {
        cam_Video = raw_Image.GetComponent<RawImage>();
        changeCam(false);


        //tog_togCamera = uiRoot.Find("tog_ChangeCam").GetComponent<Toggle>();
        //transform.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        //tog_togCamera.onValueChanged.AddListener(changeCam);
        //tog_togCamera.isOn = true;
        ///自适应屏幕分辨率显示摄像头数据
        //宽度不变，缩放高度自适应显示摄像头数据
        //cam_Video.rectTransform.sizeDelta = new Vector2(h_cam * Screen.height / w_cam, Screen.width);
        //宽度不变，缩放宽度自适应显示摄像头数据
        //cam_Video.rectTransform.sizeDelta = new Vector2(Screen.height, w_cam * Screen.width / h_cam);
    }
    void changeCam(bool isOn)
    {
        StartCoroutine(CallCamera(isOn));
    }
    IEnumerator CallCamera(bool isOn)
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (camTexture != null)
                camTexture.Stop();
            WebCamDevice[] cameraDevices = WebCamTexture.devices;
            string deviceName = "";
            for (int i = 0; i < cameraDevices.Length; i++)
            {
                //如果是前置摄像机  
                if (WebCamTexture.devices[i].isFrontFacing && isOn)
                {
                    deviceName = WebCamTexture.devices[i].name;
                    TurnCam(isOn);
                    break;
                }
                //如果是后置摄像机  
                else if (!WebCamTexture.devices[i].isFrontFacing && !isOn)
                {
                    deviceName = WebCamTexture.devices[i].name;
                    TurnCam(isOn);
                    break;
                }
            }
            camTexture = new WebCamTexture(deviceName, w_cam, h_cam, 12);
            cam_Video.texture = camTexture;
            print("**********"+cam_Video.texture.width+ "**********" + cam_Video.texture.height);
            camTexture.Play();
        }
    }
    /// <summary>
    /// 翻转plane,正确显示摄像头数据
    /// </summary>
    /// <param name="isOn">If set to <c>true</c> is turn.</param>
    public void TurnCam(bool isOn)
    {
#if UNITY_IOS || UNITY_IPHONE
        if (!isOn)
            cam_Video.rectTransform.localEulerAngles = new Vector3(180, 0, 90);
        else cam_Video.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
#elif UNITY_ANDROID
        if (!isOn)
            cam_Video.rectTransform.localEulerAngles = new Vector3(180, 180, 90);
        else cam_Video.rectTransform.localEulerAngles = new Vector3(0, 180, 90);
#endif
    }
}