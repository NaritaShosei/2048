using System;
using System.Collections;
using System.Linq;
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
    private void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        RankingSystem.RankingLoad();

        var list = RankingSystem.ScoreList.OrderByDescending(x => x).ToArray();

        for (int i = 0; i < _texts.Length; i++)
        {
            int score = i < list.Length ? list[i] : 0;
            _texts[i].text = $"{i + 1}位:{score:00000}";
        }

        yield return StartCoroutine(Fade(1, 0, null));

        _startAction = () => StartCoroutine(Fade(0, 1, () => SceneChanger.SceneChange((int)SceneType.Ingame)));
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

    IEnumerator Fade(int start, int target, Action onComplete)
    {
        yield return StartCoroutine(_fadeUI.StartFade(start, target));
        onComplete?.Invoke();
    }

    private void OnDisable()
    {
        _startButton.OnClick -= _startAction;
        _resetButton.OnClick -= _resetAction;
    }
}
