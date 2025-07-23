using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class BoardView : MonoBehaviour
{
    [SerializeField] GameObject _cellContainer;
    [SerializeField] Text _scoreText;
    [SerializeField] GameObject _cellPrefab;
    List<CellView> _cells;
    [SerializeField] GridLayoutGroup _layoutGroup;
    public void Initialize()
    {
        _cells = new List<CellView>();
        for (int i = 0; i < 16; i++)
        {
            _cells.Add(Instantiate(_cellPrefab.GetComponent<CellView>(), _cellContainer.transform));
        }
    }
    public void SetScore(int score)
    {
        _scoreText.text = $"{score}";
    }

    public void SetBoard(int[,] board)
    {
        for (int i = 0; i < board.GetLength(0) * board.GetLength(1); i++)
        {
            int r = i / board.GetLength(1);
            int c = i % board.GetLength(0);

            int n = board[r, c];

            if (n == 0)
            {
                _cells[i].Hide();
            }
            else
            {
                _cells[i].Set(n);
            }
        }
    }

    public async UniTask CellMoveAnimation(Dictionary<int, BoardPosition> data, int[,] originalBoard)
    {
        _layoutGroup.enabled = false;
        Dictionary<int, Vector2> targetPositions = new Dictionary<int, Vector2>();
        foreach (var item in data)
        {
            int fromRow = item.Key / 4;
            int fromCol = item.Key % 4;

            var target = item.Value;
            int index = target.Row * 4 + target.Column;
            if (originalBoard[fromRow, fromCol] != 0)
            {
                targetPositions[item.Key] = (((RectTransform)(_cells[index].transform)).anchoredPosition);
            }
        }

        foreach (var item in targetPositions)
        {
            _cells[item.Key].MoveAnimation(item.Value);
        }
        await UniTask.Delay(300, cancellationToken: destroyCancellationToken);
        _layoutGroup.enabled = true;
    }
}
/// <summary>
/// セルが存在する座標を保存する
/// </summary>
public struct BoardPosition : IEquatable<BoardPosition>
{
    int _row;
    int _column;
    public int Row => _row;
    public int Column => _column;

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public BoardPosition(int row, int column)
    {
        _row = row;
        _column = column;
    }
    /// <summary>
    /// 等価比較
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(BoardPosition other)
    {
        return _row == other._row && _column == other._column;
    }
    /// <summary>
    /// Hash値の取得
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(_row, _column);
    }
    /// <summary>
    /// 同じ値かどうか
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(BoardPosition a, BoardPosition b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// 違う値かどうか
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(BoardPosition a, BoardPosition b)
    {
        return !a.Equals(b);
    }
}