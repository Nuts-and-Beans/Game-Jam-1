using System;
using TMPro;
using UnityEngine;

public class TimeLimitUI : MonoBehaviour
{
  [SerializeField] private TMP_Text timelimit;

  private void Update()
  {
    float time  = GameManager.GameTime - GameManager.CurrentGameTime;
    time = Mathf.Clamp(time, 0.0f, float.MaxValue);
    
    int minutes = (int)Mathf.Floor(time / 60f);
    int seconds = (int)time - (60 * minutes);

    // NOTE(WSWhitehouse): Yes, thats two boxing allocations... sue me.
    timelimit.text = $"{minutes:00}:{seconds:00}";
  }
}