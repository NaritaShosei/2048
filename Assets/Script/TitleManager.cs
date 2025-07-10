using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] ButtonBase _startButton;
    [SerializeField] Text[] _texts;
    private void Start()
    {
        _startButton.OnClick += () => SceneChanger.SceneChange((int)SceneType.Ingame);
        RankingSystem.RankingLoad();
        var list = RankingSystem.ScoreList.OrderByDescending(x => x).ToArray();
        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i].text = $"{i + 1}位:{list[i]:00000}";
        }

    }
    private void OnDisable()
    {
        _startButton.OnClick -= () => SceneChanger.SceneChange((int)SceneType.Ingame);
    }
}
