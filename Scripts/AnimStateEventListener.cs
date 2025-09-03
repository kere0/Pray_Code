using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateEventListener : MonoBehaviour
{
    public Action<string> OnOccursAnimStateEvent;

    public void OccursAnimStateEvent(string stateName)
    {
        OnOccursAnimStateEvent?.Invoke(stateName);
    }
}
