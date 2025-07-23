using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] ButtonBase _startButton;
    [SerializeField] ButtonBase _resetButton;
    [SerializeField] Text[] _texts;
    [SerializeField] FadeUI _fadeUI;
    Action _startAction;
    Action _resetAction;
    private async void Start()
    {
        await Initialize();
    }

    async UniTask Initialize()
    {
        RankingSystem.RankingLoad();

        var list = RankingSystem.ScoreList.OrderByDescending(x => x).ToArray();

        for (int i = 0; i < _texts.Length; i++)
        {
            int score = i < list.Length ? list[i] : 0;
            _texts[i].text = $"{i + 1}位:{score:00000}";
        }

        await Fade(1, 0, null);

        _startAction = async () => await Fade(0, 1, () => SceneChanger.SceneChange((int)SceneType.Ingame));
        _resetAction = () =>
        {
            RankingSystem.RankingReset();
            for (int i = 0; i < _texts.Length; i++)
            {
                _texts[i].text = $"{i + 1}位:00000";
            }
        };

        _startButton.OnClick += _startAction;
        _resetButton.OnClick += _resetAction;
    }

    async UniTask Fade(int start, int target, Action onComplete)
    {
        await _fadeUI.StartFade(start, target);
        onComplete?.Invoke();
    }

    private void OnDisable()
    {
        _startButton.OnClick -= _startAction;
        _resetButton.OnClick -= _resetAction;
    }
}
