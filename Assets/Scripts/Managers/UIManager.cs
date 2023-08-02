using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField]
    private List<BaseUIElement> menuPrefabs;

    [SerializeField]
    private LoadingScreen loadingScreen;
    public LoadingScreen LoadingScreen { get; private set; }

    private List<BaseUIElement> _menus = new List<BaseUIElement>();

    private void Awake()
    {
        InitMenus();
        InitLoadingScreen();
    }

    private void InitMenus()
    {
        CreateMenus();
        HideAllMenus();
    }

    private void InitLoadingScreen()
    {
        LoadingScreen = Instantiate(loadingScreen, MainCanvas.Instance.Canvas.transform);
        LoadingScreen.Hide();
    }

    private void CreateMenus()
    {
        foreach (var menu in menuPrefabs)
        {
            _menus.Add(Instantiate(menu, MainCanvas.Instance.MenusParent.transform));
        }
    }

    private void HideAllMenus()
    {
        foreach (var menu in _menus)
        {
            menu.Hide();
        }
    }

    public bool TryGetMenuByType<T>(out T menu) where T : BaseUIElement
    {
        menu = _menus.FirstOrDefault(x => x.GetType().Equals(typeof(T))) as T;
        if (menu)
        {
            return true;
        }
        return false;
    }
}
