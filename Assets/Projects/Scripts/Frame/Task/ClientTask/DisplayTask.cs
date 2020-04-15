using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class DisplayTask : BaseTask
{
    public DisplayTask(BaseState state) : base(state)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<DisplayPanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<DisplayPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
    }
}
