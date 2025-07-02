using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameSystem : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] BoardView _boardView;
    public int[,] Board { get; private set; }
    public int Score { get; private set; }

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

    IEnumerator UpdateCells()
    {

        yield return null;
    }
}
enum InputType
{
    Up,
    Down,
    Left,
    Right
}
