using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Globals.player.playerState = 0;
        Globals.player.keyDisabled = false;
        Globals.player.animationDisabled = false;
    }
}
