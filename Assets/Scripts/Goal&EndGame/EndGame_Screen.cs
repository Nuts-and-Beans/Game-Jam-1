using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGame_Screen : MonoBehaviour
{
    public TMP_Text winID;
    public GameObject replay;
    [SerializeField] int delay;
    //public Text score1;

    // Update is called once per frame
    void Start()
    {
        replay.SetActive(false);
        StartCoroutine(endGame());
    }

    IEnumerator endGame() 
    {
        if (GameManager.playerwon == Player.INVALID)
        {
            winID.text = ($"TIMEOUT! NO WINNER.");
        }
        else
        {
            winID.text = ($"WINNER P{(((int)GameManager.playerwon) + 1).ToString()}");
        }
      
        yield return new WaitForSeconds(delay);

        replay.SetActive(true);
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(0);
    }
}
