using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow : XRGrabInteractable
{
    //How fast arrow flies
    private float speed = 1000f;

    public Transform front = null;
    public LayerMask layermask = Physics.IgnoreRaycastLayer;
    public static float launchzone; //if the arrow is released from a distance far enough away from the target for scoring points

    private new Collider collider = null;
    private new Rigidbody rigidbody = null;
    
    private Vector3 lastposition = Vector3.zero; //the last position of the arrow
    public static bool launched = false; // if the arrow has been released from the notch and launched
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        // Do this first, so we get the right physics values
        if (args.interactor is XRDirectInteractor)
            Clear();

        // Make sure to do this
        base.OnSelectEntering(args);
    }

    void Clear() 
    {
       TogglePhysics(true);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Make sure to do this
        base.OnSelectExited(args);

        // If it's a notch, launch the arrow
        Debug.Log("notch  "+ args.interactor);
        if (args.interactor is Notch notch)
            Launch(notch);
    }

    void Launch(Notch notch) 
    {
        //Check to see if Bow is dropped while arrow is in socket
        if (notch.IsReady) 
        {
            SetLaunch(true);
            UpdateLastPosition();
            ApplyForce(notch.StringPull);
            launchzone = transform.position.z; 
        }
    }

    private void SetLaunch(bool value) 
    {
        collider.isTrigger = value;
        launched = value;
    }

    private void UpdateLastPosition() 
    {
        //Use the front of the arrow's position as last position
        lastposition = front.position;
    }

    void ApplyForce(StringPull pullstrength) 
    {
        //Apply the strength of the pull as a force to the arrow
        float power = pullstrength.PullAmount;
        Vector3 force = transform.forward * (power * speed);
        rigidbody.AddForce(force);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (launched)
        {
            // Check for collision as often as possible
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                UpdateLastPosition();
            }

            // Only set the direction with each physics update
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
                SetDirection();
        }
    }
    void SetDirection() 
    {
        //Check to see which direction the arrow is moving.
        if (rigidbody.velocity.z > 0.5f) 
        {
            transform.forward = rigidbody.velocity;
        } 
    }


    void TogglePhysics(bool value) 
    {
        //Disable the physics for child creation and grabbing arrows
        rigidbody.isKinematic = !value;
        rigidbody.useGravity = value;
    }
}
