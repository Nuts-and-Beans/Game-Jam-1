using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingBlock : MonoBehaviour
{
    public GameObject position1;
    public GameObject position2;
    // Start is called before the first frame update
    void Start()
    {
        int ran_num = Random.Range(0, 7);

        Block current_block1 = BlockPool.GetBlock((BlockType)ran_num);

        current_block1.gameObject.GetComponent<Random_Spawn>().control = true;
             
        current_block1.PlayerID = Player.PLAYER_1;
        current_block1.gameObject.GetComponent<Random_Spawn>().player = 1;

        current_block1.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        current_block1.gameObject.transform.position = position1.transform.position;

        Block current_block2 = BlockPool.GetBlock((BlockType)ran_num);

        current_block2.gameObject.GetComponent<Random_Spawn>().control = true;
        current_block2.PlayerID = Player.PLAYER_2;
        Debug.Log((current_block2.PlayerID));
        current_block2.gameObject.GetComponent<Random_Spawn>().player = 2;

        current_block2.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        current_block2.gameObject.transform.position = position2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
