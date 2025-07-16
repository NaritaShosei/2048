using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FlickInputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] InGameSystem _gameSystem;
    Vector2 _startPosition;
    Vector2 _currentPosition;
    bool _isUIClick;
    private void Awake()
    {
        _input.FindAction("Crick").started += OnPointerDown;
        _input.FindAction("Crick").canceled += OnPointerUp;
        _input.FindAction("Pointer").performed += OnPointerPosition;
    }
    private void OnDisable()
    {
        _input.FindAction("Crick").started -= OnPointerDown; ;
        _input.FindAction("Crick").canceled -= OnPointerUp; ;
        _input.FindAction("Pointer").performed -= OnPointerPosition;
    }

    void OnPointerDown(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // UI上を押しているのでフリック処理を無視
            Debug.Log("UI上をタッチしたのでフリック処理はスキップ");
            _isUIClick = true;
            return;
        }
        Debug.Log("DOWN");
        _startPosition = _currentPosition;
        _isUIClick = false;
    }

    void OnPointerPosition(InputAction.CallbackContext context)
    {
        _currentPosition = context.ReadValue<Vector2>();
    }

    void OnPointerUp(InputAction.CallbackContext context)
    {
        if (_isUIClick) { Debug.Log("UI上をクリックしていたためフリックは実行されません"); return; }
        Debug.Log("UP");
        Vector2 endPosition = _currentPosition;
        Vector2 flickDir = (endPosition - _startPosition).normalized;

        if (Mathf.Abs(flickDir.x) < Mathf.Abs(flickDir.y))
        {
            if (flickDir.y > 0)
            {
                _gameSystem.Flick(InputType.Up);
            }
            else
            {
                _gameSystem.Flick(InputType.Down);
            }
        }
        else
        {
            if (flickDir.x > 0)
            {
                _gameSystem.Flick(InputType.Right);
            }
            else
            {
                _gameSystem.Flick(InputType.Left);
            }
        }
    }
}
