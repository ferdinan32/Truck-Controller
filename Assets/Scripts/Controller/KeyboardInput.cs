using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

[RequireComponent(typeof(InputActionManager))]
public class KeyboardInput : MonoBehaviour
{
    private InputActionManager manager;
    public InputActionManager Manager
    {
        get
        {
            manager ??= GetComponent<InputActionManager>();
            return manager;
        }
    }

    private InputActionMap keyboard;
    public InputActionMap Keyboard
    {
        get
        {
            keyboard ??= Manager.actionAssets.Find(x => x.name == "Bindings").FindActionMap("Keyboard Bindings");
            return keyboard;
        }
    }

#region CAMERA
    private bool keycode_CameraPerspective;
    public bool Keycode_CameraPerspective
    {
        get
        {
            keycode_CameraPerspective = Keyboard.FindAction("Tab").WasPerformedThisFrame();
            return keycode_CameraPerspective;
        }
    }
#endregion CAMERA

#region Excavator
#region SWITCH
    private bool keycode_RevolvingLamp, keycode_PowerMaximizingSwitch, keycode_HornSwitch, keycode_TurnOnOffExcavator,
    keycode_ShutdownSecondarySwitch, keycode_FuelDialChange, keycode_ToggleLight, keycode_StepLever, keycode_SwingLockSwitch,
    keycode_AttachingSwitch;

    public bool Keycode_RevolvingLamp
    {
        get
        {
            keycode_RevolvingLamp = Keyboard.FindAction("F").WasPerformedThisFrame();
            return keycode_RevolvingLamp;
        }
    }

    public bool Keycode_PowerMaximizingSwitch
    {
        get
        {
            keycode_PowerMaximizingSwitch = Keyboard.FindAction("U").WasPerformedThisFrame();
            return keycode_PowerMaximizingSwitch;
        }
    }

    public bool Release_PowerMaximizingSwitch
    {
        get
        {
            keycode_PowerMaximizingSwitch = Keyboard.FindAction("U").WasReleasedThisFrame();
            return keycode_PowerMaximizingSwitch;
        }
    }

    public bool Keycode_HornSwitch
    {
        get
        {
            keycode_HornSwitch = Keyboard.FindAction("H").WasPerformedThisFrame();
            return keycode_HornSwitch;
        }
    }

    public bool Keycode_TurnOnOffExcavator
    {
        get
        {
            keycode_TurnOnOffExcavator = Keyboard.FindAction("P").WasPerformedThisFrame();
            return keycode_TurnOnOffExcavator;
        }
    }

    public bool Keycode_ShutdownSecondarySwitch
    {
        get
        {
            keycode_ShutdownSecondarySwitch = Keyboard.FindAction("J").WasPerformedThisFrame();
            return keycode_ShutdownSecondarySwitch;
        }
    }

    public bool Keycode_FuelDialChange
    {
        get
        {
            keycode_FuelDialChange = Keyboard.FindAction("R").WasPerformedThisFrame();
            return keycode_FuelDialChange;
        }
    }

    public bool Keycode_ToggleLight
    {
        get
        {
            keycode_ToggleLight = Keyboard.FindAction("L").WasPerformedThisFrame();
            return keycode_ToggleLight;
        }
    }

    public bool Keycode_StepLever
    {
        get
        {
            keycode_StepLever = Keyboard.FindAction("M").WasPerformedThisFrame();
            return keycode_StepLever;
        }
    }

    public bool Keycode_SwingLockSwitch
    {
        get
        {
            keycode_SwingLockSwitch = Keyboard.FindAction("N").WasPerformedThisFrame();
            return keycode_SwingLockSwitch;
        }
    }

    public bool Keycode_AttachingSwitch
    {
        get
        {
            keycode_AttachingSwitch = Keyboard.FindAction("O").WasPerformedThisFrame();
            return keycode_AttachingSwitch;
        }
    }
#endregion SWITCH

#region LEVER
    private bool keycode_LControlLeverForward, keycode_LControlLeverLeft, keycode_LControlLeverBackward, keycode_D,
    keycode_RControlLeverForward, keycode_RControlLeverLeft, keycode_RControlLeverRight, keycode_RControlLeverBackward;

    public bool Keycode_LControlLeverForward
    {
        get
        {
            keycode_LControlLeverForward = Keyboard.FindAction("W").IsPressed();
            return keycode_LControlLeverForward;
        }
    }

    public bool Keycode_LControlLeverLeft
    {
        get
        {
            keycode_LControlLeverLeft = Keyboard.FindAction("A").IsPressed();
            return keycode_LControlLeverLeft;
        }
    }

    public bool Keycode_LControlLeverBackward
    {
        get
        {
            keycode_LControlLeverBackward = Keyboard.FindAction("S").IsPressed();
            return keycode_LControlLeverBackward;
        }
    }

    public bool Keycode_LControlLeverRight
    {
        get
        {
            keycode_D = Keyboard.FindAction("D").IsPressed();
            return keycode_D;
        }
    }

    public bool Keycode_RControlLeverForward
    {
        get
        {
            keycode_RControlLeverForward = Keyboard.FindAction("UpArrow").IsPressed();
            return keycode_RControlLeverForward;
        }
    }

    public bool Keycode_RControlLeverLeft
    {
        get
        {
            keycode_RControlLeverLeft = Keyboard.FindAction("LeftArrow").IsPressed();
            return keycode_RControlLeverLeft;
        }
    }

    public bool Keycode_RControlLeverRight
    {
        get
        {
            keycode_RControlLeverRight = Keyboard.FindAction("RightArrow").IsPressed();
            return keycode_RControlLeverRight;
        }
    }

