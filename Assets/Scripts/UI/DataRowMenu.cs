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
    [Range(1, 5)]
    [SerializeField]
    private int rowsPerPage;

    private DataRowPoolingController _dataRowPool;
    private int _currentPageIndex;
    private int _pagesCount;
    private int _dataCount;
    private IList<DataItem> _dataItems;

    private bool IsPreviousButtonInteractable => _currentPageIndex > 0;
    private bool IsNextButtonInteractable => _currentPageIndex < _pagesCount - 1;

#pragma warning disable 0649
    [Inject]
    private readonly IDataServerWrapper _dataServerWrapper;
#pragma warning restore 0649

    private void Awake()
    {
        _dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);

        _dataRowPool.Init(rowsPerPage, rowsPerPage);
    }

    public override async void Show()
    {
        base.Show();

        //Show loading screen to block input while loading data
        UIManager.Instance.LoadingScreen.Show();
        
        //First, get data count to calculate page count
        _dataCount = await _dataServerWrapper.GetDataCount();
        _pagesCount = Mathf.CeilToInt((float)_dataCount / rowsPerPage);

        //Set current page index to first one on menu show
        _currentPageIndex = 0;

        //Load data and refresh view
        await LoadData();
        RefreshView();

        //Restore input
        UIManager.Instance.LoadingScreen.Hide();
    }

    private async Task LoadData()
    {
        var requestedRows = Math.Min(rowsPerPage, _dataCount - _currentPageIndex * rowsPerPage);
        var startIndex = _currentPageIndex * rowsPerPage;
        _dataItems = await _dataServerWrapper.GetData(startIndex, requestedRows);
    }

    private void RefreshView()
    {
        ClearView();
        PopulateView(_dataItems);
        RefreshNavigationButtons();
    }

    private void PopulateView(IList<DataItem> dataItems)
    {
        for (int i = 0; i < dataItems.Count; i++)
        {
            var currentDataRowNumber = (i + 1) + _currentPageIndex * rowsPerPage;
            PopulateRow(dataItems[i], currentDataRowNumber);
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
        //show loading screen to block input while loading data
        UIManager.Instance.LoadingScreen.Show();

        await LoadData();

        RefreshView();

        //Restore input
        UIManager.Instance.LoadingScreen.Hide();
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
