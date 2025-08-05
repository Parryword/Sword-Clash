namespace Animation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DashBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Globals.player.playerState = 0;
            Globals.player.keyDisabled = false;
            Globals.player.animationDisabled = false;
        }
    }

}