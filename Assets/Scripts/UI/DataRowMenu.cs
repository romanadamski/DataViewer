using System;
using System.Collections.Generic;
using System.Threading;
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

    private DataRowPoolingController dataRowPool;
    private int _currentPageIndex;
    private int _pagesCount;
    private int _dataCount;

    private bool _isPrevButtonInteractable => _currentPageIndex > 0;
    private bool _isNextButtonInteractable => _currentPageIndex < _pagesCount - 1;

    [Inject]
    private IDataServer _dataServer;
    private CancellationTokenSource _tokenSource;

    private void Awake()
    {
        dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        dataRowPool.Init(rowsPerPage, rowsPerPage);

        _currentPageIndex = 0;
        _tokenSource = new CancellationTokenSource();
    }

    public override void Show()
    {
        base.Show();
        LoadView();
    }

    private async void LoadView()
    {
        _dataCount = await GetDataCount();
        _pagesCount = Mathf.CeilToInt((float)_dataCount / rowsPerPage);
        Debug.Log(_dataCount);
        if (_pagesCount == 0) return;

        await GetData();
    }

    private async Task<int> GetDataCount()
    {
        _tokenSource = new CancellationTokenSource();
        var dataAvailableTask = _dataServer.DataAvailable(_tokenSource.Token);
        try
        {
            await dataAvailableTask;
        }
        catch (OperationCanceledException ex)
        {
            Debug.LogError(ex.Message);
            return 0;
        }
        finally
        {
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }

        return dataAvailableTask.Result;
    }

    private async Task GetData()
    {
        
        _tokenSource = new CancellationTokenSource();
        var requestedRows = Math.Min(rowsPerPage, _dataCount - _currentPageIndex * rowsPerPage);
        var requestedIndex = _currentPageIndex * rowsPerPage;
        var requestDataTask = _dataServer.RequestData(requestedIndex, requestedRows, _tokenSource.Token);
        Debug.Log($"_currentPage {_currentPageIndex} {requestedIndex} {requestedRows}");
        try
        {
            await requestDataTask;
        }
        catch (OperationCanceledException ex)
        {
            Debug.LogError(ex.Message);
        }
        finally
        {
            if (!_tokenSource.Token.IsCancellationRequested)
            {
                var _itemData = requestDataTask.Result;
                PopulateView(_itemData);

                RefreshNavigationButtons();
                UIManager.Instance.LoadingScreen.Hide();
            }

            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }
    }

    private void PopulateView(IList<DataItem> dataItems)
    {
        for (int i = 0; i < dataItems.Count;i++)
        {
            PopulateRow(dataItems[i], i + 1);
        }
    }

    private void PopulateRow(DataItem dataItem, int number)
    {
        var dataRow = dataRowPool.Get();
        if (!dataRow) return;

        dataRow.GetComponent<DataRow>().Init(dataItem, number);
    }

    private void ClaerView()
    {
        dataRowPool.ReleaseAll();
    }

    private async void OnPreviousButtonClick()
    {
        UIManager.Instance.LoadingScreen.Show();
        ClaerView();
        _currentPageIndex--;
        await GetData();
    }

    private async void OnNextButtonClick()
    {
        UIManager.Instance.LoadingScreen.Show();
        ClaerView();
        _currentPageIndex++;
        await GetData();
    }

    private void RefreshNavigationButtons()
    {
        RefreshNavigationButton(previousButton, _isPrevButtonInteractable);
        RefreshNavigationButton(nextButton, _isNextButtonInteractable);
    }

    private void RefreshNavigationButton(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;
        button.image.sprite = isInteractable ? activeButton : inactiveButton;
    }
}
