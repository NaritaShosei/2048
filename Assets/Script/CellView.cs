using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CellView : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _text;
    [SerializeField] Color _hideColor = Color.gray;
    [SerializeField] Color[] _colors = new[] { Color.red, Color.white, Color.cyan, Color.gray };
    static readonly int[] _nums = new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };//11
    [SerializeField] float _duration;

    public void Set(int num)
    {
        _text.enabled = true;
        int index = Array.IndexOf(_nums, num);
        if (index == -1) { Debug.LogError($"予期しない数字{num}です。"); return; }
        if (index >= _colors.Length) { Debug.LogError($"colorsの要素数が足りません"); return; }
        _text.text = $"{num}";
        _image.color = _colors[index];
    }
    public void Hide()
    {
        _text.enabled = false;
        _image.color = _hideColor;
    }
    public void MoveAnimation(Vector2 pos)
    {
        ((RectTransform)transform).DOAnchorPos(pos, _duration);
    }
}
