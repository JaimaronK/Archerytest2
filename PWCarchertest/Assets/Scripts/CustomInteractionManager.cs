using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class CustomInteractionManager : XRInteractionManager
{
    [System.Obsolete]
    public void ForceDeselect(XRBaseInteractor interactor)
    {
        if (interactor.selectTarget)
            SelectExit(interactor, interactor.selectTarget);
    }
}
