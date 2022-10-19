using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Camera camera;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 45.0f;
    [SerializeField] private Vector2 worldBounds;
    [SerializeField] private float playerSeparator;

    [Header("Physics Settings")]
    [Range(0.0f, 1.0f)] public float PhysicsRange = 0.5f;
    
    // --- Static Variables --- //
    public static AudioManager AudioManager { get; private set; }
    public static Camera Camera { get; private set; }
    public static float GameTime { get; private set; }
    public static float CurrentGameTime { get; private set; }
    public static Vector2 WorldBounds { get; private set; }
    public static Vector2 HalfWorldBounds { get; private set; }
    public static float PlayerSeparator { get; private set; }
    public static Player playerwon {get; set;}

    private void Awake()
    {
        // Set the static data
        AudioManager = FindObjectOfType<AudioManager>();
        Camera = camera;
        GameTime = gameTime;
        WorldBounds = worldBounds;
        HalfWorldBounds = WorldBounds * 0.5f;
        PlayerSeparator = playerSeparator;
        playerwon = Player.INVALID;
        CurrentGameTime = 0.0f;
    }

    private void FixedUpdate()
    {
        // If random number is higher than PhysicsRange, then don't update physics
        if (Random.Range(0.0f, 1.0f) > PhysicsRange) return;

        Physics2D.Simulate(Time.deltaTime);
        CurrentGameTime += Time.deltaTime;
        
        if (CurrentGameTime >= GameTime)
        {
          playerwon = Player.INVALID;
          SceneManager.LoadScene(2);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, worldBounds);

        float separatorY = worldBounds.y * 0.5f;
        Vector2 sepTop = new Vector2(playerSeparator, separatorY);
        Vector2 sepBottom = new Vector2(playerSeparator, -separatorY);
        Gizmos.DrawLine(sepTop, sepBottom);
    }
#endif
}
