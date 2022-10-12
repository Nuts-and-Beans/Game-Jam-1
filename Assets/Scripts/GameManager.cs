using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
  [Header("Scene References")]
  [SerializeField] private Camera camera;
  
  [Header("Game Settings")]
  [Tooltip("How long before the game official starts (i.e. when OnGameStart event is invoked)")]
  [SerializeField] private float gameStartTime = 5.0f;
  [SerializeField] private float gameTime      = 45.0f;
  [SerializeField] private Vector2 worldBounds;
  
  [Header("Physics Settings")]
  [Range(0.0f, 1.0f)] public float PhysicsRange = 0.5f;
  
  // --- Static Variables --- //
  public static AudioManager AudioManager { get; private set; }
  public static Camera Camera             { get; private set; }
  public static float GameTime            { get; private set; }
  public static float CurrentGameTime     { get; private set; }
  public static Vector2 WorldBounds       { get; private set; }
  public static Vector2 HalfWorldBounds   { get; private set; }

  // --- Static Events --- //
  // NOTE(WSWhitehouse): This event gets invoked when the game is started@
  public Action OnGameStart;
  
  // NOTE(WSWhitehouse): This event gets invoked when a player wins
  public delegate void GameWin(int playerID);
  public static GameWin OnGameWin;
  
  // NOTE(WSWhitehouse): this event gets invoked when the game is over (i.e. timeout)
  public static Action OnGameOver;

  private void Awake()
  {
    // Set the static data
    AudioManager    = FindObjectOfType<AudioManager>();
    Camera          = camera;
    GameTime        = gameTime;
    WorldBounds     = worldBounds;
    HalfWorldBounds = WorldBounds * 0.5f;
  }

  private IEnumerator Start()
  {
    yield return new WaitForSeconds(gameStartTime);
    OnGameStart?.Invoke();
  }

  private void FixedUpdate()
  {
    if (Random.Range(0.0f, 1.0f) > PhysicsRange) return;
    
    Physics2D.Simulate(Time.deltaTime);
    
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, worldBounds);
  }
#endif
}
