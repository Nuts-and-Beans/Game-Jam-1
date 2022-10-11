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
    public bool collide = false;

    public Transform LB2B;
    public Transform LB1B;
    public Transform RB1B;
    public Transform RB2B;
    
    private int weightDistribution = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Output the Collider's GameObject's name
        //      Debug.Log(collision.collider.name);


        // Make an empty list to hold contact points
        

        // Get the contact points for this collision
        int numContacts = 4;
        if (!collide)
        {
            if (collision.collider.bounds.Contains(LB2B.position))
            {
                print("LB2B is inside collider");
                weightDistribution = weightDistribution + 10;

            }
            if (collision.collider.bounds.Contains(LB1B.position))
            {
                print("LB1B is inside collider");
                weightDistribution = weightDistribution + 25;
            }
            if (collision.collider.bounds.Contains(RB2B.position))
            {
                print("RB2B is inside collider");
                weightDistribution = weightDistribution + 10;
            }
            if (collision.collider.bounds.Contains(RB1B.position))
            {
                print("RB1B is inside collider");
                weightDistribution = weightDistribution + 25;
            }
        }
        Debug.Log(weightDistribution);
        collide = true;
        if (weightDistribution < 25)
        {
            this.transform.TransformDirection(1000, 0, 0);
            Destroy(this);
        }
      
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collide = false;
    }
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
        if (!collide)
        {
            Vector3 blockPosition = transform.position;
            float newYPos = blockPosition.y - (MovementSpeed * Time.deltaTime);
            Rigidbody.MovePosition(new Vector2(blockPosition.x, newYPos));
        }
  }
  

#if UNITY_EDITOR
    private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position + (Vector3)blockCenter, blockBounds);
  }
#endif
}
