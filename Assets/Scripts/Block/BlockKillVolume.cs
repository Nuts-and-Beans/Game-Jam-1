using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class BlockKillVolume : MonoBehaviour {
    public delegate void BlockKillEvent(Player playerID);
    public static BlockKillEvent OnControlledBlockKilled;
    
    private void OnTriggerEnter2D(Collider2D col) {
        // HACK(Zack): we're checking the parent because the colliders for blocks are children of the object
        Block block = null;
        if (col.transform.parent != null) {
            // HACK(Zack): this second check stops multiple calls to this function
            // if the parent has been disabled we return
            if (!col.transform.parent.gameObject.activeInHierarchy) return;
            
            block = col.transform.parent.GetComponent<Block>();
        } else {
            block = col.GetComponent<Block>();
        }

        if (block == null) return;

        // signal the player controller that they should not be controlling this block anymore
        if (block.IsControlled) {
            OnControlledBlockKilled?.Invoke(block.PlayerID);
        }
        
        Random_Spawn.ReturnBlock(block);
    }
}
