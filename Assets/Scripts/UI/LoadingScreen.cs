using DG.Tweening;
using System;
using UnityEngine;

/// <summary>
/// UI element which blocks input when active
/// </summary>
public class LoadingScreen : BaseUIElement
{
    [SerializeField]
    private RectTransform loadingCircle;

    [Tooltip("How fast loading circle make full turn (in secnods)")]
    [Range(0, 1)]
    [SerializeField]
    private float rotateDuration;

    public override void OnShow()
    {
        PlayLoadingAnimation();
    }

    private void PlayLoadingAnimation()
    {
        loadingCircle.localEulerAngles = Vector3.zero;
        loadingCircle
            .DORotate(new Vector3(0, 0, 360), rotateDuration)
            .SetRelative()
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public override void OnHide()
    {
        loadingCircle.DOKill();
    }
}
