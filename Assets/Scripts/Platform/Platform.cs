using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<Transform> platform_list;

    private Transform p1_plat;
    private Transform p2_plat;
    void Start()
    {
        int platnumber = Random.Range(0, platform_list.Capacity-1);
        p1_plat = Instantiate(platform_list[platnumber]);
        p2_plat = Instantiate(platform_list[platnumber]);
        p1_plat.localPosition = (Vector3.left * 4)+(Vector3.down*4);
        p2_plat.localPosition = (Vector3.right*4)+(Vector3.down*4);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void onGameEnd()
    {
        p1_plat.parent.gameObject.SetActive(false);
        p2_plat.parent.gameObject.SetActive(false);
    }
}
