using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightFlicker : MonoBehaviour {

    private float MAX_ON_TIME = 20f;
    private float MAX_OFF_TIME = 0.5f;

    public Light streetLight;

    private float toggleTime = -1;

    private void Start()
    {
        toggleTime = Time.time + Random.Range(0f, MAX_ON_TIME);
        if (Random.Range(0f, 1f) < 0.6f){
            enabled = false;
        }else if (Random.Range(0f, 1f) < 0.1f){
            MAX_ON_TIME = 1f;
        }
    }

    void Update () {
        if(Time.time > toggleTime){
            var wasOn = streetLight.enabled;
            streetLight.enabled = !streetLight.enabled;
            toggleTime = Time.time + Random.Range(0f, wasOn ? MAX_OFF_TIME : MAX_ON_TIME);
        }
	}
}
