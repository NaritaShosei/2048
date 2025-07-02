using System;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _text;
    [SerializeField] Color _hideColor = Color.gray;
    [SerializeField] Color[] _colors = new[] { Color.red, Color.white, Color.cyan, Color.gray };

    public void Set(int index,int num)
    {
        _text.enabled = true;
        if (index == -1) { Debug.LogError($"予期しない数字{index}です。"); return; }
        if (index >= _colors.Length) { Debug.LogError($"colorsの要素数が足りません"); }
        _text.text = $"{num}";
        _image.color = _colors[index];
    }
    public void Hide()
    {
        _text.enabled = false;
        _image.color = _hideColor;
    }
}
