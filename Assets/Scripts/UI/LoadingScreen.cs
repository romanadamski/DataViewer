using DG.Tweening;
using System;
using UnityEngine;

public class LoadingScreen : BaseUIElement
{
    [SerializeField]
    private RectTransform loadingCircle;
    [Range(0, 1)]
    [SerializeField]
    private float rotateDuration;

    public override void Show()
    {
        base.Show();
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

    public override void Hide()
    {
        base.Hide();
        loadingCircle.DOKill();
    }
}
