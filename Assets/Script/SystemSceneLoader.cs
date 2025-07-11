﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemSceneLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        if (!SceneManager.GetSceneByName("SystemScene").isLoaded)
        {
            SceneManager.LoadScene("SystemScene", LoadSceneMode.Additive);
        }
    }
}
