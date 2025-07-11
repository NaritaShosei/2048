using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    [SerializeField] float _duration = 1;
    float _timer;
    [SerializeField] Vector3 _position = Vector3.zero;

    public IEnumerator StartMove()
    {
        _timer = 0;
        Vector3 start = ((RectTransform)transform).anchoredPosition;
        while (_timer <= _duration)
        {
            _timer += Time.deltaTime;

            float t = Mathf.Clamp01(_timer / _duration);
            ((RectTransform)transform).anchoredPosition = Vector3.Lerp(start, _position, t);
            yield return new WaitForEndOfFrame();
        }
    }
}
