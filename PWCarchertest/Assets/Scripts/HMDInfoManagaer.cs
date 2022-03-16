using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManagaer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is Device active: " + XRSettings.isDeviceActive);
        Debug.Log("Device name is: " + XRSettings.loadedDeviceName);
        if (!XRSettings.isDeviceActive) 
        {
            Debug.Log("No headset Detected");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay")) 
        {
            Debug.Log("Using Mock HMD");
        }
        else 
        {
            Debug.Log("We have a Headset " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
