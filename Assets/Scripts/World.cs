using System;
using UnityEngine;

public class World : MonoBehaviour
{
  [SerializeField] private Vector2 worldBounds;

  public static Vector2 WorldBounds     { get; private set; }
  public static Vector2 HalfWorldBounds { get; private set; }

  private void Awake()
  {
    // Set the static data
    WorldBounds     = worldBounds;
    HalfWorldBounds = WorldBounds * 0.5f;
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, worldBounds);
  }
#endif
}