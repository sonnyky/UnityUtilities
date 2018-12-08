﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceSelector : MonoBehaviour {

    private WebCamDevice[] devices;

    // Use this for initialization
    void Start () {
        devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
            Debug.Log(devices[i].name);
    }
	
    public WebCamDevice GetDevice(int id)
    {

        if(id > devices.Length - 1)
        {
            Debug.LogError("Specified ID exceeds number of available devices");
            return devices[0];
        }
        return devices[id];
    }
    
}