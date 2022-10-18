using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class BlockKillVolume : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col) {
        // HACK(Zack): we're checking the parent because the colliders for blocks are children of the object
        Block block = null;
        if (col.transform.parent != null) {
            block = col.transform.parent.GetComponent<Block>();
        } else {
            block = col.GetComponent<Block>();
        }

        if (block == null) return;
        BlockPool.ReturnBlock(block);
    }
}
