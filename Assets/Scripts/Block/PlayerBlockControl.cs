using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlockControl : MonoBehaviour
{
  [SerializeField] private int playerIndex;
  
  private PlayerInput.InputData Input => PlayerInput.Players[playerIndex];

  private IEnumerator Start()
  {
    yield return PlayerInput.WaitForValidPlayerInput(playerIndex);
    
    Input.Asset.Block.Move.performed   += OnMovePerformed;
    Input.Asset.Block.Move.canceled    += OnMovePerformed;
    Input.Asset.Block.Rotate.performed += OnRotatePerformed;
  }

  private void OnDestroy()
  {
    if (!PlayerInput.IsPlayerValid(playerIndex)) return;
    
    Input.Asset.Block.Move.performed   -= OnMovePerformed;
    Input.Asset.Block.Move.canceled    -= OnMovePerformed;
    Input.Asset.Block.Rotate.performed -= OnRotatePerformed;
  }

  private void OnMovePerformed(InputAction.CallbackContext context)
  {
    Vector2 move = context.ReadValue<Vector2>();
    Debug.Log(move.ToString());
  }

  private void OnRotatePerformed(InputAction.CallbackContext context)
  {
    Debug.Log("Rotate");
  }
}