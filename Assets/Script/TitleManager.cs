using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] ButtonBase _startButton;
    [SerializeField] ButtonBase _resetButton;
    [SerializeField] Text[] _texts;

    System.Action _startAction;
    System.Action _resetAction;
    private void Start()
    {
        _startAction = () => SceneChanger.SceneChange((int)SceneType.Ingame);

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

        RankingSystem.RankingLoad();

        var list = RankingSystem.ScoreList.OrderByDescending(x => x).ToArray();

        for (int i = 0; i < _texts.Length; i++)
        {
            int score = i < list.Length ? list[i] : 0;
            _texts[i].text = $"{i + 1}位:{score:00000}";
        }
    }
    private void OnDisable()
    {
        _startButton.OnClick -= _startAction;
        _resetButton.OnClick -= _resetAction;
    }
}
