using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField]
    private List<BaseUIElement> uiElementPrefabs;

    [SerializeField]
    private LoadingScreen loadingScreenPrefab;
    public LoadingScreen LoadingScreen { get; private set; }

    private readonly List<BaseUIElement> _uiElements = new();

#pragma warning disable 0649
    [Inject]
    private readonly DiContainer _diContainer;
#pragma warning restore 0649

    private void Awake()
    {
        InitUIElements();
        InitLoadingScreen();
    }

    private void InitUIElements()
    {
        CreateUIElements();
        HideAllUIElements();
    }

    private void InitLoadingScreen()
    {
        LoadingScreen = _diContainer.InstantiatePrefab(loadingScreenPrefab, MainCanvas.Instance.Canvas.transform).GetComponent<LoadingScreen>();
        LoadingScreen.Hide();
    }

    private void CreateUIElements()
    {
        foreach (var uiElement in uiElementPrefabs)
        {
            _uiElements.Add(_diContainer.InstantiatePrefab(uiElement, MainCanvas.Instance.MenusParent.transform).GetComponent<BaseUIElement>());
        }
    }

    private void HideAllUIElements()
    {
        foreach (var uiElement in _uiElements)
        {
            uiElement.Hide();
        }
    }

    /// <summary>
    /// Try to get UI Element of given type.
    /// </summary>
    /// <typeparam name="T">BaseUIElement type.</typeparam>
    /// <returns>True if found UIElement of given type.</returns>
    public bool TryGetUIElementByType<T>(out T uiElement) where T : BaseUIElement
    {
        uiElement = _uiElements.FirstOrDefault(x => x.GetType().Equals(typeof(T))) as T;
        
        if (uiElement) return true;
        
        return false;
    }
}
