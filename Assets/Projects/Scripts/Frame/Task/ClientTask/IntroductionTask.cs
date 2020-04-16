using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class IntroductionTask : BaseTask
{
    public IntroductionTask(BaseState state) : base(state)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<IntroductionPanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<IntroductionPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
    }
}
