using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    [SerializeField] private BlockType blockType;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private Vector2 blockCenter = Vector2.zero;
    [SerializeField] private Vector2 blockBounds = Vector2.one;

    [Header("Collision Settings")]
    [SerializeField] private float waitBeforeLockInPlaceTimer = 0.05f;
    [SerializeField] private ContactFilter2D filter;
    [SerializeField] private Collider2D[] colliders;

    public float MovementSpeed => movementSpeed * MovementMultiplier;

    public Vector2 BlockCenter => Vector3.Scale(transform.lossyScale, blockCenter);
    public Vector2 BlockBounds => Vector3.Scale(transform.lossyScale, blockBounds);

    public const float DefaultMovementMultiplier = 1.0f;
    public float MovementMultiplier { get; set; } = DefaultMovementMultiplier;

    public BlockType Type => blockType;

    public Rigidbody2D Rigidbody { get; private set; }

    // NOTE(WSWhitehouse): Which player does this block belong too... 
    public Player PlayerID { get; set; } = Player.INVALID;


    // NOTE(Zack): an event so that the block knows [PlayerBlockControl] knows to stop controlling this block
    public delegate void BlockEvent();
    public BlockEvent OnBlockLockedIn;

    private delegate IEnumerator BlockWaitCo();
    private BlockWaitCo WaitBeforeLockIn;
    
    private bool moving = false;
    private Coroutine waitTimerCo;

    private ContactPoint2D[] contacts = new ContactPoint2D[15];
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

        // preallocate the coroutine
        WaitBeforeLockIn = __WaitBeforeLockIn;
    }


    private void FixedUpdate()
    {
        // HACK(Zack): shouldn't be doing this check, but it's a quick hacky fix for now
        if (!moving) return;

        
        Vector3 blockPosition = transform.position;
        float newYPos = blockPosition.y - (MovementSpeed * Time.deltaTime);
        Rigidbody.MovePosition(new Vector2(blockPosition.x, newYPos));
        //AudioManager.Play("MovingBlock"); Vlad - Do not uncoment this will hurt your ears
    }


    private void OnTriggerEnter2D(Collider2D col) {
        // stops stationary blocks from having the logic below run
        if (!moving) return;

        waitTimerCo = StartCoroutine(WaitBeforeLockIn());
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (!moving) return;

        if (waitTimerCo == null) return;
        StopCoroutine(waitTimerCo);
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetActive(bool active) {
        moving = true;

        // we set the newly active block to be a trigger collider, so it doesn't push the stationary blocks with physics
        for (int i = 0; i < colliders.Length; ++i) {
            colliders[i].isTrigger = true;
        }
        
        gameObject.SetActive(active);
    }

    private void SetBlockStationary() {
        moving = false;
        Rigidbody.mass = 5;
        Rigidbody.gravityScale = 1;

        // we make all of the colliders physical colliders again
        for (int i = 0; i < colliders.Length; ++i) {
            colliders[i].isTrigger = false;
        }

        OnBlockLockedIn?.Invoke();
        Debug.Log("Block Locked In");
    }


    private IEnumerator __WaitBeforeLockIn() {
        float elapsed = 0f;
        while (elapsed < waitBeforeLockInPlaceTimer) {
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetBlockStationary();
        waitTimerCo = null;
        yield break;
    }
    

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + (Vector3)BlockCenter, BlockBounds);
    }
#endif
}
