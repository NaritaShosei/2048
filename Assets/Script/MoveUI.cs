using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    [SerializeField] float _duration = 1;
    float _timer;
    [SerializeField] Vector3 _position = Vector3.zero;

    public async UniTask StartMove()
    {
        _timer = 0;
        Vector3 start = ((RectTransform)transform).anchoredPosition;
        while (_timer <= _duration)
        {
            _timer += Time.deltaTime;

            float t = Mathf.Clamp01(_timer / _duration);
            ((RectTransform)transform).anchoredPosition = Vector3.Lerp(start, _position, t);
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
        }
    }
}
