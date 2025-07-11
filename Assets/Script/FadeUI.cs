using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    float _timer;
    [SerializeField] float _duration = 1;
    [SerializeField] Image _image;
    public IEnumerator StartFade(int start, int end)
    {
        _image.raycastTarget = true;
        _timer = 0;
        Color color = _image.color;
        while (_timer <= _duration)
        {
            _timer += Time.deltaTime;
            float t = Mathf.Clamp01(_timer / _duration);
            color.a = Mathf.Lerp(start, end, t);
            _image.color = color;
            yield return new WaitForEndOfFrame();
        }
        _image.raycastTarget = false;
    }
}
