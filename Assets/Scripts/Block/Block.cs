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
}


[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    public bool collide = false;

    public Transform LB2B;
    public Transform LB1B;
    public Transform RB1B;
    public Transform RB2B;

    public GameObject LB2BG;
    public GameObject LB1BG;
    public GameObject RB1BG;
    public GameObject RB2BG;

    private Renderer LB2BR;
    private Renderer LB1BR;
    private Renderer RB1BR;
    private Renderer RB2BR;

    private Color newcolour;


    private int weightDistribution = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
      //  weightDistribution = 0;
        // Make an empty list to hold contact points
        ContactPoint2D[] contacts = new ContactPoint2D[10];

        // Get the contact points for this collision
        int numContacts = collision.GetContacts(contacts);

        // Iterate through each contact point
        for (int i = 0; i < numContacts; i++)
        {
            Debug.Log(contacts[i].point);
            Debug.Log("L2 " + LB2B.position);
            Debug.Log("L1 " + LB1B.position);
            Debug.Log("R1 " + RB1B.position);
            Debug.Log("R2 " + RB2B.position);
            // Test the distance from the contact point to the right hand
            if (Vector2.Distance(contacts[i].point, LB2B.position) < .5f) 
            {
                weightDistribution = weightDistribution + 15;
            }

            // Test the distance from the contact point to the left hand
            if (Vector2.Distance(contacts[i].point, LB1B.position) < .5f)
            {
                weightDistribution = weightDistribution + 25;
            }
            // Test the distance from the contact point to the right hand
            if (Vector2.Distance(contacts[i].point, RB2B.position) < .5f)
            {
                weightDistribution = weightDistribution + 15;
            }

            // Test the distance from the contact point to the left hand
            if (Vector2.Distance(contacts[i].point, RB1B.position) < .5f)
            {
                weightDistribution = weightDistribution + 25;
            }
         
        }
        Debug.Log(weightDistribution);
    
            StartCoroutine(Falling());
        
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
        LB1BR = LB1BG.GetComponent<Renderer>();
        LB2BR = LB2BG.GetComponent<Renderer>();
        RB1BR = RB1BG.GetComponent<Renderer>();
        RB2BR = RB2BG.GetComponent<Renderer>();
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
    IEnumerator Falling()
    {
        yield return new WaitForSeconds(.2f);
        if (weightDistribution < 25)
        {
            Debug.Log("1");
            newcolour = new Color(0, 1, 0, 1);
            LB1BR.material.SetColor("_colour1", newcolour);
            LB2BR.material.SetColor("_colour2", newcolour);
            RB1BR.material.SetColor("_colour3", newcolour);
            RB2BR.material.SetColor("_colour4", newcolour);

         
            Debug.Log("2");
            newcolour = new Color(1, 0, 0, 1);
            LB1BR.material.SetColor("_colour1", newcolour);
            LB2BR.material.SetColor("_colour2", newcolour);
            RB1BR.material.SetColor("_colour3", newcolour);
            RB2BR.material.SetColor("_colour4", newcolour);

            yield return new WaitForSeconds(.7f);
            Debug.Log("3");
            Destroy(gameObject);
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
