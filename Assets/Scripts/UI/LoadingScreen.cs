using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class LoadingScreen : BaseUIElement
{
    [SerializeField]
    RectTransform _loadingCircle;

    public override void Show()
    {
        base.Show();
        PlayLoadingAnimation();
    }

    private void PlayLoadingAnimation()
    {
        _loadingCircle
            .DORotate(new Vector3(0, 0, 360), 0.7f)
            .SetRelative()
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public override void Hide()
    {
        base.Hide();
        _loadingCircle.DOKill();
    }
}
