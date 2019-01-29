using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAlwaysMoving : GenericHammerLike
{
   

    public override void InitializeMe (float waitingTime , float riseTime) {
        canFall = true;
        base.InitializeMe (waitingTime , riseTime);
        ResetMe (); 
    }

}
