using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GoalLine : MonoBehaviour
{
  [SerializeField] private Player player;
  [Tooltip("How long blocks can stay in the trigger before calling it a win!")]
  [SerializeField] private float timer;
  [SerializeField] private float delay;
  
  private int _blockCount    = 0;
  private float _timer       = 0.0f;
  private bool _timerReached = false;


  private Coroutine lineReachedCo;
  
  private void OnTriggerStay2D(Collider2D other)
  {
    Block block = null;
    if (other.transform.parent != null)
    {
      block = other.transform.parent.GetComponent<Block>();
    }
    else
    {
      block = other.GetComponent<Block>();
    }
    
    if (block == null) return;
    if (block.IsControlled) return;
    if (block.PlayerID != player) return;
    
     // NOTE(WSWhitehouse): Clamping block count so it can't go below 0
     _blockCount = Mathf.Clamp(_blockCount + 1, 0, int.MaxValue);

     if (lineReachedCo != null) return;
     lineReachedCo = StartCoroutine(GoalLineReached());
    }

  private void OnTriggerExit2D(Collider2D other)
  {
    Block block = other.GetComponent<Block>();
    
    if (block == null) return;
    if (block.IsControlled) return;
    if (block.PlayerID != player) return;
    
    // NOTE(WSWhitehouse): Clamping block count so it can't go below 0
    _blockCount = Mathf.Clamp(_blockCount - 1, 0, int.MaxValue);

    if (lineReachedCo == null) return;
    StopCoroutine(GoalLineReached());
  }

  private IEnumerator GoalLineReached()
  {
    // TODO: Fire end game UI and stuff...
    Debug.Log($"Player {(((int)player) + 1).ToString()} reached the goal line!");

    float elapsed = 0f;
    while (elapsed < timer)
    {
     elapsed += Time.deltaTime;
     yield return null;
    }
    
    GameManager.playerwon = player;

    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene(2);
  }
}
