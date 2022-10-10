using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    [SerializeField] private Block prefab;
    [SerializeField] private int initialSpawnAmount;

    private static BlockPool Instance;

    private static Stack<Block> Pool;
    private static List<Block> ActiveBlocks;

    public bool spawnBlock = false; //Seb - To spawn new blocks for testing

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Block Pools active!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        // Create pool variables
        Pool = new Stack<Block>(initialSpawnAmount);
        ActiveBlocks = new List<Block>(initialSpawnAmount);

        // Spawn initial blocks
        for (int i = 0; i < initialSpawnAmount; i++)
        {
            Block block = SpawnNewBlock();
            block.SetActive(false);
            Pool.Push(block);
        }
    }

    private void FixedUpdate()
    {
        if (spawnBlock) //Seb - to spawn new blocks for testing
        {
            spawnBlock = false;
            GetBlock();
        }
    }

    private void OnDestroy()
    {
        if (Instance != this) return;

        Instance = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Block SpawnNewBlock() => Instantiate(Instance.prefab, Vector3.zero, Quaternion.identity, Instance.transform);

    public static Block GetBlock()
    {
        Debug.Assert(Instance != null, "A Block Pool Instance cannot be found! Please ensure one is placed in the scene.");

        Block block;
        if (Pool.Count <= 0)
        {
            Debug.LogWarning("Spawning new blocks for pool! Consider resizing initial spawn amount.");
            block = SpawnNewBlock();
        }
        else
        {
            block = Pool.Pop();
        }

        block.SetActive(true);
        ActiveBlocks.Add(block);
        return block;
    }

    public static void ReturnBlock(Block block)
    {
        Debug.Assert(Instance != null, "A Block Pool Instance cannot be found! Please ensure one is placed in the scene.");

        if (!ActiveBlocks.Contains(block))
        {
            Debug.LogWarning("Returning block that cannot be found in the ActiveBlocks list. Possibly returning a block not spawned by the pool... Please do NOT spawn blocks - use BlockPool.GetBlock()");
        }

        block.SetActive(false);
        ActiveBlocks.Remove(block);
        Pool.Push(block);
    }

    public int GetActiveBlocks()
    {
        return ActiveBlocks.Count;
    }

    // Seb - test spawn block
    public void spawnBlockTEST()
    {
        spawnBlock = true;
    }

}
