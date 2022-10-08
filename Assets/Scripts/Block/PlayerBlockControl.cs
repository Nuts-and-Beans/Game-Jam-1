using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockControl : MonoBehaviour
{
  [SerializeField] private int playerIndex;
  
  [Header("Movement Settings")]
  [SerializeField] private float horizontalMoveAmount   = 0.5f;
  [SerializeField] private float downMovementMultiplier = 1.5f;
  
  [Header("Rotation Settings")]
  [SerializeField] private float rotationAmount = 90.0f;
  [SerializeField] private Vector3 rotationAxis = new Vector3(0.0f, 0.0f, 1.0f);
  [Space]
  [SerializeField] private Block startingBlock; // TODO(WSWhitehouse): Remove this as its for testing only
  
  private Block _activeBlock      = null;
  private bool _horizontalPressed = false;
  
  private const float MovementDeadZone = 0.05f;
  private PlayerInput.InputData Input => PlayerInput.Players[playerIndex];

  private IEnumerator Start()
  {
    yield return PlayerInput.WaitForValidPlayerInput(playerIndex);

    // NOTE(WSWhitehouse): Clamping rotation axis to ensure its in a 0-1 range. Otherwise, weird rotations will occur...
    rotationAxis = new Vector3(Mathf.Clamp01(rotationAxis.x), 
                               Mathf.Clamp01(rotationAxis.y), 
                               Mathf.Clamp01(rotationAxis.z));
    
    Input.Asset.Block.Move.performed   += OnMovePerformed;
    Input.Asset.Block.Move.canceled    += OnMovePerformed;
    Input.Asset.Block.Rotate.performed += OnRotatePerformed;
    
    SetActiveBlock(startingBlock); // TODO(WSWhitehouse): Remove this as its for testing only
  }

  private void OnDestroy()
  {
    if (!PlayerInput.IsPlayerValid(playerIndex)) return;
    
    Input.Asset.Block.Move.performed   -= OnMovePerformed;
    Input.Asset.Block.Move.canceled    -= OnMovePerformed;
    Input.Asset.Block.Rotate.performed -= OnRotatePerformed;
  }
  
  public void SetActiveBlock(Block block)
  {
    _activeBlock = block;
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
      
      float newXPos = blockPosition.x + moveAmount;
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
  }
}