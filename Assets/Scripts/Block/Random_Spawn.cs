using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Random_Spawn : MonoBehaviour
{

   
    private GameObject spawn;
    public bool control = true;
    public int player = 1;
  
    

    private void Awake()
    {

    
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    // spawns random block based on random number generated
    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.GetComponent<Block>() == null)
        {
            if (control == true)
            {


                control = false;

                int ran_num = Random.Range(0, 7);

                Block current_block = BlockPool.GetBlock((BlockType)ran_num);

                current_block.gameObject.GetComponent<Random_Spawn>().control = true;
                current_block.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                if (player == 1)
                {
                    current_block.PlayerID = Player.PLAYER_1;
                    current_block.gameObject.GetComponent<Random_Spawn>().player = 1;
                    spawn = GameObject.FindGameObjectWithTag("P1Spawn");

                }
                if (player == 2)
                {
                    current_block.PlayerID = Player.PLAYER_2;
                    Debug.Log((current_block.PlayerID));
                    current_block.gameObject.GetComponent<Random_Spawn>().player = 2;
                    spawn = GameObject.FindGameObjectWithTag("P2Spawn");

                }


                var position = spawn.transform.position;
               
                current_block.gameObject.transform.position = position;
                
            }
            return;
        }




       
        {

            if (control == true && col.gameObject.GetComponent<Random_Spawn>().control == false)
        {
               
         
                control = false;

                int ran_num = Random.Range(0, 7);

                Block current_block = BlockPool.GetBlock((BlockType)ran_num);
              
                current_block.gameObject.GetComponent<Random_Spawn>().control = true;

                if (col.gameObject.GetComponent<Random_Spawn>().player == 1)
                {
                    current_block.PlayerID = Player.PLAYER_1;
                    current_block.gameObject.GetComponent<Random_Spawn>().player = 1;
                    spawn = GameObject.FindGameObjectWithTag("P1Spawn");

                }
                if (col.gameObject.GetComponent<Random_Spawn>().player == 2)
                {
                current_block.PlayerID = Player.PLAYER_2;
                Debug.Log((current_block.PlayerID));
                current_block.gameObject.GetComponent<Random_Spawn>().player = 2;
                    spawn = GameObject.FindGameObjectWithTag("P2Spawn");

                }


                var position = spawn.transform.position;
                current_block.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            current_block.gameObject.transform.position = position;
             
            }

        }
        
    }



}
