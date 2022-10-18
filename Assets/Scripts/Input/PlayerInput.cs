using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public static class PlayerInput
{
  public class InputData
  {
    public InputData(InputDevice device, InputControlScheme controlScheme)
    {
      // https://forum.unity.com/threads/input-system-generate-c-code-what-its-good-for.995674/
      // https://forum.unity.com/threads/solved-can-the-new-input-system-be-used-without-the-player-input-component.856108/#post-5669128
      
      Device          = device;
      ControlScheme   = controlScheme;
      
      Asset             = new GameInput();
      Asset.devices     = new[] { Device };
      Asset.bindingMask = InputBinding.MaskByGroup(Device.name);
      Asset.Enable();

      User = InputUser.PerformPairingWithDevice(Device);
      User.AssociateActionsWithUser(Asset);
      User.ActivateControlScheme(ControlScheme);
    }
    
    ~InputData()
    {
      User.UnpairDevicesAndRemoveUser();
      Asset.Disable();
    }
    
    // Input Data
    public GameInput Asset                  { get; }
    public InputUser User                   { get; }
    public InputDevice Device               { get; }
    public InputControlScheme ControlScheme { get; }
  }
  
  public const int MaxPlayerCount   = 2;
  public static InputData[] Players = new InputData[MaxPlayerCount];
  public static int PlayerCount     => Players.Count(x => x != null);
  
  public static Action<int> OnPlayerJoined;
  public static Action<int> OnPlayerRemoved;

  private static Input.NewDeviceDetected OnNewDeviceDetected;
  public delegate IEnumerator WaitForValidPlayerInputDelegate(int index);
  public static WaitForValidPlayerInputDelegate WaitForValidPlayerInput;
  
  [RuntimeInitializeOnLoadMethod]
  private static void Init()
  {
    OnNewDeviceDetected      = __OnNewDeviceDetected;
    WaitForValidPlayerInput  = __WaitForValidPlayerInput;
    
    StartSearchingForPlayers();
  }

  public static void StartSearchingForPlayers() { Input.OnNewDeviceDetected += OnNewDeviceDetected; } 
  public static void StopSearchingForPlayers()  { Input.OnNewDeviceDetected -= OnNewDeviceDetected; }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void RemovePlayer(int index)
  {
    Players[index] = null;
    OnPlayerRemoved?.Invoke(index);
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void RemoveAllPlayers() { for (int i = 0; i < MaxPlayerCount; i++) RemovePlayer(i); }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsPlayerValid(int index) => Players[index] != null;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool IsPlayerValid(Player index) => IsPlayerValid((int)index);
  
  private static void __OnNewDeviceDetected(InputDevice device, InputControlScheme controlScheme)
  {
    int currentPlayerCount = PlayerCount;
    if (currentPlayerCount == MaxPlayerCount) return; 
    
    if (device is Keyboard)
    {
      switch (currentPlayerCount)
      {
        case 0:
        {
          const int playerID = (int)Player.PLAYER_1;
          Players[playerID]  = new InputData(device, Input.KeyboardControlSchemes[playerID]);
          OnPlayerJoined?.Invoke(playerID);
        } goto case 1;
          
        case 1:
        {
          const int playerID = (int)Player.PLAYER_2;
          Players[playerID]  = new InputData(device, Input.KeyboardControlSchemes[playerID]);
          OnPlayerJoined?.Invoke(playerID);
        } goto default;
          
        default: return;
      }
    }
    
    Players[currentPlayerCount] = new InputData(device, controlScheme);
    OnPlayerJoined?.Invoke(currentPlayerCount);
  }
  
  private static IEnumerator __WaitForValidPlayerInput(int index)
  {
    while (Players[index] == null)
    {
      yield return null; // Wait for update
    }
  }
}