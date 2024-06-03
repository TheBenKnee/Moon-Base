using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUIObject : MonoBehaviour
{
    private Action myAction;

    public void InitializeAction(Action action)
    {
        myAction = action;
    }

    public void ActionSelected()
    {
        myAction.Execute();
    }
}
