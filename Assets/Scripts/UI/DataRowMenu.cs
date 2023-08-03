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

    private DataRowPoolingController _dataRowPool;
    private int _currentPageIndex;
    private int _pagesCount;
    private int _dataCount;

    private bool IsPrevButtonInteractable => _currentPageIndex > 0;
    private bool IsNextButtonInteractable => _currentPageIndex < _pagesCount - 1;

    [Inject]
    private IDataServer _dataServer;
    private CancellationTokenSource _tokenSource;

    private void Awake()
    {
        _dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        _dataRowPool.Init(rowsPerPage, rowsPerPage);

        _currentPageIndex = 0;
    }

    public override void Show()
    {
        base.Show();
        LoadView();
    }

    private async void LoadView()
    {
        UIManager.Instance.LoadingScreen.Show();
        _dataCount = await GetDataCount();
        _pagesCount = Mathf.CeilToInt((float)_dataCount / rowsPerPage);

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
        }

        return dataAvailableTask.Result;
    }

    private async Task GetData()
    {
        _tokenSource = new CancellationTokenSource();
        var requestedRows = Math.Min(rowsPerPage, _dataCount - _currentPageIndex * rowsPerPage);
        var requestedIndex = _currentPageIndex * rowsPerPage;
        var requestDataTask = _dataServer.RequestData(requestedIndex, requestedRows, _tokenSource.Token);

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
                ClearView();
                PopulateView(requestDataTask.Result);

                RefreshNavigationButtons();
                UIManager.Instance.LoadingScreen.Hide();
            }

            _tokenSource.Dispose();
        }
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
        UIManager.Instance.LoadingScreen.Show();
        _currentPageIndex--;
        await GetData();
    }

    private async void OnNextButtonClick()
    {
        UIManager.Instance.LoadingScreen.Show();
        _currentPageIndex++;
        await GetData();
    }

    private void RefreshNavigationButtons()
    {
        RefreshNavigationButton(previousButton, IsPrevButtonInteractable);
        RefreshNavigationButton(nextButton, IsNextButtonInteractable);
    }

    private void RefreshNavigationButton(Button button, bool isInteractable)
    {
        button.interactable = isInteractable;
        button.image.sprite = isInteractable ? activeButton : inactiveButton;
    }
}
