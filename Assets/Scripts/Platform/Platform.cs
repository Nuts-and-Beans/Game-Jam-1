using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public List<Transform> platform_list;
    [SerializeField] private int p1_layer;
    [SerializeField] private int p2_layer;

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

            foreach(Transform child in p1_platforms[i].transform) {
                child.gameObject.layer = p1_layer;
            }
            
            p1_platforms[i].gameObject.SetActive(false);
            
            p2_platforms.Add(i, Instantiate(platform_list[i]));
            p2_platforms[i].localPosition= (Vector3.right*4)+(Vector3.down*4);

            foreach(Transform child in p2_platforms[i].transform) {
                child.gameObject.layer = p2_layer;
            }
            
            p2_platforms[i].gameObject.SetActive(false);
        }
        int randnum = Random.Range(0, platform_list.Capacity - 1);
        p1_platforms[randnum].gameObject.SetActive(true);
        p1_platforms[randnum].gameObject.layer = p1_layer;
        p2_platforms[randnum].gameObject.SetActive(true);
        p2_platforms[randnum].gameObject.layer = p2_layer;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void spawnPlatforms()
    {
     p1_platforms[prev_plat].gameObject.SetActive(false);
     p2_platforms[prev_plat].gameObject.SetActive(false);
     int randnum = Random.Range(0, platform_list.Capacity);
     p1_platforms[randnum].gameObject.SetActive(true);
     p2_platforms[randnum].gameObject.SetActive(true);
     prev_plat = randnum;
    }

}
