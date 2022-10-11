using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Random_Spawn : MonoBehaviour
{
    public GameObject spawn;
    public BlockPool block;




    // Update is called once per frame
    void Update()
    {
        RandomBlockSpawn();
    }

    // spawns random block based on random number generated
    void RandomBlockSpawn()
    {
        Random.Range(1, 8);


    }

}
