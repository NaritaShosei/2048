using UnityEngine;

public class SystemSceneManager : MonoBehaviour
{
    bool _isInitialize = false;

    private void Awake()
    {
        if (_isInitialize)
        {
            Destroy(gameObject);
            return;
        }
        _isInitialize = true;
        DontDestroyOnLoad(gameObject);
    }
}
