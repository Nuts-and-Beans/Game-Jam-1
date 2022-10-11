using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Spawn : MonoBehaviour
{

    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject spawn;

    // Update is called once per frame
    void Update()
    {
        RandomBlockSpawn();
    }

    void RandomBlockSpawn()
    {
        var position = spawn.transform.position;

        if (Input.GetKeyDown ("space"))
        {
            int ran_num = Random.Range(1, 3);

            if (ran_num == 1)
            {
                Instantiate(block1, position, Quaternion.identity);
            }
            if (ran_num == 2)
            {
                Instantiate(block1, position, Quaternion.identity);
            }
            if (ran_num == 3)
            {
                Instantiate(block1, position, Quaternion.identity);
            }
        }
        
    }


}
