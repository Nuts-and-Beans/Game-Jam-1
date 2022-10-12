#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ImprovedEditor<T> : Editor
  where T : Object
{
  protected T Target => (T)target;
}
#endif
