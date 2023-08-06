using UnityEngine;

/// <summary>
/// Base class for UI such as menus, popus ect. providing Show and Hide metods.
/// </summary>
public class BaseUIElement : MonoBehaviour
{
    /// <summary>
    /// Called after show ui element.
    /// Can be overriden.
    /// </summary>
    public virtual void OnShow() { }

    /// <summary>
    /// Called after hide ui element.
    /// Can be overriden.
    /// </summary>
    public virtual void OnHide() { }

    public void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }
}
