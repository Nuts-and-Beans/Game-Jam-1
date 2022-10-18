using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text[] playerStatusText = Array.Empty<TMP_Text>();
    [SerializeField] private string playerActiveText = "Joined!";
    [Space]
    [SerializeField] private int nextSceneIndex = 1;
    [SerializeField] private float nextSceneWaitTimer = 1.5f;

    private bool _loadingScene = false;

    private delegate IEnumerator LoadNextSceneDel();
    private LoadNextSceneDel LoadNextScene;

    private void Awake()
    {
        LoadNextScene = __LoadNextScene;
        Debug.Assert(playerStatusText.Length == PlayerInput.MaxPlayerCount, "Player Status Text length doesn't match max player count!");

        // Force all players to disconnect on starting this scene so we can detect new players
        PlayerInput.RemoveAllPlayers();

        PlayerInput.OnPlayerJoined += OnPlayerJoined;
        PlayerInput.StartSearchingForPlayers();
    }

    private void Start()
    {
        AudioManager.Play("Theme");
        UpdateUI();
    }

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
            StartCoroutine(LoadNextScene());
            

        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateUI()
    {
        for (int i = 0; i < PlayerInput.MaxPlayerCount; i++)
        {
            bool playerActive = PlayerInput.IsPlayerValid(i);
            if (!playerActive) continue;
            playerStatusText[i].text = playerActiveText;
        }
    }

    private IEnumerator __LoadNextScene()
    {
        yield return new WaitForSeconds(nextSceneWaitTimer);
        SceneManager.LoadScene(nextSceneIndex);
        AudioManager.Stop("Theme");
    }

#if UNITY_EDITOR
    /// <summary>
    /// This is an editor only function! Do NOT call it from
    /// normal gameplay code!
    /// </summary>
    public void EDITOR_LoadNextScene()
    {
        _loadingScene = true;
        SceneManager.LoadScene(nextSceneIndex);
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MainMenu))]
    public class MainMenuEditor : ImprovedEditor<MainMenu>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(!Application.isPlaying))
            {
                if (GUILayout.Button("Load Next Scene"))
                {
                    Target.EDITOR_LoadNextScene();
                }
            }
        }
    }
}
#endif
