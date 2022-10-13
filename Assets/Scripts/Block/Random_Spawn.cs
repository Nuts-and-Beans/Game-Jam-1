using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Random_Spawn : MonoBehaviour
{

    [SerializeField] private GameObject spawn_1;
    [SerializeField] private GameObject spawn_2;


    // Update is called once per frame
    void Update()
    {
        RandomBlockSelect();
    }

    // spawns random block based on random number generated
    void RandomBlockSelect() 
    {

    }

}
