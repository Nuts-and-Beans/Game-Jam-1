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
    public TMP_Text p1Score;
    public TMP_Text p2Score;
    public int scoreMultiplier = 100;

    // Update is called once per frame
    void Start()
    {
        replay.SetActive(false);
        StartCoroutine(endGame());
        AudioManager.Play("Win");
    }

    IEnumerator endGame() 
    {
        if (GameManager.playerwon == Player.INVALID)
        {
            winID.text = ($"TIMEOUT! NO WINNER.");
            AudioManager.Play("NoWin");
        }
        else
        {
            winID.text = ($"WINNER P{(((int)GameManager.playerwon) + 1).ToString()}");
            AudioManager.Play("Win");
        }
        
        p1Score.text = $"P1 {Random_Spawn.p1Score * scoreMultiplier}";
        p2Score.text = $"P2 {Random_Spawn.p2Score * scoreMultiplier}";
      
        yield return new WaitForSeconds(delay);

        replay.SetActive(true);
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(0);
    }
}
