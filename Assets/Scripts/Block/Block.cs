using System.Collections;
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

  MAX_BLOCK_TYPES
}

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
  [SerializeField] private Collider2D[] colliders;
  [SerializeField] private BlockType blockType;
  [SerializeField] private float movementSpeed = 1.0f;
  [SerializeField] private float freezeWaitTime = 0.5f;
  
  public Vector2 blockCenter = Vector2.zero;
  public Vector2 blockBounds = Vector2.one;
  
  public float MovementSpeed => movementSpeed * MovementMultiplier;
  
  public const float DefaultMovementMultiplier  = 1.0f;
  public float MovementMultiplier { get; set; } = DefaultMovementMultiplier;

  public BlockType Type => blockType;
  
  public Rigidbody2D Rigidbody { get; private set; }

  public delegate void BlockDel();
  public BlockDel OnBlockLockedIn;
  
  private Coroutine checkTimerCo;
  private float collisionCheckTimer = 0f;
  private bool moving = true;
  private bool pausedMovement = false;
  
  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody2D>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void SetActive(bool active)
  {
    moving = true;
    pausedMovement = false;
    Rigidbody.mass = 1;
    Rigidbody.gravityScale = 0;
    checkTimerCo = null;

    for (int i = 0; i < colliders.Length; ++i)
    {
      colliders[i].isTrigger = true;
    }
    /*
    */
    
    gameObject.SetActive(active);
  }

  private void FixedUpdate()
  {
    // HACK(Zack): this is a dirty way to stop the block from moving
    if (!moving) return;

    if (pausedMovement) return;
    
    Vector3 blockPosition = transform.position;
    float newYPos = blockPosition.y - (MovementSpeed * Time.deltaTime);
    Rigidbody.MovePosition(new Vector2(blockPosition.x, newYPos));
  }


  private void OnCollisionEnter2D(Collision2D col)
  {
    Debug.Log("Collision");

    // HACK(Zack): this is a dirty way to stop the block from moving
    if (!moving) return;
    pausedMovement = true;

    checkTimerCo = StartCoroutine(CheckContactTime());
  }

  private void OnCollisionExit2D(Collision2D col)
  {
    pausedMovement = false;
    
    if (!moving) return;
    if (checkTimerCo != null)
    {
      StopCoroutine(checkTimerCo);
    }
  }



  // TODO(Zack): check for contact point normals and see if they're horizontal, and don't allow them 
  private void OnTriggerEnter2D(Collider2D col)
  {
    Debug.Log("Collision");

    // HACK(Zack): this is a dirty way to stop the block from moving
    if (!moving) return;
    pausedMovement = true;

    checkTimerCo = StartCoroutine(CheckContactTime());
  }

  private void OnTriggerExit2D(Collider2D col)
  {
    pausedMovement = false;
    
    if (!moving) return;
    if (checkTimerCo != null)
    {
      StopCoroutine(checkTimerCo);
    }
  }
  

  private IEnumerator CheckContactTime()
  {
    collisionCheckTimer = 0f;

    while (collisionCheckTimer < freezeWaitTime)
    {
      collisionCheckTimer += Time.deltaTime;
      yield return null;
    }

    SetBlockStationary();

    yield break;
  }

  private void SetBlockStationary()
  {
    moving = false;
    Rigidbody.mass = 5;
    Rigidbody.gravityScale = 1;

    for (int i = 0; i < colliders.Length; ++i)
    {
      colliders[i].isTrigger = false;
    }
    /*
    */
    
    OnBlockLockedIn?.Invoke();
    Debug.Log("Block Locked In");
  }
  
#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position + (Vector3)blockCenter, blockBounds);
  }
#endif
}
