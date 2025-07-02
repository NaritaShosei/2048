using System;
using UnityEngine;

public class CellView : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Color[] _colors = new[] { Color.red, Color.white, Color.cyan, Color.gray };
    readonly int[] _nums = new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

    public void ChangeColor(int num)
    {
        int index = Array.IndexOf(_nums, num);
        if (index == -1) { Debug.LogError($"予期しない数字{num}です。"); return; }
        if (index >= _colors.Length) { Debug.LogError($"colorsの要素数が足りません"); }
        _sr.color = _colors[index];
    }
}
