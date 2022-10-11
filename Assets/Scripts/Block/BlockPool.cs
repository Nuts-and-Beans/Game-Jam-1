using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
  [SerializeField] private Block[] prefabs;
  [SerializeField] private int initialSpawnAmount;
  
  private static BlockPool Instance;
  
  private static Dictionary<BlockType, Stack<Block>> Pools;
  private static Dictionary<BlockType, int> BlockTypeIndexs;
  private static List<Block> ActiveBlocks;

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
    Pools           = new Dictionary<BlockType, Stack<Block>>(prefabs.Length);
    ActiveBlocks    = new List<Block>(initialSpawnAmount * prefabs.Length);
    BlockTypeIndexs = new (prefabs.Length);

    // Spawn initial block pools
    for (int j = 0; j < prefabs.Length; ++j)
    {
      // add the block type to a dictionary for the index into the prefabs array (to make it easier to instantiate the blocks)
      BlockTypeIndexs.Add(prefabs[j].Type, j);      
      Pools.Add(prefabs[j].Type, new Stack<Block>(initialSpawnAmount));
      
      for (int i = 0; i < initialSpawnAmount; i++)
      {
        Block block = SpawnNewBlock(prefabs[j].Type);

        block.SetActive(false);
        Pools[block.Type].Push(block);        
      }
    }
  }

  private void OnDestroy()
  {
    if (Instance != this) return;
    
    Instance = null;
  }

  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static Block SpawnNewBlock(BlockType type) => Instantiate(Instance.prefabs[BlockTypeIndexs[type]], Vector3.zero, Quaternion.identity, Instance.transform);
  
  public static Block GetBlock(BlockType type)
  {
    Debug.Assert(Instance != null, "A Block Pool Instance cannot be found! Please ensure one is placed in the scene.");
    
    Block block;
    if (Pools[type].Count <= 0)
    {
      Debug.LogWarning("Spawning new blocks for pool! Consider resizing initial spawn amount.");
      block = SpawnNewBlock(type);
    }
    else
    {
      block = Pools[type].Pop();
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
    Pools[block.Type].Push(block);
  }
}
