using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Random_Spawn : MonoBehaviour {
    [SerializeField] private Transform playerOneSpawn;
    [SerializeField] private Transform playerTwoSpawn;

    
    private static Transform p1Spawn;
    private static Transform p2Spawn;

    private static BlockType p1NextBlockType;
    private static BlockType p2NextBlockType;
    

    private void Awake() {
        p1NextBlockType = RandomBlockType();
        p2NextBlockType = RandomBlockType();

        p1Spawn = playerOneSpawn;
        p2Spawn = playerTwoSpawn;
    }
    
    // spawns random block based on random number generated
    public static Block GetBlock(Player id) {
        switch (id) {
            case Player.PLAYER_1: {
                Block newBlock = BlockPool.GetBlock(p1NextBlockType);

                // increment player 1's next block type
                p1NextBlockType = RandomBlockType();

                newBlock.transform.position = p1Spawn.position;
                newBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newBlock.transform.rotation = Quaternion.identity;
                newBlock.MovementMultiplier = Block.DefaultMovementMultiplier;
                
                return newBlock;
            } break;
                
            case Player.PLAYER_2: {
                Block newBlock = BlockPool.GetBlock(p2NextBlockType);

                // increment player 2's next block type
                p2NextBlockType = RandomBlockType();

                
                newBlock.transform.position = p2Spawn.position;
                newBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newBlock.transform.rotation = Quaternion.identity;
                newBlock.MovementMultiplier = Block.DefaultMovementMultiplier;
                
                return newBlock;
            } break;

            default: return null;
        }
    }


    public static BlockType GetPlayerNextBlockType(Player id) {
        switch (id) {
            case Player.PLAYER_1: {
                return p1NextBlockType;
            }
                
            case Player.PLAYER_2: {
                return p2NextBlockType;
            }

            default: return BlockType.INVALID;
        }
    }


    // NOTE(Zack): for internal usage for this class
    private static BlockType RandomBlockType() {
        return (BlockType)UnityEngine.Random.Range(0, (int)BlockType.MAX_BLOCKS);
    }
}
