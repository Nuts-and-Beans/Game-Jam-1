using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  [SerializeField] private TMP_Text[] playerStatusText = Array.Empty<TMP_Text>();
  [SerializeField] private string playerInactiveText   = "Press to join...";
  [SerializeField] private string playerActiveText     = "Joined!";
  [Space]
  [SerializeField] private int nextSceneIndex       = 1;
  [SerializeField] private float nextSceneWaitTimer = 1.5f;
  
  private bool _loadingScene = false;

  private void Awake()
  {
    Debug.Assert(playerStatusText.Length == PlayerInput.MaxPlayerCount, "Player Status Text length doesn't match max player count!");
    
    // Force all players to disconnect on starting this scene so we can detect new players
    PlayerInput.RemoveAllPlayers();
    
    PlayerInput.OnPlayerJoined += OnPlayerJoined;
    PlayerInput.StartSearchingForPlayers();
  }

  private void Start() => UpdateUI();

  private void OnDestroy()
  {
    PlayerInput.OnPlayerJoined -= OnPlayerJoined;
    PlayerInput.StopSearchingForPlayers();
  }

  private void OnPlayerJoined(int playerIndex)
  {
    UpdateUI();
    
    if (!_loadingScene && PlayerInput.PlayerCount == PlayerInput.MaxPlayerCount)
    {
      _loadingScene = true;
      StartCoroutine(LoadNextScene()); // TODO(WSWhitehouse): Remove delegate alloc
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void UpdateUI()
  {
    for (int i = 0; i < PlayerInput.MaxPlayerCount; i++)
    {
      bool playerActive = PlayerInput.IsPlayerValid(i);
      playerStatusText[i].text = playerActive ? playerActiveText : playerInactiveText;
    }
  }
  
  private IEnumerator LoadNextScene()
  {
    yield return new WaitForSeconds(nextSceneWaitTimer);
    SceneManager.LoadScene(nextSceneIndex);
  }
}