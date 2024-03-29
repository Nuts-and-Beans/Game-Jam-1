﻿using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockControl : MonoBehaviour
{
  [SerializeField] private Player playerID;
  [SerializeField] private int collisionLayer;
  
  [Header("Movement Settings")]
  [SerializeField] private float horizontalMoveAmount   = 0.5f;
  [SerializeField] private float downMovementMultiplier = 1.5f;
  
  [Header("Rotation Settings")]
  [SerializeField] private float rotationAmount = 90.0f;
  [SerializeField] private Vector3 rotationAxis = new Vector3(0.0f, 0.0f, 1.0f);
 
  private Block _activeBlock      = null;
  private bool _horizontalPressed = false;
  
  private const float MovementDeadZone = 0.05f;
  private PlayerInput.InputData Input => PlayerInput.Players[(int)playerID];

  private IEnumerator Start()
  {
    yield return PlayerInput.WaitForValidPlayerInput((int)playerID);

    // NOTE(WSWhitehouse): Clamping rotation axis to ensure its in a 0-1 range. Otherwise, weird rotations will occur...
    rotationAxis = new Vector3(Mathf.Clamp01(rotationAxis.x), 
                               Mathf.Clamp01(rotationAxis.y), 
                               Mathf.Clamp01(rotationAxis.z));
    
    Input.Asset.Block.Move.performed   += OnMovePerformed;
    Input.Asset.Block.Move.canceled    += OnMovePerformed;
    Input.Asset.Block.Rotate.performed += OnRotatePerformed;


    // if we have not set a start
    SetActiveBlock(Random_Spawn.GetBlock(playerID));

    BlockKillVolume.OnControlledBlockKilled += OnControlledBlockKilled;
  }

  private void OnDestroy()
  {
    if (!PlayerInput.IsPlayerValid(playerID)) return;
    
    Input.Asset.Block.Move.performed   -= OnMovePerformed;
    Input.Asset.Block.Move.canceled    -= OnMovePerformed;
    Input.Asset.Block.Rotate.performed -= OnRotatePerformed;

    BlockKillVolume.OnControlledBlockKilled -= OnControlledBlockKilled;
    
    if (_activeBlock == null) return;
    _activeBlock.OnBlockLockedIn -= OnBlockLockedIn;
  }
  
  public void SetActiveBlock(Block block)
  {
    _activeBlock = block;
    _activeBlock.OnBlockLockedIn += OnBlockLockedIn;
    _activeBlock.PlayerID = playerID;

    // we set the collision layer for every collider
    _activeBlock.gameObject.layer = collisionLayer;
    foreach (Transform child in _activeBlock.transform) {
        child.gameObject.layer = collisionLayer;
    }
  }

  private void OnMovePerformed(InputAction.CallbackContext context)
  {
    Vector2 move = context.ReadValue<Vector2>();

    MoveVertical(move.y);
    MoveHorizontal(move.x);
  }
  
  private void MoveVertical(float y)
  {
    if (_activeBlock == null) return;
    
    if (y <= -MovementDeadZone) // Moving down
    {
      _activeBlock.MovementMultiplier = downMovementMultiplier;
            AudioManager.Play("MovingBlock");
        }
    else // Stopped moving down
    {
      _activeBlock.MovementMultiplier = Block.DefaultMovementMultiplier;
    }
  }
  
  private void MoveHorizontal(float x)
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Move(float moveAmount)
    {
      Transform blockTransform = _activeBlock.transform;
      Vector3 blockPosition    = blockTransform.position;

      float maxX, minX;
      switch (_activeBlock.PlayerID)
      {
        case Player.INVALID:
        {
          maxX = ( GameManager.HalfWorldBounds.x - _activeBlock.BlockCenter.x) - (_activeBlock.BlockBounds.x * 0.5f);
          minX = (-GameManager.HalfWorldBounds.x - _activeBlock.BlockCenter.x) + (_activeBlock.BlockBounds.x * 0.5f);
          break;
        }
        case Player.PLAYER_1:
        {
          maxX = ( GameManager.PlayerSeparator   - _activeBlock.BlockCenter.x) - ((_activeBlock.BlockBounds.x * 0.5f) + 0.15f);
          minX = (-GameManager.HalfWorldBounds.x - _activeBlock.BlockCenter.x) + ((_activeBlock.BlockBounds.x * 0.5f) + 0.15f);
          break;
        }
        case Player.PLAYER_2:
        {
          maxX = (GameManager.HalfWorldBounds.x - _activeBlock.BlockCenter.x) - ((_activeBlock.BlockBounds.x * 0.5f) + 0.15f);
          minX = (GameManager.PlayerSeparator   - _activeBlock.BlockCenter.x) + ((_activeBlock.BlockBounds.x * 0.5f) + 0.15f);
          break;
        }
        default: throw new ArgumentOutOfRangeException();
      }
      
      float newXPos = Mathf.Clamp(blockPosition.x + moveAmount, minX, maxX);
      blockTransform.position = new Vector3(newXPos, blockPosition.y, blockPosition.z);
    }
    
    if (_horizontalPressed)
    {
      if (Mathf.Abs(x) < MovementDeadZone) _horizontalPressed = false;
      return;
    }
    
    if (_activeBlock == null) return;
    
    // Move Right
    if (x >= MovementDeadZone)
    {
      Move(horizontalMoveAmount);
      _horizontalPressed = true;
    }
    
    // Move Left
    if (x <= -MovementDeadZone)
    {
      Move(-horizontalMoveAmount);
      _horizontalPressed = true;
    }
  }

  private void OnRotatePerformed(InputAction.CallbackContext context)
  {
    if (_activeBlock == null) return;

    Transform blockTransform    = _activeBlock.transform;
    blockTransform.eulerAngles += rotationAxis * rotationAmount;
    AudioManager.Play("RotatingBlock"); /// Add ding sounds - vlad
  }

  private void OnBlockLockedIn()
  {
    // we unsubscribe from the current blocks event
    _activeBlock.OnBlockLockedIn -= OnBlockLockedIn;

    // and set a new block to be controlled
    Block block = Random_Spawn.GetBlock(playerID);
    SetActiveBlock(block);
  }

  private void OnControlledBlockKilled(Player id)
  {
    if (id != playerID) return;
    
    // we unsubscribe from the current blocks event
    _activeBlock.OnBlockLockedIn -= OnBlockLockedIn;
      
    // and set a new block to be controlled
    Block block = Random_Spawn.GetBlock(playerID);
    SetActiveBlock(block);
  }
}
