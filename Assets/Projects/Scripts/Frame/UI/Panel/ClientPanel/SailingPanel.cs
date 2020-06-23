using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class SailingPanel : BasePanel
{
    public Button BackButton;

    public TrainButton[] TrainButtons;

    public Text TiltleText;

    public ZhuanChangPanel ZhuanChangPanel;
    public PuGuanPanel PuguanPanel;
    public DiaoZhuangPanel DiaozhuangPanel;


    public override void InitFind()
    {
        base.InitFind();

        TrainButtons = FindTool.FindChildNode(transform, "TrainModelButtonGroup").GetComponentsInChildren<TrainButton>();

        TiltleText = FindTool.FindChildComponent<Text>(transform, "TiltleText");

        ZhuanChangPanel = FindTool.FindChildComponent<ZhuanChangPanel>(transform, "ZhuanChangPanel");

        PuguanPanel = FindTool.FindChildComponent<PuGuanPanel>(transform, "PuGuanPanel");

        DiaozhuangPanel = FindTool.FindChildComponent<DiaoZhuangPanel>(transform, "DiaoZhuangPanel");

    }

    public override void InitEvent()
    {
        base.InitEvent();

        for (int i = 0; i < TrainButtons.Length; i++)
        {
            InitTrainButtons(TrainButtons[i].button, i);
        }

        TrainButtons[0].button.onClick.AddListener(() => {
            PuguanPanel.Hide();
            ZhuanChangPanel.MaskHide();
            if (DiaozhuangPanel.IsOpen)
            {
                DiaozhuangPanel.Hide();
            }
        });

        TrainButtons[1].button.onClick.AddListener(() => {
            PuguanPanel.Open();
            ZhuanChangPanel.MaskOpen();
            if (DiaozhuangPanel.IsOpen)
            {
                DiaozhuangPanel.Hide();
            }
        });

        TrainButtons[2].button.onClick.AddListener(() => {
            PuguanPanel.Hide();
            ZhuanChangPanel.MaskOpen();
            ZhuanChangPanel.Reset();
            PuguanPanel.Reset();
            if(!DiaozhuangPanel.IsOpen)
            {
                DiaozhuangPanel.Open();
            }
        });

        BackButton.onClick.AddListener(() =>
        {
            UdpSclient.Instance.SceneChange(SceneName.WaitScene, PanelName.WaitPanel);
        });

    }

    public override void Open()
    {
        base.Open();
        Reset();
        
        ZhuanChangPanel.Reset();
    }

    public override void Hide()
    {
        base.Hide();
        //ZhuanChangPanel.Hide();
    }

    private void InitTrainButtons(Button button,int i)
    {
        button.onClick.AddListener(() => {

            RestTrainButtonColor();
            TrainButtons[i].OnClick();

            TiltleText.text = TrainButtons[i].text.text;

            TrainModelData data = new TrainModelData();
            TrainModel trainModel = (TrainModel)Enum.ToObject(typeof(TrainModel), i);
            data.trainModel = trainModel.ToString();
            UdpSclient.Instance.SendDataToSever(ParmaterCodes.TrainModelData, data);
        });
    }

    private void RestTrainButtonColor()
    {
        foreach (TrainButton item in TrainButtons)
        {
            item.Reset();
        }
    }

    private void Reset()
    {

        RestTrainButtonColor();
        TrainButtons[0].OnClick();
        PuguanPanel.Reset();
        DiaozhuangPanel.Hide();
        ZhuanChangPanel.Open();
    }
}
