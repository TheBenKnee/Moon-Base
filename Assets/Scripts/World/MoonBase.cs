using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoonBaseStatus
{
    Unopened,
    Opened,
    Sealed
}

public class MoonBase : MonoBehaviour
{
    private MoonBaseStatus myMoonBaseStatus = MoonBaseStatus.Unopened;
    

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    // GET/SET Methods
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public MoonBaseStatus GetMoonBaseStatus()
    {
        return myMoonBaseStatus;
    }

    public void SetMoonBaseStatus(MoonBaseStatus newMoonBaseStatus)
    {
        myMoonBaseStatus = newMoonBaseStatus;
    }
}
