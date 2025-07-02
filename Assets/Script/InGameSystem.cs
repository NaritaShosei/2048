using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameSystem : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] BoardView _boardView;
    public int[,] Board { get; private set; }
    public int Score { get; private set; }
    bool _isUpdate;

    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        var board = new int[4, 4];
        board[Random.Range(0, 4), Random.Range(0, 4)] = Random.Range(0, 10) < 5 ? 2 : 4;
        InitializeGame(board, Score);

        _boardView.Initialize();
        _boardView.Set(Board);

        yield return new WaitForSeconds(1);

        _input.FindAction("Up").performed += Up;
        _input.FindAction("Down").performed += Down;
        _input.FindAction("Left").performed += Left;
        _input.FindAction("Right").performed += Right;

    }
    void SpawnCell(BoardPosition pos, int value)
    {
        Board[pos.Row, pos.Column] = value;
        _boardView.Set(Board);
    }

    BoardPosition[] GetEmptyBoardPosition()
    {
        List<BoardPosition> list = new List<BoardPosition>();
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(1); k++)
            {
                if (Board[i, k] == 0)
                {
                    list.Add(new BoardPosition(i, k));
                }
            }
        }
        return list.ToArray();
    }

    void InitializeGame(int[,] board, int score)
    {
        Board = board;
        Score = score;
    }
    void Up(InputAction.CallbackContext context)
    {

    }
    void Down(InputAction.CallbackContext context)
    {

    }
    void Left(InputAction.CallbackContext context)
    {

    }
    void Right(InputAction.CallbackContext context)
    {

    }
    [ContextMenu("Update")]
    public void StartC()
    {
        StartCoroutine(nameof(UpdateCells));
    }

    IEnumerator UpdateCells()
    {
        _isUpdate = true;

        var emptyPositions = GetEmptyBoardPosition();
        if (emptyPositions.Length > 0)
        {
            var value = Random.Range(0, 10) < 5 ? 2 : 4;
            var randomPos = emptyPositions[Random.Range(0, emptyPositions.Length)];
            SpawnCell(randomPos, value);
        }

        yield return null;
        _isUpdate = false;
    }
}
enum InputType
{
    Up,
    Down,
    Left,
    Right
}
