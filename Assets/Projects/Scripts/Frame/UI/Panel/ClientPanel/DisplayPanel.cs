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
    public Slider slider;

    public override void InitFind()
    {
        base.InitFind();
        slider = FindTool.FindChildComponent<Slider>(transform, "Slider");
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        IsRotate = false;
        ObjectManager.Instance.Cube.SetActive(true);
        ObjectManager.Instance.Cube.transform.localEulerAngles = new Vector3(-90, 0, 0);
        EventManager.AddUpdateListener(MTFrame.MTEvent.UpdateEventEnumType.Update, "DisplayUpdate", DisplayUpdate);
    }

    private Vector3 oldPos, newPos;
    private Vector3 oldBoatPos, newBoatPos;
    private float TimeInterval = 0.0f;

    public void ValueOnChange()
    {
        BoatRotateY boatRotateY = new BoatRotateY();
        boatRotateY.y = slider.value;
        UdpSclient.Instance.SendDataToSever(ParmaterCodes.BoatRotateY, boatRotateY);
    }

    private void DisplayUpdate(float timeProcess)
    {
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
                if (Mathf.Abs(newPos.x - oldPos.x) > Mathf.Abs(newPos.y - oldPos.y))
                {
                    if (newPos.x > oldPos.x)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(0, -RotateSpeed, 0,Space.World); 
                    }
                    else if (newPos.x < oldPos.x)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(0, RotateSpeed, 0, Space.World);
                    }
                    }
                else if (Mathf.Abs(newPos.x - oldPos.x) < Mathf.Abs(newPos.y - oldPos.y))
                {
                    if (newPos.y > oldPos.y)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(RotateSpeed, 0, 0, Space.World);
                        // ObjectManager.Instance.Cube.transform.localEulerAngles += new Vector3(RotateSpeed, 0, 0);
                    }
                    else if (newPos.y < oldPos.y)
                    {
                        ObjectManager.Instance.Cube.transform.Rotate(-RotateSpeed, 0, 0, Space.World);
                        //ObjectManager.Instance.Cube.transform.localEulerAngles += new Vector3(-RotateSpeed, 0, 0);
                    }
                }

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
}
