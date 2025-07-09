using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] ButtonBase _startButton;
    private void Start()
    {
        _startButton.OnClick += () => SceneChanger.SceneChange((int)SceneType.Ingame);
    }
}
