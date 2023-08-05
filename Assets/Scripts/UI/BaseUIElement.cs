using UnityEngine;

/// <summary>
/// Base class for UI such as menus, popus ect. providing Show and Hide metods.
/// </summary>
public class BaseUIElement : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
