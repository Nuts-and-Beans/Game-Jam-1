using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlockPool : MonoBehaviour
{
    [SerializeField] private Block[] prefabs;
    [SerializeField] private int initialSpawnAmount;
    private static BlockSlowdown blockSlowdown;

    private static BlockPool Instance;

    private static Dictionary<BlockType, Stack<Block>> Pools;
    private static Dictionary<BlockType, int> BlockTypeIndexs;
    private static List<Block> ActiveBlocks;
    private static int BlockTypeLength;
    private static int next_block_index;
    private static NextBlockUI nextBlockUI;

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
        Pools = new Dictionary<BlockType, Stack<Block>>(prefabs.Length);
        ActiveBlocks = new List<Block>(initialSpawnAmount * prefabs.Length);
        BlockTypeIndexs = new(prefabs.Length);

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

        // Find the block slowdown script
        blockSlowdown = FindObjectOfType<BlockSlowdown>();
        nextBlockUI = FindObjectOfType<NextBlockUI>();

        BlockTypeLength = BlockType.GetNames(typeof(BlockType)).Length - 1;

    }

    private void Start()
    {
        // NOTE(Seb): Must be called in the Start function as it won't show the correct block to begin with.
        setNextBlockIndex();
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
        blockSlowdown.UpdateSlowdownPhysics(ActiveBlocks.Count);
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
        blockSlowdown.UpdateSlowdownPhysics(ActiveBlocks.Count);
    }

    public int GetActiveBlocks()
    {
        return ActiveBlocks.Count;
    }

    private static void setNextBlockIndex()
    {
        // Set the next block index to a random number of valid block types
        // TODO(Seb): Maybe should set BlockTypeLength to a variable so it doesn't have to be calculated every time

        int rand_num = Random.Range(0, BlockTypeLength);
        next_block_index = rand_num;
        nextBlockUI.changeNextBlockUI((BlockType)next_block_index);
        Debug.Log("Next block is a :" + (BlockType)next_block_index);

    }

    //Button to spawn blocks for testing.
#if UNITY_EDITOR
    [CustomEditor(typeof(BlockPool))]
    public class BlockPoolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
                if (GUILayout.Button("Spawn Block"))
                {
                    Block current_block = GetBlock((BlockType)next_block_index);
                    current_block.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    setNextBlockIndex();
                }
                else if (GUILayout.Button("Remove Active Block"))
                {
                    if (ActiveBlocks.Count <= 0) return;
                    Block current_block = ActiveBlocks[0];
                    ReturnBlock(current_block);
                }
        }
    }
#endif
}
