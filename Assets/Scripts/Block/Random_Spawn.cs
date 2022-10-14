using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Random_Spawn : MonoBehaviour
{

    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    private GameObject spawn;

    public int player = 1;
    private TestControl _controlledBlock;
    

    private void Awake()
    {

        if (player == 1)
        {
            spawn = GameObject.FindGameObjectWithTag("P1Spawn");
        }
        if (player == 2)
        {
            spawn = GameObject.FindGameObjectWithTag("P2Spawn");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    // spawns random block based on random number generated
    private void OnCollisionEnter2D(Collision2D col)
    {
        
        var position = spawn.transform.position;
        _controlledBlock = col.gameObject.GetComponent<TestControl>();




        if (_controlledBlock.control)
        {
            int ran_num = Random.Range(1, 3);

            if (ran_num == 1)
            {
                GameObject newObject = Instantiate(block1, position, Quaternion.identity) as GameObject;
                newObject.transform.localScale = new Vector3(.5f, .5f, .5f);
                PlayerBlock(newObject);

                
            }
            if (ran_num == 2)
            {
                GameObject newObject = Instantiate(block2, position, Quaternion.identity) as GameObject;
                newObject.transform.localScale = new Vector3(.5f, .5f, .5f);
                PlayerBlock(newObject);
            }
            if (ran_num == 3)
            {
                GameObject newObject = Instantiate(block3, position, Quaternion.identity) as GameObject;
                newObject.transform.localScale = new Vector3(.5f, .5f, .5f);
                PlayerBlock(newObject);
            }
            _controlledBlock.control = false;
          
        }
        
    }

    private void PlayerBlock(GameObject ob)
    {
        if (player == 1)
        {
            ob.GetComponent<Block>().PlayerID = Player.PLAYER_1;
            ob.GetComponent<Random_Spawn>().player = 1;
        }
        if (player == 2)
        {
            ob.GetComponent<Block>().PlayerID = Player.PLAYER_2;
            ob.GetComponent<Random_Spawn>().player = 2;
        }

    }


}
