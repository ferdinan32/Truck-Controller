using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] KeyboardInput keyboardInput;
    [SerializeField] GameObject cameraVR;
    [SerializeField] GameObject cameraFPP;
    [SerializeField] GameObject cameraTPP;

    private bool isFPPCamera = false;
    private bool isVRmode = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetActiveCamera(new CameraToggle{
            isVR = isVRmode,
            isFPP = isFPPCamera,
            isTPP = !isFPPCamera
        });

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            isVRmode = true;
            SetActiveCamera(new CameraToggle{
                isVR = isVRmode
            });
            break;
        }

        if(keyboardInput == null)
        {
            keyboardInput = GameObject.FindObjectOfType<KeyboardInput>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(keyboardInput.Keycode_CameraPerspective)
        {
            ChangeCameraMode();
        }
    }

    void ChangeCameraMode()
    {
        if(isVRmode)
            return;

        isFPPCamera = !isFPPCamera;

        SetActiveCamera(new CameraToggle{
            isFPP = isFPPCamera,
            isTPP = !isFPPCamera
        });
    }

    void SetActiveCamera(CameraToggle args)
    {
        cameraVR.SetActive(args.isVR);
        cameraFPP.SetActive(args.isFPP);
        cameraTPP.SetActive(args.isTPP);
    }

    [System.Serializable]
    public class CameraToggle
    {
        public bool isVR;
        public bool isFPP;
        public bool isTPP;
    }
}
