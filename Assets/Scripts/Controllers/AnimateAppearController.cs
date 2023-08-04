using DG.Tweening;
using UnityEngine;

public class AnimateAppearController : MonoBehaviour
{
    private RectTransform _rectTransform;
    private const float _enableDuration = 0.3f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        AniamateOnEnable();
    }

    private void AniamateOnEnable()
    {
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(1, _enableDuration);
    }
}
