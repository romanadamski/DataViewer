using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField]
    private List<BaseUIElement> menuPrefabs;

    [SerializeField]
    private LoadingScreen loadingScreenPrefab;
    public LoadingScreen LoadingScreen { get; private set; }


    private List<BaseUIElement> _menus = new List<BaseUIElement>();

    [Inject]
    private DiContainer _diContainer;

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
        LoadingScreen = _diContainer.InstantiatePrefab(loadingScreenPrefab, MainCanvas.Instance.Canvas.transform).GetComponent<LoadingScreen>();
        LoadingScreen.Hide();
    }

    private void CreateMenus()
    {
        foreach (var menu in menuPrefabs)
        {
            _menus.Add(_diContainer.InstantiatePrefab(menu, MainCanvas.Instance.MenusParent.transform).GetComponent<BaseUIElement>());
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
