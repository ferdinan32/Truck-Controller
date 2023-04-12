using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControllerNew : MonoBehaviour
{
    [SerializeField] Transform wheel;
    [SerializeField] HingeJoint joint;
    [SerializeField] bool gripped;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gripped) ResetJoint();
    }

    public void ReturnToOrPos()
    {
        var localY = wheel.localEulerAngles.y;
        localY = localY > 180 ? localY - 360 : localY;
        wheel.localEulerAngles = new(0f, Mathf.Lerp(localY, 0f, Time.deltaTime * 1f), 0f);
    }

    public void SelectEntered()
    {
        gripped = true;
        UnlockJoint();
    }

    public void SelectExited()
    {
        gripped = false;
    }

    public void ResetJoint()
    {
        var eulerAngles = wheel.localEulerAngles;
        var localY = eulerAngles.y >= 180f ? eulerAngles.y - 360f : eulerAngles.y;

        var limits = joint.limits;
        limits.min = Mathf.Lerp(localY, 0, Time.deltaTime * 1f);
        limits.max = Mathf.Lerp(localY, 0, Time.deltaTime * 1f);
        joint.limits = limits;
    }
    
    public void UnlockJoint()
    {
        var limits = joint.limits;
        limits.min = -179;
        limits.max = 179;
        joint.limits = limits;
    }
}
