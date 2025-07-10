using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameSystem : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] BoardView _boardView;
    [SerializeField] FadeUI _fadeUI;
    public int[,] Board { get; private set; }
    public int[,] OriginalBoard { get; private set; }
    public int Score { get; private set; }
    bool _isUpdate;
    [SerializeField] int _probability = 5;
    Dictionary<int, BoardPosition> _moveData = new();
    InputType[] _inputTypes = new InputType[] { InputType.Up, InputType.Down, InputType.Left, InputType.Right };
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        var board = new int[4, 4];
        board[Random.Range(0, 4), Random.Range(0, 4)] = Random.Range(0, 10) < _probability ? 2 : 4;
        InitializeGame(board, 0);

        _boardView.Initialize();
        _boardView.SetBoard(Board);
        _boardView.SetScore(Score);

        yield return StartCoroutine(_fadeUI.StartFade(1, 0));

        _input.FindAction("Up").started += Up;
        _input.FindAction("Down").started += Down;
        _input.FindAction("Left").started += Left;
        _input.FindAction("Right").started += Right;

    }
    void SpawnCell(BoardPosition pos, int value)
    {
        Board[pos.Row, pos.Column] = value;
        _boardView.SetBoard(Board);
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
        if (_isUpdate) { return; }
        if (BoardSlide(InputType.Up)) StartCoroutine(nameof(UpdateCells));
    }
    void Down(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        if (BoardSlide(InputType.Down)) StartCoroutine(nameof(UpdateCells));
    }
    void Left(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        if (BoardSlide(InputType.Left)) StartCoroutine(nameof(UpdateCells));
    }
    void Right(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        if (BoardSlide(InputType.Right)) StartCoroutine(nameof(UpdateCells));
    }
    public void DumpBoard()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            sb.Append($"[{Board[i, 0]}");
            for (int k = 1; k < Board.GetLength(1); k++)
            {
                sb.Append($", {Board[i, k]}");
            }
            sb.Append("]" + (i != Board.GetLength(0) - 1 ? "\n" : ""));
        }
        Debug.Log(sb.ToString());
    }
    bool BoardSlide(InputType type)
    {
        OriginalBoard = Board.Clone() as int[,];

        _moveData = new();

        bool isMoved = false;
        bool[,] marged = new bool[Board.GetLength(0), Board.GetLength(1)];
        switch (type)
        {
            case InputType.Up:
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int k = 0; k < Board.GetLength(1); k++)
                    {
                        if (i == 0) { continue; }
                        if (Board[i, k] == 0) { continue; }

                        int index = i * 4 + k;

                        int currentRow = i;
                        while (currentRow > 0)
                        {
                            int nextRow = currentRow - 1;

                            if (Board[nextRow, k] == 0)
                            {
                                Board[nextRow, k] = Board[currentRow, k];
                                Board[currentRow, k] = 0;
                                currentRow--;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(nextRow, k);
                                continue;
                            }
                            if (Board[nextRow, k] == Board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                Board[nextRow, k] *= 2;
                                Score += Board[nextRow, k];
                                Board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(nextRow, k);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Down:
                for (int i = Board.GetLength(0) - 1; 0 <= i; i--)
                {
                    for (int k = 0; k < Board.GetLength(1); k++)
                    {
                        if (i == Board.GetLength(0) - 1) { continue; }
                        if (Board[i, k] == 0) { continue; }
                        int currentRow = i;
                        int index = i * 4 + k;
                        while (currentRow < Board.GetLength(0) - 1)
                        {
                            int nextRow = currentRow + 1;

                            if (Board[nextRow, k] == 0)
                            {
                                Board[nextRow, k] = Board[currentRow, k];
                                Board[currentRow, k] = 0;
                                currentRow++;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(nextRow, k);
                                continue;
                            }
                            if (Board[nextRow, k] == Board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                Board[nextRow, k] *= 2;
                                Score += Board[nextRow, k];
                                Board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
                                _moveData[index] = new BoardPosition(nextRow, k);
                                isMoved = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Left:
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int k = 0; k < Board.GetLength(1); k++)
                    {
                        if (k == 0) { continue; }
                        if (Board[i, k] == 0) { continue; }
                        int currentColumn = k;
                        int index = i * 4 + k;
                        while (currentColumn > 0)
                        {
                            int nextColumn = currentColumn - 1;

                            if (Board[i, nextColumn] == 0)
                            {
                                Board[i, nextColumn] = Board[i, currentColumn];
                                Board[i, currentColumn] = 0;
                                currentColumn--;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(i, nextColumn);
                                continue;
                            }
                            if (Board[i, nextColumn] == Board[i, currentColumn] && !marged[i, nextColumn] && !marged[i, currentColumn])
                            {
                                Board[i, nextColumn] *= 2;
                                Score += Board[i, nextColumn];
                                Board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(i, nextColumn);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Right:
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int k = Board.GetLength(1) - 1; 0 <= k; k--)
                    {
                        if (k == Board.GetLength(1) - 1) { continue; }
                        if (Board[i, k] == 0) { continue; }
                        int currentColumn = k;
                        int index = i * 4 + k;
                        while (currentColumn < Board.GetLength(1) - 1)
                        {
                            int nextColumn = currentColumn + 1;

                            if (Board[i, nextColumn] == 0)
                            {
                                Board[i, nextColumn] = Board[i, currentColumn];
                                Board[i, currentColumn] = 0;
                                currentColumn++;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(i, nextColumn);
                                continue;
                            }
                            if (Board[i, nextColumn] == Board[i, currentColumn] && !marged[i, nextColumn] && !marged[i, currentColumn])
                            {
                                Board[i, nextColumn] *= 2;
                                Score += Board[i, nextColumn];
                                Board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
                                isMoved = true;
                                _moveData[index] = new BoardPosition(i, nextColumn);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
        }
        return isMoved;
    }

    bool CanSlide(InputType type)
    {
        var board = Board.Clone() as int[,];

        _moveData = new();

        bool isMoved = false;
        bool[,] marged = new bool[board.GetLength(0), board.GetLength(1)];
        switch (type)
        {
            case InputType.Up:
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int k = 0; k < board.GetLength(1); k++)
                    {
                        if (i == 0) { continue; }
                        if (board[i, k] == 0) { continue; }
                        int currentRow = i;
                        while (currentRow > 0)
                        {
                            int nextRow = currentRow - 1;

                            if (Board[nextRow, k] == 0)
                            {
                                board[nextRow, k] = board[currentRow, k];
                                board[currentRow, k] = 0;
                                currentRow--;
                                isMoved = true;
                                continue;
                            }
                            if (Board[nextRow, k] == board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                board[nextRow, k] *= 2;
                                board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
                                isMoved = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Down:
                for (int i = board.GetLength(0) - 1; 0 <= i; i--)
                {
                    for (int k = 0; k < board.GetLength(1); k++)
                    {
                        if (i == board.GetLength(0) - 1) { continue; }
                        if (Board[i, k] == 0) { continue; }
                        int currentRow = i;
                        while (currentRow < board.GetLength(0) - 1)
                        {
                            int nextRow = currentRow + 1;

                            if (board[nextRow, k] == 0)
                            {
                                board[nextRow, k] = board[currentRow, k];
                                board[currentRow, k] = 0;
                                currentRow++;
                                isMoved = true;
                                continue;
                            }
                            if (board[nextRow, k] == board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                board[nextRow, k] *= 2;
                                board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
                                isMoved = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Left:
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int k = 0; k < board.GetLength(1); k++)
                    {
                        if (k == 0) { continue; }
                        if (board[i, k] == 0) { continue; }
                        int currentColumn = k;
                        while (currentColumn > 0)
                        {
                            int nextColumn = currentColumn - 1;

                            if (board[i, nextColumn] == 0)
                            {
                                board[i, nextColumn] = board[i, currentColumn];
                                board[i, currentColumn] = 0;
                                currentColumn--;
                                isMoved = true;
                                continue;
                            }
                            if (board[i, nextColumn] == board[i, currentColumn] && !marged[i, nextColumn] && !marged[i, currentColumn])
                            {
                                board[i, nextColumn] *= 2;
                                board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
                                isMoved = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
            case InputType.Right:
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int k = board.GetLength(1) - 1; 0 <= k; k--)
                    {
                        if (k == board.GetLength(1) - 1) { continue; }
                        if (board[i, k] == 0) { continue; }
                        int currentColumn = k;
                        while (currentColumn < board.GetLength(1) - 1)
                        {
                            int nextColumn = currentColumn + 1;

                            if (board[i, nextColumn] == 0)
                            {
                                board[i, nextColumn] = board[i, currentColumn];
                                board[i, currentColumn] = 0;
                                currentColumn++;
                                isMoved = true;
                                continue;
                            }
                            if (board[i, nextColumn] == board[i, currentColumn] && !marged[i, nextColumn] && !marged[i, currentColumn])
                            {
                                board[i, nextColumn] *= 2;
                                board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
                                isMoved = true; break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;
        }
        return isMoved;
    }

    void OverCellReset()
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(1); k++)
            {
                var n = Board[i, k];
                if (n > 2048)
                {
                    Board[i, k] = 0;
                }
            }
        }
    }

    bool IsGameOver()
    {
        foreach (var type in _inputTypes)
        {
            if (CanSlide(type))
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator UpdateCells()
    {
        _isUpdate = true;
        Debug.Log("Start");

        if (_moveData != null)
        {
            yield return StartCoroutine((_boardView.CellMoveAnimation(_moveData, OriginalBoard)));
            _moveData = null;
        }

        OverCellReset();

        var emptyPositions = GetEmptyBoardPosition();

        if (emptyPositions.Length > 0)
        {
            var value = Random.Range(0, 10) < _probability ? 2 : 4;
            var randomPos = emptyPositions[Random.Range(0, emptyPositions.Length)];
            SpawnCell(randomPos, value);
        }

        _boardView.SetBoard(Board);
        _boardView.SetScore(Score);
        DumpBoard();
        if (IsGameOver())
        {
            Debug.Log("GameOver");
            StartCoroutine(StartGameOver());
        }
        _isUpdate = false;
    }
    IEnumerator StartGameOver()
    {
        RankingSystem.ScoreList.Add(Score);
        RankingSystem.RankingSave();
        yield return new WaitForEndOfFrame();
        SceneChanger.SceneChange((int)SceneType.Title);
    }
    private void OnDisable()
    {
        _input.FindAction("Up").started -= Up;
        _input.FindAction("Down").started -= Down;
        _input.FindAction("Left").started -= Left;
        _input.FindAction("Right").started -= Right;
    }
}

enum InputType
{
    Up,
    Down,
    Left,
    Right
}
