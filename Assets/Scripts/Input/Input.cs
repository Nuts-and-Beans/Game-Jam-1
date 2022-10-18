using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

public static class Input
{
  public static GameInput StaticAsset                       { get; private set; }
  public static InputControlScheme ControlScheme            { get; private set; }
  public static InputControlScheme[] KeyboardControlSchemes { get; private set; }
  
  private static InputControlScheme[] AllControlSchemes;
  
  public delegate void NewDeviceDetected(InputDevice device, InputControlScheme controlScheme);
  public static NewDeviceDetected OnNewDeviceDetected;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  private static void Init()
  {
    // Create new input actions
    StaticAsset = new GameInput();
    StaticAsset.Enable();
    
    // Getting the index of all the control schemes here so they can be split up into their own array
    ReadOnlyArray<InputControlScheme> controlSchemes = StaticAsset.controlSchemes;
    int keyboard1Index  = controlSchemes.IndexOf(x => x.name == "KeyboardP1");
    int keyboard2Index  = controlSchemes.IndexOf(x => x.name == "KeyboardP2");
    int controllerIndex = controlSchemes.IndexOf(x => x.name == "Controller");
    
    // Set up default control schemes
    KeyboardControlSchemes = new InputControlScheme[PlayerInput.MaxPlayerCount];
    KeyboardControlSchemes[(int)Player.PLAYER_1] = controlSchemes[keyboard1Index];
    KeyboardControlSchemes[(int)Player.PLAYER_2] = controlSchemes[keyboard2Index];
    
    ControlScheme = controlSchemes[controllerIndex];
    
    // REVIEW(WSWhitehouse): This aint nice...
    List<InputControlScheme> schemes = new List<InputControlScheme>(KeyboardControlSchemes) { ControlScheme };
    AllControlSchemes = schemes.ToArray();

    // Search for unpaired devices
    InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    ++InputUser.listenForUnpairedDeviceActivity;
  }
  
  private static void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
  {
    // Check if device is valid
    InputDevice device = control.device;
    if (!IsDeviceValid(device)) return;
    
    // Check if control scheme is valid
    InputControlScheme? controlSchemeOptional = FindControlScheme(device);
    if (!controlSchemeOptional.HasValue) return;
    
    OnNewDeviceDetected?.Invoke(device, controlSchemeOptional.Value);
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static InputControlScheme? FindControlScheme(InputDevice device)
  {
    if (StaticAsset.controlSchemes.Count <= 0) return null;

    InputControlScheme[] controlSchemes = AllControlSchemes;
    using InputControlList<InputDevice> unpairedDevices = InputUser.GetUnpairedInputDevices();
    return InputControlScheme.FindControlSchemeForDevices(unpairedDevices, controlSchemes, device);
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool IsDeviceValid(InputDevice device)
  {
    // If the asset has control schemes, see if there's one that works with the device plus
    // whatever unpaired devices we have left.
    if (StaticAsset.controlSchemes.Count > 0)
    {
      return FindControlScheme(device) != null;
    }

    // Otherwise just check whether any of the maps has bindings usable with the device.
    foreach (InputActionMap actionMap in StaticAsset.asset.actionMaps)
    {
      if (actionMap.IsUsableWithDevice(device)) return true;
    }

    return false;
  }

}