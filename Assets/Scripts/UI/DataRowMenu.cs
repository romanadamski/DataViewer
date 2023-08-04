using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(DataRowPoolingController))]
public class DataRowMenu : BaseUIElement
{
    [Header("Fields")]

    [SerializeField]
    private Button previousButton;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Transform dataParent;
    [SerializeField]
    private Sprite activeButton;
    [SerializeField]
    private Sprite inactiveButton;
    [Range(1,5)]
    [SerializeField]
    private int rowsPerPage;

    private DataRowPoolingController _dataRowPool;
    private int _currentPageIndex;
    private int _pagesCount;
    private int _dataCount;
    private IList<DataItem> _dataItems;

    private bool IsPreviousButtonInteractable => _currentPageIndex > 0;
    private bool IsNextButtonInteractable => _currentPageIndex < _pagesCount - 1;

    [Inject]
    private IDataServerWrapper _dataServerWrapper;

    private void Awake()
    {
        _dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        _dataRowPool.Init(rowsPerPage, rowsPerPage);

        _currentPageIndex = 0;
    }

    public override async void Show()
    {
        base.Show();

        UIManager.Instance.LoadingScreen.Show();
        _dataCount = await _dataServerWrapper.GetDataCount();

        if (_dataCount == 0) return;

        _pagesCount = Mathf.CeilToInt((float)_dataCount / rowsPerPage);

        await LoadData();

        RefreshView();
    }

    private async Task LoadData()
    {
        var requestedRows = Math.Min(rowsPerPage, _dataCount - _currentPageIndex * rowsPerPage);
        var requestedIndex = _currentPageIndex * rowsPerPage;
        _dataItems = await _dataServerWrapper.GetData(requestedRows, requestedIndex);
    }

    private void RefreshView()
    {
        ClearView();
        PopulateView(_dataItems);
        RefreshNavigationButtons();
        UIManager.Instance.LoadingScreen.Hide();
    }

    private void PopulateView(IList<DataItem> dataItems)
    {
        for (int i = 0; i < dataItems.Count;i++)
        {
            PopulateRow(dataItems[i], (i + 1) + _currentPageIndex * rowsPerPage);
        }
    }

    private void PopulateRow(DataItem dataItem, int number)
    {
        var dataRow = _dataRowPool.Get();
        if (!dataRow) return;

        dataRow.Init(dataItem, number);
        dataRow.transform.SetAsLastSibling();
    }

    private void ClearView()
    {
        _dataRowPool.ReleaseAll();
    }

    private async void OnPreviousButtonClick()
    {
        _currentPageIndex--;
        await LoadCurrentPage();
    }

    private async void OnNextButtonClick()
    {
        _currentPageIndex++;
        await LoadCurrentPage();
    }

    private async Task LoadCurrentPage()
    {
        UIManager.Instance.LoadingScreen.Show();

        await LoadData();

        RefreshView();
    }

    private void RefreshNavigationButtons()
    {
        RefreshNavigationButton(previousButton, IsPreviousButtonInteractable);
        RefreshNavigationButton(nextButton, IsNextButtonInteractable);
    }

    private void RefreshNavigationButton(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;
        button.image.sprite = isInteractable ? activeButton : inactiveButton;
    }
}
