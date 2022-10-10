using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlowdown : MonoBehaviour
{
    //Game Mechanics: Blocks Falling (glitchy/slowdown)
    public bool isSlowdown = true;
    public float slowdownFactor = 2f;
    [SerializeField]
    private int blocks_amount; //Change to calculate amount of blocks on screen

    Block block;
    BlockPool blockPool;
    private void Start()
    {
        block = GetComponent<Block>();
        blockPool = GameObject.FindWithTag("BlockPool").GetComponent<BlockPool>();

    }

    private void FixedUpdate()
    {
        blocks_amount = blockPool.GetActiveBlocks();
        Debug.Log(block.MovementMultiplier);

        if (isSlowdown)
        {
            if (block.MovementMultiplier > 0.05f && blocks_amount > 0)
            {
                block.MovementMultiplier = 1f / blocks_amount;
            }

        }
        else
        {
            block.MovementMultiplier = 1.0f;
        }
    }
}