    public bool Keycode_RControlLeverBackward
    {
        get
        {
            keycode_RControlLeverBackward = Keyboard.FindAction("DownArrow").IsPressed();
            return keycode_RControlLeverBackward;
        }
    }
#endregion LEVER

#region PEDAL
    private bool keycode_LeftPedalForward, keycode_LeftPedalBackward, keycode_RightPedalForward, keycode_RightPedalBackward;

    public bool Keycode_LeftPedalForward
    {
        get
        {
            keycode_LeftPedalForward = Keyboard.FindAction("T").IsPressed();
            return keycode_LeftPedalForward;
        }
    }

    public bool Keycode_LeftPedalBackward
    {
        get
        {
            keycode_LeftPedalBackward = Keyboard.FindAction("G").IsPressed();
            return keycode_LeftPedalBackward;
        }
    }

    public bool Keycode_RightPedalForward
    {
        get
        {
            keycode_RightPedalForward = Keyboard.FindAction("I").IsPressed();
            return keycode_RightPedalForward;
        }
    }

    public bool Keycode_RightPedalBackward
    {
        get
        {
            keycode_RightPedalBackward = Keyboard.FindAction("K").IsPressed();
            return keycode_RightPedalBackward;
        }
    }

    public bool Release_LeftPedalForward
    {
        get
        {
            keycode_LeftPedalForward = Keyboard.FindAction("T").WasReleasedThisFrame();
            return keycode_LeftPedalForward;
        }
    }

    public bool Release_LeftPedalBackward
    {
        get
        {
            keycode_LeftPedalBackward = Keyboard.FindAction("G").WasReleasedThisFrame();
            return keycode_LeftPedalBackward;
        }
    }

    public bool Release_RightPedalForward
    {
        get
        {
            keycode_RightPedalForward = Keyboard.FindAction("I").WasReleasedThisFrame();
            return keycode_RightPedalForward;
        }
    }

    public bool Release_RightPedalBackward
    {
        get
        {
            keycode_RightPedalBackward = Keyboard.FindAction("K").WasReleasedThisFrame();
            return keycode_RightPedalBackward;
        }
    }
#endregion PEDAL
#endregion Excavator

#region Mixer Truck
    private bool keycode_Space;
    public bool Keycode_Space
    {
        get
        {
            keycode_Space = Keyboard.FindAction("Space").WasPerformedThisFrame();
            return keycode_Space;
        }
    }

    private float keycode_Steering;
    public float Keycode_Steering
    {
        get
        {
            keycode_Steering = Keyboard.FindAction("Steering").ReadValue<float>();
            return keycode_Steering;
        }
    }

    private float keycode_Acceleration;
    public float Keycode_Acceleration
    {
        get
        {
            keycode_Acceleration = Keyboard.FindAction("Acceleration").ReadValue<float>();
            return keycode_Acceleration;
        }
    }

    private float keycode_Break;
    public float Keycode_Brake
    {
        get
        {
            keycode_Break = Keyboard.FindAction("Brake").ReadValue<float>();
            return keycode_Break;
        }
    }

    private bool keycode_GearUp;
    public bool Keycode_GearUp
    {
        get
        {
            keycode_GearUp = Keyboard.FindAction("UpArrow").WasPerformedThisFrame();
            return keycode_GearUp;
        }
    }

    private bool keycode_GearDown;
    public bool Keycode_GearDown
    {
        get
        {
            keycode_GearDown = Keyboard.FindAction("DownArrow").WasPerformedThisFrame();
            return keycode_GearDown;
        }
    }

    private bool keycode_EngineStart;
    public bool Keycode_EngineStart
    {
        get
        {
            keycode_EngineStart = Keyboard.FindAction("P").WasPerformedThisFrame();
            return keycode_EngineStart;
        }
    }

    private bool keycode_ChangeMaterial;
    public bool Keycode_ChangeMaterial
    {
        get
        {
            keycode_ChangeMaterial = Keyboard.FindAction("C").WasPerformedThisFrame();
            return keycode_ChangeMaterial;
        }
    }

    private bool keycode_TurnSignalLeft;
    public bool Keycode_TurnSignalLeft
    {
        get
        {
            keycode_TurnSignalLeft = Keyboard.FindAction("Q").WasPerformedThisFrame();
            return keycode_TurnSignalLeft;
        }
    }

    private bool keycode_TurnSignalRight;
    public bool Keycode_TurnSignalRight
    {
        get
        {
            keycode_TurnSignalRight = Keyboard.FindAction("E").WasPerformedThisFrame();
            return keycode_TurnSignalRight;
        }
    }

    private bool keycode_TurnSignalWarning;
    public bool Keycode_TurnSignalWarning
    {
        get
        {
            keycode_TurnSignalWarning = Keyboard.FindAction("Z").WasPerformedThisFrame();
            return keycode_TurnSignalWarning;
        }
    }

    private bool keycode_MixerActive;
    public bool Keycode_MixerActive
    {
        get
        {
            keycode_MixerActive = Keyboard.FindAction("B").WasPerformedThisFrame();
            return keycode_MixerActive;
        }
    }

    private bool keycode_MixerRight;
    public bool Keycode_MixerRight
    {
        get
        {
            keycode_MixerRight = Keyboard.FindAction("N").WasPerformedThisFrame();
            return keycode_MixerRight;
        }
    }

    private bool keycode_MixerLeft;
    public bool Keycode_MixerLeft
    {
        get
        {
            keycode_MixerLeft = Keyboard.FindAction("V").WasPerformedThisFrame();
            return keycode_MixerLeft;
        }
    }

    private bool keycode_WorkingLamp;
    public bool Keycode_WorkingLamp
    {
        get
        {
            keycode_WorkingLamp = Keyboard.FindAction("G").WasPerformedThisFrame();
            return keycode_WorkingLamp;
        }
    }

#endregion Mixer Truck
}
