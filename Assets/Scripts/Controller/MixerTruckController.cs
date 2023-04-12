using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerTruckController : MonoBehaviour
{
    [SerializeField] CementController cementController;
    [SerializeField] LampClass lampClass;
    [SerializeField] PartClass partClass;
    [SerializeField] AudioClass audioClass;

    [Serializable]
    public class PartClass
    {
        public Transform mixerTube;
        public Transform[] wheelMeshs;
        public WheelCollider[] wheels;
    }

    [Serializable]
    public class LampClass
    {
        public GameObject roomLamp;
        public GameObject frontLamp;
        public GameObject workingLamp;
        public GameObject brakeSignal;
        public GameObject[] leftTurnSignals;
        public GameObject[] rightTurnSignals;
    }

    [Serializable]
    public class AudioClass
    {
        public AudioSource engineSound;
        public AudioClip engineStartClip;
        public AudioClip engineShootdownClip;
        public AudioClip turnSignalOnClip;
        public AudioClip turnSignalOffClip;
    }

#region Private Variable
    private bool? isMixingRight;
    private bool isMixing = false;
    private bool isRoomLampOn = true;
    private bool isFrontLampOn = true;
    private bool isWorkingLamp = true;
    private bool isEngineStart = false;
    private bool isGearNetral = true;
    private bool isGearRear = false;
    private bool isTurnRight = false;
    private bool isTurnLeft = false;
    private bool isTurnWarning = false;
    private int defaultIndex = 0;
    private int frontWheelIndex = 1;
    private float minGearShift = 1;
    private float maxGearShift = 5;
    private float angleRotate = 360;
    private float timePerSecond = 60;
    private float acceleration = 10;
    private float brakingForce = 50;
    private float maxTurnAngle = 30;
    private float mixingSpeed = 1;
    private float minMixingSpeed = 1;
    private float maxMixingSpeed = 4;
    private float delaySignal = .5f;
    private float pitchOffside = .5f;
    private float maxEnginePitch = 2;
    private float enginePitch;
    private float moveDirection;
    private float mixerDirection;
    private float currentMixerDirection;
    private float currentGearShift;
    private float currentAcceleration;
    private float currentBrakingForce;
    private float currentTurnAngle;
    private KeyboardInput keyboardInput;
    private IEnumerator signalLampIEnumerator;
    private List<IEnumerator> signalLampIEnumerators;
#endregion Private Variable

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        keyboardInput = GetComponent<KeyboardInput>();
        signalLampIEnumerators = new List<IEnumerator>();
        currentGearShift = minGearShift;
        SetAllMainLamp();
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardFunction();
        EngineSoundPitch();
        EngineForce();
        MixerRotating();
        WheelRotating();
        Braking();
        Acceleration();
    }

    void KeyboardFunction()
    {
        if(keyboardInput.Keycode_EngineStart)
        {
            EngineStart();
        }

        if(keyboardInput.Keycode_ToggleLight)
        {
            TurnOnLamp(lampClass.roomLamp);
        }

        if(keyboardInput.Keycode_RevolvingLamp)
        {
            TurnOnLamp(lampClass.frontLamp);
        }

        if(keyboardInput.Keycode_WorkingLamp)
        {
            TurnOnLamp(lampClass.workingLamp);
        }

        if(keyboardInput.Keycode_GearUp)
        {
            GearShiftUp();
        }

        if(keyboardInput.Keycode_GearDown)
        {
            GearShiftDown();
        }

        if(keyboardInput.Keycode_TurnSignalLeft)
        {
            TurnSignalLamp(isRight: false);
        }

        if(keyboardInput.Keycode_TurnSignalRight)
        {
            TurnSignalLamp(isRight: true);
        }

        if(keyboardInput.Keycode_TurnSignalWarning)
        {
            TurnSignalWarning();
        }

        if(keyboardInput.Keycode_Space)
        {
            MixerPouring();
        }

        if(keyboardInput.Keycode_MixerActive)
        {
            MixerActive();
        }

        if(keyboardInput.Keycode_MixerRight)
        {
            MixerDirection(isRight: true);
        }

        if(keyboardInput.Keycode_MixerLeft)
        {
            MixerDirection(isRight: false);
        }

        if(keyboardInput.Keycode_ChangeMaterial)
        {
            if(!isMixing)
            {
                cementController.SetMaterial();
            }
        }

        if(isEngineStart)
        {
            if(keyboardInput.Keycode_HornSwitch)
            {
                HornSwitch();
            }
        }
    }

    void MixerPouring()
    {
        cementController.IsStartPour = true;
        cementController.IsPouring = !cementController.IsPouring;
    }

    void MixerActive()
    {
        isMixing = !isMixing;

        if(mixerDirection == Vector3.zero.x)
            mixerDirection = angleRotate;
    }

    void MixerRotating()
    {
        if(isMixing)
        {
            currentMixerDirection = mixerDirection * ((minMixingSpeed/maxMixingSpeed) * mixingSpeed);
            partClass.mixerTube.Rotate (Vector3.zero.x, currentMixerDirection * Time.deltaTime, Vector3.zero.z);
        }
    }

    void MixerDirection(bool isRight)
    {
        if(!isMixing)
            MixerActive();

        if(isMixingRight == isRight)
        {
            mixingSpeed++;

            if(mixingSpeed > maxMixingSpeed)
                mixingSpeed = minMixingSpeed;
        }
        else
        {
            isMixingRight = isRight;
            mixingSpeed = minMixingSpeed;
        }

        if(isRight)
            mixerDirection = angleRotate;
        else
            mixerDirection = -(angleRotate);
    }

    void Acceleration()
    {
        if(!isEngineStart)
            return;

        if(isGearRear)
            moveDirection = -(Mathf.Abs(keyboardInput.Keycode_Acceleration));
        else
            moveDirection = Mathf.Abs(keyboardInput.Keycode_Acceleration);

        if(isGearNetral)
            currentAcceleration = Vector3.zero.x;
        else
            currentAcceleration = acceleration * currentGearShift * moveDirection;
    }

    void Braking()
    {
        bool isBraking = false;
        currentBrakingForce = brakingForce * keyboardInput.Keycode_Brake;

        if(keyboardInput.Keycode_Brake > Vector3.zero.x)
            isBraking = true;

        lampClass.brakeSignal.SetActive(isBraking);
    }

    void WheelRotating()
    {
        float offside = Vector3.zero.x;

        if(keyboardInput.Keycode_Steering < offside && keyboardInput.Keycode_Steering > Vector3.left.x)
        {
            offside = Vector3.left.x - keyboardInput.Keycode_Steering;
        }
        else if(keyboardInput.Keycode_Steering > offside && keyboardInput.Keycode_Steering < Vector3.right.x)
        {
            offside = Vector3.right.x - keyboardInput.Keycode_Steering;
        }

        currentTurnAngle = maxTurnAngle * (keyboardInput.Keycode_Steering + offside);

        for (int i = defaultIndex; i < partClass.wheelMeshs.Length; i++)
        {
            if(i > frontWheelIndex)
            {
                partClass.wheelMeshs[i].Rotate(Vector3.zero.x, Vector3.zero.y, WheelRotation());
            }
            else
            {
                partClass.wheelMeshs[i].Rotate(WheelRotation(), Vector3.zero.y, Vector3.zero.z);
                partClass.wheels[i].transform.localEulerAngles = new Vector3(Vector3.zero.x, currentTurnAngle, Vector3.zero.z);
            }
        }
    }

    float WheelRotation()
    {
        return (partClass.wheels[defaultIndex].rpm / timePerSecond * angleRotate * Time.deltaTime);
    }

    void TurnOnLamp(GameObject lamp)
    {
        bool isOn = false;

        if(lamp == lampClass.roomLamp)
        {
            isRoomLampOn = !isRoomLampOn;
            isOn = isRoomLampOn;
        }
        else if(lamp == lampClass.frontLamp)
        {
            isFrontLampOn = !isFrontLampOn;
            isOn = isFrontLampOn;
        }
        else if(lamp == lampClass.workingLamp)
        {
            isWorkingLamp = !isWorkingLamp;
            isOn = isWorkingLamp;
        }

        lamp.SetActive(isOn);
    }

    void SetAllMainLamp()
    {
        SetSignalLamp();
        TurnOnLamp(lampClass.roomLamp);
        TurnOnLamp(lampClass.frontLamp);
        TurnOnLamp(lampClass.workingLamp);
    }

    void TurnSignalLamp(bool isRight)
    {
        TurnOffWarning();

        if(isRight)
        {
            isTurnLeft = false;
            isTurnRight = !isTurnRight;
            signalLampIEnumerator = IESignalLamp(lampClass.rightTurnSignals);
            SetSignalLamp();

            if(isTurnRight)
                StartCoroutine(signalLampIEnumerator);
        }
        else
        {
            isTurnRight = false;
            isTurnLeft = !isTurnLeft;
            signalLampIEnumerator = IESignalLamp(lampClass.leftTurnSignals);
            SetSignalLamp();

            if(isTurnLeft)
                StartCoroutine(signalLampIEnumerator);
        }
    }

    void SetSignalLamp()
    {
        for (int i = defaultIndex; i < lampClass.rightTurnSignals.Length; i++)
        {
            lampClass.rightTurnSignals[i].SetActive(isTurnRight);
            lampClass.leftTurnSignals[i].SetActive(isTurnLeft);
        }
    }

    void TurnSignalWarning()
    {
        isTurnWarning = !isTurnWarning;

        if(isTurnWarning)
            TurnOnWarning();
        else
            TurnOffWarning();
    }

    void TurnOnWarning()
    {
        isTurnLeft = true;
        isTurnRight = true;

        if(signalLampIEnumerator != null)
            StopCoroutine(signalLampIEnumerator);

        signalLampIEnumerators.Add(IESignalLamp(lampClass.rightTurnSignals));
        signalLampIEnumerators.Add(IESignalLamp(lampClass.leftTurnSignals));

        foreach (var item in signalLampIEnumerators)
        {
            StartCoroutine(item);
        }
    }

    void TurnOffWarning()
    {
        isTurnWarning = false;

        if(signalLampIEnumerator != null)
            StopCoroutine(signalLampIEnumerator);

        if(signalLampIEnumerators.Count > defaultIndex)
        {
            foreach (var item in signalLampIEnumerators)
            {
                if(item != null)
                    StopCoroutine(item);
            }

            signalLampIEnumerators.Clear();
        }

        if(isTurnLeft && isTurnRight)
        {
            isTurnLeft = false;
            isTurnRight = false;
            SetSignalLamp();
        }
    }

    IEnumerator IESignalLamp(GameObject[] signalLamps)
    {
        while (true)
        {
            SetSignalWarning(true, signalLamps);

            yield return new WaitForSeconds(delaySignal);

            SetSignalWarning(false, signalLamps);

            yield return new WaitForSeconds(delaySignal);
        }
    }

    void SetSignalWarning(bool isActive, GameObject[] signalLamps)
    {
        AudioClip signalClip = null;

        if(isActive)
            signalClip = audioClass.turnSignalOnClip;
        else
            signalClip = audioClass.turnSignalOffClip;

        if(!AudioManager.Instance.SfxAudioSource.isPlaying)
            AudioManager.Instance.PlayOneShotSFX(signalClip);

        foreach (var item in signalLamps)
        {
            item.SetActive(isActive);
        }
    }

    void HornSwitch()
    {
        AudioManager.Instance.PlayOneShotSFX(AudioManager.Instance.HornClip);
    }

    void EngineStart()
    {
        isEngineStart = !isEngineStart;

        foreach (var item in partClass.wheels)
        {
            item.enabled = isEngineStart;
        }

        if(isEngineStart)
        {
            AudioManager.Instance.PlayOneShotSFX(audioClass.engineStartClip);
            audioClass.engineSound.Play();
        }
        else
        {
            moveDirection = Vector3.zero.x;
            currentAcceleration = Vector3.zero.x;
            AudioManager.Instance.PlayOneShotSFX(audioClass.engineShootdownClip);
            audioClass.engineSound.Stop();
        }
    }

    void EngineForce()
    {
        foreach (var item in partClass.wheels)
        {
            item.motorTorque = currentAcceleration;
            item.steerAngle = currentTurnAngle;
            item.brakeTorque = currentBrakingForce;
        }
    }

    void GearShiftUp()
    {
        if(isGearRear)
        {
            isGearNetral = true;
            isGearRear = false;
        }
        else
        {
            if(!isGearNetral)
                currentGearShift++;
            else
                isGearNetral = false;

            if(currentGearShift > maxGearShift)
                currentGearShift = maxGearShift;
        }
    }

    void GearShiftDown()
    {
        currentGearShift--;

        if(currentGearShift < minGearShift)
        {
            currentGearShift = minGearShift;

            if(isGearNetral)
            {
                isGearNetral = false;
                isGearRear = true;
            }
            else if(!isGearRear && isGearNetral || !isGearRear && !isGearNetral)
            {
                isGearNetral = true;
                isGearRear = false;
            }
        }
    }

    void EngineSoundPitch()
    {
        enginePitch = Mathf.Abs(WheelRotation() / acceleration) + pitchOffside;

        if(enginePitch > maxEnginePitch)
            enginePitch = maxEnginePitch;

        audioClass.engineSound.pitch = enginePitch;

        if(isGearNetral)
            audioClass.engineSound.pitch += (Mathf.Abs(keyboardInput.Keycode_Acceleration)/maxEnginePitch);
    }

}
