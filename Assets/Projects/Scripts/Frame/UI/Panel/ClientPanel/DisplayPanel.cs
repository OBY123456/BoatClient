using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class DisplayPanel : BasePanel
{
    public Slider slider_x, slider_y,slider_z;

    private BoatRotateX rotateDataX = new BoatRotateX();
    private BoatRotateY rotateDataY = new BoatRotateY();
    private BoatRotateZ rotateDataZ = new BoatRotateZ();

    public override void InitFind()
    {
        base.InitFind();
        slider_x = FindTool.FindChildComponent<Slider>(transform, "Slider");
        slider_y = FindTool.FindChildComponent<Slider>(transform, "Slider (1)");
        slider_z = FindTool.FindChildComponent<Slider>(transform, "Slider (2)");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        //slider_x.onValueChanged.AddListener((float value) => SliderX(value));
        //slider_y.onValueChanged.AddListener((float value) => SliderY(value));
        //slider_z.onValueChanged.AddListener((float value) => SliderZ(value));
    }

    public void SliderX()
    {
        rotateDataX.X = slider_x.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatRotateX, rotateDataX);
    }

    public void SliderY()
    {
        rotateDataY.Y = slider_y.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatRotateY, rotateDataY);
    }

    public void SliderZ()
    {
        rotateDataZ.Z = slider_z.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatRotateZ, rotateDataZ);
    }

    public override void Open()
    {
        base.Open();
        Reset();
    }

    private void Reset()
    {
        slider_x.value = slider_y.value = slider_z.value = 0;
    }
}
