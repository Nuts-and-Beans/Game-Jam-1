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

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{

  [SerializeField] private BlockType blockType;
  [SerializeField] private float movementSpeed = 1.0f;
  public Vector2 blockCenter = Vector2.zero;
  public Vector2 blockBounds = Vector2.one;
  
  public float MovementSpeed => movementSpeed * MovementMultiplier;
  
  public const float DefaultMovementMultiplier  = 1.0f;
  public float MovementMultiplier { get; set; } = DefaultMovementMultiplier;

  public BlockType Type => blockType;
  
  public Rigidbody2D Rigidbody { get; private set; }

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody2D>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void SetActive(bool active) => gameObject.SetActive(active);

  private void FixedUpdate()
  {
    Vector3 blockPosition = transform.position;
    float newYPos = blockPosition.y - (MovementSpeed * Time.deltaTime);
    Rigidbody.MovePosition(new Vector2(blockPosition.x, newYPos));
    //AudioManager.Play("MovingBlock"); Vlad - Do not uncoment this will hurt your ears
    }
  
#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position + (Vector3)blockCenter, blockBounds);
  }
#endif
}
