using DG.Tweening;
using UnityEngine;

public class SelectButton : ButtonBase
{
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _enterScale = 1.2f;
    [SerializeField] float _exitScale = 1f;

    void Start()
    {
        OnMouseEnter += () => ScaleAnimation(_enterScale);
        OnMouseExit += () => ScaleAnimation(_exitScale);
    }

    void ScaleAnimation(float scale)
    {
        transform.DOScale(Vector3.one * scale, _duration).SetLink(gameObject);
    }

    private void OnDisable()
    {
        OnMouseEnter -= () => ScaleAnimation(_enterScale);
        OnMouseExit -= () => ScaleAnimation(_exitScale);
    }
}
