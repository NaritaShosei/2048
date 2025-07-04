﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameSystem : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] BoardView _boardView;
    public int[,] Board { get; private set; }
    public int Score { get; private set; }
    bool _isUpdate;
    [SerializeField] int _probability = 5;
    [SerializeField] float _waitTime = 0.5f;
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

        yield return new WaitForSeconds(1);

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
        BoardSlide(InputType.Up);
        StartUpdate();
    }
    void Down(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        BoardSlide(InputType.Down);
        StartUpdate();
    }
    void Left(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        BoardSlide(InputType.Left);
        StartUpdate();
    }
    void Right(InputAction.CallbackContext context)
    {
        if (_isUpdate) { return; }
        BoardSlide(InputType.Right);
        StartUpdate();
    }
    [ContextMenu("Update")]
    void StartUpdate()
    {
        StartCoroutine(nameof(UpdateCells));
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
        bool[,] marged = new bool[Board.GetLength(0), Board.GetLength(1)];
        switch (type)
        {
            case InputType.Up:
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int k = 0; k < Board.GetLength(1); k++)
                    {
                        if (i == 0) { continue; }
                        int currentRow = i;
                        while (currentRow > 0)
                        {
                            int nextRow = currentRow - 1;

                            if (Board[nextRow, k] == 0)
                            {
                                Board[nextRow, k] = Board[currentRow, k];
                                Board[currentRow, k] = 0;
                                currentRow--;
                                continue;
                            }
                            if (Board[nextRow, k] == Board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                Board[nextRow, k] *= 2;
                                Score += Board[nextRow, k];
                                Board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
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
                        int currentRow = i;
                        while (currentRow < Board.GetLength(0) - 1)
                        {
                            int nextRow = currentRow + 1;

                            if (Board[nextRow, k] == 0)
                            {
                                Board[nextRow, k] = Board[currentRow, k];
                                Board[currentRow, k] = 0;
                                currentRow++;
                                continue;
                            }
                            if (Board[nextRow, k] == Board[currentRow, k] && !marged[nextRow, k] && !marged[currentRow, k])
                            {
                                Board[nextRow, k] *= 2;
                                Score += Board[nextRow, k];
                                Board[currentRow, k] = 0;
                                marged[nextRow, k] = true;
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
                        int currentColumn = k;
                        while (currentColumn > 0)
                        {
                            int nextColumn = currentColumn - 1;

                            if (Board[i, nextColumn] == 0)
                            {
                                Board[i, nextColumn] = Board[i, currentColumn];
                                Board[i, currentColumn] = 0;
                                currentColumn--;
                                continue;
                            }
                            if (Board[i, nextColumn] == Board[i, currentColumn] && !marged[nextColumn, k] && !marged[currentColumn, k])
                            {
                                Board[i, nextColumn] *= 2;
                                Score += Board[i, nextColumn];
                                Board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
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
                        int currentColumn = k;
                        while (currentColumn < Board.GetLength(1) - 1)
                        {
                            int nextColumn = currentColumn + 1;

                            if (Board[i, nextColumn] == 0)
                            {
                                Board[i, nextColumn] = Board[i, currentColumn];
                                Board[i, currentColumn] = 0;
                                currentColumn++;
                                continue;
                            }
                            if (Board[i, nextColumn] == Board[i, currentColumn] && !marged[nextColumn, k] && !marged[currentColumn, k])
                            {
                                Board[i, nextColumn] *= 2;
                                Score += Board[i, nextColumn];
                                Board[i, currentColumn] = 0;
                                marged[i, nextColumn] = true;
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
        return true;
    }

    IEnumerator UpdateCells()
    {
        _isUpdate = true;
        Debug.Log("Start");
        var emptyPositions = GetEmptyBoardPosition();
        if (emptyPositions.Length > 0)
        {
            var value = Random.Range(0, 10) < _probability ? 2 : 4;
            var randomPos = emptyPositions[Random.Range(0, emptyPositions.Length)];
            SpawnCell(randomPos, value);
        }
        _boardView.SetBoard(Board);
        DumpBoard();
        yield return new WaitForSeconds(_waitTime);
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
