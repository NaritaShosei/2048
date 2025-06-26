using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    [SerializeField] GameObject _cellContainer;
    [SerializeField] Text _scoreText;

    void Start()
    {

    }

    void UpdateView()
    {

    }

    // Update is called once per frame
    void Update()
    {

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