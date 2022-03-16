using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(StringPull))]
public class Notch : XRSocketInteractor
{

    [Range(0, 1)] public float releaseThreshold = 0.25f;
    public StringPull StringPull { get; private set; } = null;
    public bool IsReady { get; private set; } = false;

    //Custom Manager
    private CustomInteractionManager CustomManager => interactionManager as CustomInteractionManager;

    protected override void Awake()
    {
        base.Awake();
        StringPull = GetComponent<StringPull>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        // Arrow is released once the puller is released
        StringPull.selectExited.AddListener(ReleaseArrow);

        // Move the point where the arrow is attached
        StringPull.Pulled.AddListener(MoveAttach);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StringPull.selectExited.RemoveListener(ReleaseArrow);
        StringPull.Pulled.RemoveListener(MoveAttach);
    }

    [System.Obsolete]
    public void ReleaseArrow(SelectExitEventArgs args)
    {
        // Only release if the target is an arrow using custom deselect
        if (selectTarget is Arrow && StringPull.PullAmount > releaseThreshold) 
        {
            CustomManager.ForceDeselect(this);
        }
    }

    public void MoveAttach(Vector3 pullPosition, float pullAmount)
    {
        // Move attach when bow is pulled, this updates the renderer as well
        Debug.Log("bow is being pulled");
        attachTransform.position = pullPosition;
    }

    [System.Obsolete]
    public void SetReady(BaseInteractionEventArgs args)
    {
        // Set the notch ready if bow is selected
        IsReady = args.interactable.isSelected;
    }

    [System.Obsolete]
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        // We check for the hover here too, since it factors in the recycle time of the socket
        // We also check that notch is ready, which is set once the bow is picked up
        return base.CanSelect(interactable) && CanHover(interactable) && IsArrow(interactable) && IsReady;
    }

    private bool IsArrow(XRBaseInteractable interactable)
    {
        // Simple arrow check, can be tag or interaction layer as well
        return interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        // Use instantaneous so it follows smoothly
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    // This enables the socket to grab the arrow immediately
    [System.Obsolete]
    public override bool requireSelectExclusive => false;
}
