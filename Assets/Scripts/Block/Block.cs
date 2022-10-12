using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
  [SerializeField] private Player player; // TODO(WSWhitehouse): Remove this after testing
  [SerializeField] private BlockType blockType;
  [SerializeField] private float movementSpeed = 1.0f;
  public Vector2 blockCenter = Vector2.zero;
  public Vector2 blockBounds = Vector2.one;
  
  public float MovementSpeed => movementSpeed * MovementMultiplier;
  
  public const float DefaultMovementMultiplier  = 1.0f;
  public float MovementMultiplier { get; set; } = DefaultMovementMultiplier;

  public BlockType Type => blockType;
  
  public Rigidbody2D Rigidbody { get; private set; }
  
  // NOTE(WSWhitehouse): Which player does this block belong too... 
  public Player PlayerID { get; set; } = Player.INVALID;

  private void Awake()
  {
    PlayerID = player; // TODO(WSWhitehouse): Remove this after testing
    Rigidbody = GetComponent<Rigidbody2D>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void SetActive(bool active) => gameObject.SetActive(active);

  private void FixedUpdate()
  {
    Vector3 blockPosition = transform.position;
    float newYPos = blockPosition.y - (MovementSpeed * Time.deltaTime);
    Rigidbody.MovePosition(new Vector2(blockPosition.x, newYPos));
  }
  
#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(transform.position + (Vector3)blockCenter, blockBounds);
  }
#endif
}
