using DG.Tweening;
using UnityEngine;

/// <summary>
/// Component that triggers an animation on an object when it is enabled.
/// </summary>
public class AnimateAppearController : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [Tooltip("Duration of the activation animation (in seconds).")]
    [Range(0, 1)]
    [SerializeField]
    private float _enableDuration = 0.3f;

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
