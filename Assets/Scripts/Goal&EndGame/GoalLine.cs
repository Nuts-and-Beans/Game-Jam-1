﻿using UnityEngine;
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


  private void OnTriggerEnter2D(Collider2D other)
  {
    Block block = other.GetComponent<Block>();
    
    if (block == null) return;
    if (block.PlayerID != player) return;
    
    // NOTE(WSWhitehouse): Clamping block count so it can't go below 0
    _blockCount = Mathf.Clamp(_blockCount + 1, 0, int.MaxValue);

        StartCoroutine(GoalLineReached());
    }

  private void OnTriggerExit2D(Collider2D other)
  {
    Block block = other.GetComponent<Block>();
    
    if (block == null) return;
    if (block.PlayerID != player) return;
    
    // NOTE(WSWhitehouse): Clamping block count so it can't go below 0
    _blockCount = Mathf.Clamp(_blockCount - 1, 0, int.MaxValue);
  }

  private void Update()
  {
    if (_timerReached) return;
    
    if (_blockCount <= 0)
    {
      _timer = 0.0f;
      return;
    }
    
    _timer += Time.deltaTime;

    if (_timer >= timer)
    {
      _timerReached = true;
      StartCoroutine (GoalLineReached());
    }
  }

  private IEnumerator GoalLineReached()
  {
    // TODO: Fire end game UI and stuff...
    Debug.Log($"Player {(((int)player) + 1).ToString()} reached the goal line!");

    GameManager.playerwon = player;

    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene(2);

  }
}