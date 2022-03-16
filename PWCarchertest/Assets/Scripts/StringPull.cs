using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class StringPull : XRBaseInteractable
{
    public class PullEvent : UnityEvent<Vector3, float> { }
    public PullEvent Pulled = new PullEvent();

    public Transform start = null;
    public Transform end = null;

    private float pullAmount = 0.0f;
    public float PullAmount => pullAmount;

    XRBaseInteractor pullinteractor = null;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Set interactor for measurement
        pullinteractor = args.interactor;
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Clear interactor, and reset pull amount for animation
        pullinteractor = null;

        // Reset everything
        SetPullValues(start.position, 0.0f);
    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            // Update pull values while the measurer is grabbed
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                PullCheck();
        }
    }
    void PullCheck()
    {
        //Usiing the interactor's position to calculate the amount
        Vector3 interactorPosition = pullinteractor.transform.position;
        //New Pull Value and where it is in the space
        float newPullAmount = CalculatePull(interactorPosition);
        Vector3 newPullPosition = CalculatePosition(newPullAmount);

        //Checking to see if an event is needed to be sent
        SetPullValues(newPullPosition, newPullAmount);
    }

    float CalculatePull(Vector3 pullPosition)
    {
        //Get the Direction and Length
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;

        //Pull Direction
        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        //Actual Distance
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        pullValue = Mathf.Clamp(pullValue, 0.0f, 1.0f);

        return pullValue;
    }

    Vector3 CalculatePosition(float amount)
    {
        //Find the position of the Hand
        return Vector3.Lerp(start.position, end.position, amount);
    }

    void SetPullValues(Vector3 newPullPosition, float newPullAmount)
    {
        //check for a new value
        if (newPullAmount != pullAmount)
        {
            pullAmount = newPullAmount;
            Pulled?.Invoke(newPullPosition, newPullAmount);
        }
    }

    [System.Obsolete]
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        // Only let direct interactors pull the string
        return base.IsSelectableBy(interactor) && IsDirectInteractor(interactor);
    }

    private bool IsDirectInteractor(XRBaseInteractor interactor)
    {
        return interactor is XRDirectInteractor;
    }
    private void OnDrawGizmos()
    {
        // Draw line from start to end point
        if (start && end)
            Gizmos.DrawLine(start.position, end.position);
    }
}
