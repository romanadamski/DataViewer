using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class PlayModeController : EditorWindow
{
    [MenuItem("HotKey/Run _F5")]
    private static void PlayGame()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
        else
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(YieldReplayGame());
        }
    }

    private static IEnumerator YieldReplayGame()
    {
        EditorApplication.ExecuteMenuItem("Edit/Play");
        yield return new WaitUntil(() => !EditorApplication.isPlaying);
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    //[MenuItem("HotKey/Reset prefs and run _F4")]
    //public static void ResetPrefsAndPlay()
    //{
    //    if (!EditorApplication.isPlaying)
    //    {
    //        PlayWithResetPrefs();
    //    }
    //    else
    //    {
    //        EditorCoroutineUtility.StartCoroutineOwnerless(YieldResetPrefsAndPlay());
    //    }
    //}

    private static void PlayWithResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    private static IEnumerator YieldResetPrefsAndPlay()
    {
        EditorApplication.ExecuteMenuItem("Edit/Play");
        yield return new WaitUntil(() => !EditorApplication.isPlaying);
        PlayWithResetPrefs();
    }

    [MenuItem("HotKey/Open C# Project _F6")]
    private static void OpenCSharpProject()
    {
        EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
    }
}