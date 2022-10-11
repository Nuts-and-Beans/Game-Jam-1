using System.Runtime.CompilerServices;
using UnityEngine;

public enum BlockType
{
  INVALID = -1,
  S_BLOCK,
  Z_BLOCK,
  T_BLOCK,
  O_BLOCK,
  L_BLOCK,
  J_BLOCK,
  I_BLOCK,
}


public class Block : MonoBehaviour
{

  [SerializeField] private BlockType blockType;
  [SerializeField] private float movementSpeed = 1.0f;
  
  public float MovementSpeed => movementSpeed * MovementMultiplier;
  
  public const float DefaultMovementMultiplier  = 1.0f;
  public float MovementMultiplier { get; set; } = DefaultMovementMultiplier;

  public BlockType Type => blockType;
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void SetActive(bool active) => gameObject.SetActive(active);

  private void FixedUpdate()
  {
    Vector3 blockPosition = transform.position;
    
    float newYPos      = blockPosition.y - (MovementSpeed * Time.deltaTime);
    transform.position = new Vector3(blockPosition.x, newYPos, blockPosition.z);
  }
}
