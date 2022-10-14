using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<Transform> platform_list;

    private Dictionary<int, Transform> p1_platforms;
    private Dictionary<int, Transform> p2_platforms;
 
    
    private int prev_plat;
    void Start()
    {
        //instantiate platforms in separate dictionaries
        p1_platforms = new Dictionary<int, Transform>(platform_list.Capacity);
        p2_platforms = new Dictionary<int, Transform>(platform_list.Capacity);
        for(int i = 0; i < platform_list.Capacity-1; i++)
        {
            p1_platforms.Add(i, Instantiate(platform_list[i]));
            p1_platforms[i].localPosition = (Vector3.left * 4) + (Vector3.down * 4);
            p1_platforms[i].parent.gameObject.SetActive(false);
            
            p2_platforms.Add(i, Instantiate(platform_list[i]));
            p2_platforms[i].localPosition= (Vector3.right*4)+(Vector3.down*4);
            p2_platforms[i].parent.gameObject.SetActive(false);
        }
        int randnum = Random.Range(0, platform_list.Capacity - 1);
        p1_platforms[randnum].parent.gameObject.SetActive(true);
        p2_platforms[randnum].parent.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void spawnPlatforms()
    {
     p1_platforms[prev_plat].parent.gameObject.SetActive(false);
     p2_platforms[prev_plat].parent.gameObject.SetActive(false);
     int randnum = Random.Range(0, platform_list.Capacity);
     p1_platforms[randnum].parent.gameObject.SetActive(true);
     p2_platforms[randnum].parent.gameObject.SetActive(true);
     prev_plat = randnum;
    }

}
