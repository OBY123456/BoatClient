using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class DisplayPanel : BasePanel
{
    private BoatRotate boatRotate = new BoatRotate();
    private float RotateSpeed = 2.0f;

    private bool IsRotate = false;

    private bool IsPlay;

    public Button backButton, ResetButton;

    public Button[] VideoButton;

    public Sprite[] Video_Sprite_click, Video_Sprite_NotClick;
    public Sprite[] PlayButtonSprite;

    public Button VideoFinishButton, PlayButton, SlowButton, QuickButton;

    public Slider VideoSlider;
    private float CurrentVideoLenth = 0;
    private float[] VideoLenth = {
        54.0f,
        29.0f,
        55.0f,
        60.0f,
        32.0f,
    };
    private float ForwordTime = 1;

    public Color blue;

    public Text TrainText,Panel_TiltleText,VideoControl_TiltleText;
    public Image VideControl_TipImage, VideControl_Mask;


    public override void InitFind()
    {
        base.InitFind();
        backButton = FindTool.FindChildComponent<Button>(transform, "BackButtonGroup");
        ResetButton = FindTool.FindChildComponent<Button>(transform, "ResetButton");
        VideoButton = FindTool.FindChildNode(transform, "VideoPlayGroup").GetComponentsInChildren<Button>();
        VideoFinishButton = FindTool.FindChildComponent<Button>(transform, "VideoControl/VideoFinishButton");
        PlayButton = FindTool.FindChildComponent<Button>(transform, "VideoControl/PlayButton");
        SlowButton = FindTool.FindChildComponent<Button>(transform, "VideoControl/SlowButton");
        QuickButton = FindTool.FindChildComponent<Button>(transform, "VideoControl/QuickButton");
        VideoSlider = FindTool.FindChildComponent<Slider>(transform, "VideoControl/Slider");

        VideoControl_TiltleText = FindTool.FindChildComponent<Text>(transform, "VideoControl/VideoText");
        VideControl_TipImage = FindTool.FindChildComponent<Image>(transform, "VideoControl/TipImage");
        TrainText = FindTool.FindChildComponent<Text>(transform, "TrainButton/Text");
        Panel_TiltleText = FindTool.FindChildComponent<Text>(transform, "TiltleText");
        VideControl_Mask = FindTool.FindChildComponent<Image>(transform, "VideoControl/Mask");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        backButton.onClick.AddListener(() => {
            UdpSclient.Instance.SceneChange(SceneName.WaitScene,PanelName.WaitPanel);
        });

        ResetButton.onClick.AddListener(() => {
            ObjectManager.Instance.Cube.transform.localEulerAngles = Vector3.zero;
            SentRotateData(ObjectManager.Instance.Cube.transform);
        });

        for (int i = 0; i < VideoButton.Length; i++)
        {
            SetButtonOnclick(VideoButton[i], i);
        }

        VideoFinishButton.onClick.AddListener(() => {

            Display_PlayVideo display_PlayVideo = new Display_PlayVideo();
            display_PlayVideo.name = VideoName.结束.ToString(); ;
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.Display_PlayVideo, display_PlayVideo);
            VideControl_Mask.gameObject.SetActive(true);
            ResetVideoButtonSprite();
            IsPlay = false;
            SetSlider(1);
        });

        PlayButton.onClick.AddListener(() => {
            if(IsPlay)
            {
                SentVideoStateData(VideoControl.暂停);
                PlayButton.gameObject.GetComponent<Image>().sprite = PlayButtonSprite[1];
                IsPlay = false;
            }
            else
            {
                SentVideoStateData(VideoControl.播放);
                PlayButton.gameObject.GetComponent<Image>().sprite = PlayButtonSprite[0];
                IsPlay = true;
            }
        });

        QuickButton.onClick.AddListener(() => {
            if(VideoSlider.maxValue > 1)
            {
                VideoSlider.value += ForwordTime;
                if (VideoSlider.value > VideoSlider.maxValue)
                {
                    SentVideoStateData(VideoControl.重播);
                    VideoSlider.value = 0;
                }
                else
                {
                    SentVideoStateData(VideoControl.快进);
                }   
            }

        });

        SlowButton.onClick.AddListener(() => {
            if(VideoSlider.maxValue > 1)
            {
                VideoSlider.value -= ForwordTime;
                if (VideoSlider.value < 0)
                {
                    SentVideoStateData(VideoControl.重播);
                    VideoSlider.value = 0;
                }
                else
                {
                    SentVideoStateData(VideoControl.快退);
                }
            }

        });
    }

    private void SentVideoStateData(VideoControl state)
    {
        Display_VideoControl display_VideoControl = new Display_VideoControl();
        display_VideoControl.state = state.ToString();
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.Display_VideoControl, display_VideoControl);
    }

    private void SetButtonOnclick(Button button,int i)
    {
        button.onClick.AddListener(() => {

            if(VideoButton[i].transform.GetChild(0).GetComponent<Text>().color == blue)
            {
                return;
            }

            ResetVideoButtonSprite();
            VideoButton[i].gameObject.GetComponent<Image>().sprite = Video_Sprite_click[i];
            VideoButton[i].transform.GetChild(0).GetComponent<Text>().color = blue;

            VideoControl_TiltleText.text = VideoButton[i].transform.GetChild(0).GetComponent<Text>().text;
            VideControl_TipImage.sprite = Video_Sprite_NotClick[i];

            VideControl_Mask.gameObject.SetActive(false);

            CurrentVideoLenth = VideoLenth[i];

            VideoName panelName = (VideoName)Enum.Parse(typeof(VideoName), (i + 1).ToString());
            Display_PlayVideo display_PlayVideo = new Display_PlayVideo();
            display_PlayVideo.name = panelName.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.Display_PlayVideo, display_PlayVideo);
            SetSlider(CurrentVideoLenth);
            IsPlay = true;
        });
    }

    private void ResetVideoButtonSprite()
    {
        for (int i = 0; i < VideoButton.Length; i++)
        {
            VideoButton[i].gameObject.GetComponent<Image>().sprite = Video_Sprite_NotClick[i];
            VideoButton[i].transform.GetChild(0).GetComponent<Text>().color = Color.black;
        }
        PlayButton.gameObject.GetComponent<Image>().sprite = PlayButtonSprite[0];
    }

    private void SetSlider(float value)
    {
        VideoSlider.maxValue = value;
        VideoSlider.value = 0;
    }

    public override void Open()
    {
        base.Open();
        Reset();
        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "DisplayUpdate", DisplayUpdate);
    }

    private Vector3 oldPos, newPos;
    private Vector3 oldBoatPos, newBoatPos;
    private float TimeInterval = 0.0f;

    private void DisplayUpdate(float timeProcess)
    {

        if(IsPlay && VideoSlider.maxValue > 1)
        {
            VideoSlider.value += Time.deltaTime;
            if(VideoSlider.value >= CurrentVideoLenth)
            {
                VideoSlider.value = 0;
                SentVideoStateData(VideoControl.重播);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            newPos = Input.mousePosition;
            Ray mRay = Camera.main.ScreenPointToRay(newPos);
            RaycastHit mHit;
            if (Physics.Raycast(mRay, out mHit))
            {
                if (mHit.collider.name == ObjectManager.Instance.Cube.name)
                {
                    IsRotate = true;
                }
                else
                {
                    return;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(IsRotate)
            {
                IsRotate = false;
                TimeInterval = 0;
                oldPos = Vector3.zero;
                SentRotateData(ObjectManager.Instance.Cube.transform);
            }
        }

        if(IsRotate && IsOpen)
        {
            newPos = Input.mousePosition;
            newBoatPos = ObjectManager.Instance.Cube.transform.localEulerAngles;
            if (oldPos == Vector3.zero)
            {

            }
            else
            {
                TimeInterval += Time.deltaTime;
                //if (Mathf.Abs(newPos.x - oldPos.x) > Mathf.Abs(newPos.y - oldPos.y))
                //{
                    if (newPos.x > oldPos.x)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(0, -RotateSpeed, 0,Space.World); 
                    }
                    else if (newPos.x < oldPos.x)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(0, RotateSpeed, 0, Space.World);
                    }
                //}
                //else if (Mathf.Abs(newPos.x - oldPos.x) < Mathf.Abs(newPos.y - oldPos.y))
                //{
                //    if (newPos.y > oldPos.y)
                //    {
                //        ObjectManager.Instance.Cube.transform.Rotate(RotateSpeed, 0, 0, Space.World);
                //        // ObjectManager.Instance.Cube.transform.localEulerAngles += new Vector3(RotateSpeed, 0, 0);
                //    }
                //    else if (newPos.y < oldPos.y)
                //    {
                //        ObjectManager.Instance.Cube.transform.Rotate(-RotateSpeed, 0, 0, Space.World);
                //        //ObjectManager.Instance.Cube.transform.localEulerAngles += new Vector3(-RotateSpeed, 0, 0);
                //    }
                //}

                if(oldBoatPos == Vector3.zero)
                {
                    SentRotateData(ObjectManager.Instance.Cube.transform);
                }
                else if(newBoatPos != oldBoatPos)
                {
                    if(TimeInterval > 0.02f)
                    {
                        SentRotateData(ObjectManager.Instance.Cube.transform);
                        TimeInterval = 0;
                    }
                    
                }
                
                oldBoatPos = newBoatPos;
            }
            oldPos = newPos;
        }
    }

    public override void Hide()
    {
        base.Hide();
        ObjectManager.Instance.Cube.SetActive(false);
        EventManager.RemoveUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "DisplayUpdate", DisplayUpdate);
    }

    private void SentRotateData(Transform trans)
    {
        boatRotate.X = trans.localEulerAngles.x;
        boatRotate.Y = trans.localEulerAngles.y;
        boatRotate.Z = trans.localEulerAngles.z;

        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatRotate, boatRotate);
    }

    private void Reset()
    {
        IsRotate = false;
        IsPlay = false;

        TimeInterval = VideoSlider.value = 0;

        oldPos = newPos = oldBoatPos = newBoatPos = Vector3.zero;

        ObjectManager.Instance.Cube.SetActive(true);
        ObjectManager.Instance.Cube.transform.localEulerAngles = Vector3.zero;

        ResetVideoButtonSprite();
        VideoControl_TiltleText.text = "起吊系统";
        VideControl_Mask.gameObject.SetActive(true);
        VideControl_TipImage.sprite = Video_Sprite_NotClick[0];
    }
}
