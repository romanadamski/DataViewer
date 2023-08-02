using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DataRowPoolingController))]
public class DataRowMenu : BaseMenu
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

    private IList<DataItem> _itemData;
    private DataRowPoolingController dataRowPool;
    private IDataServer _dataServer;
    private System.Threading.CancellationTokenSource _tokenSource;
    private int _currentPageIndex;
    private int _pagesCount;
    private bool _isPrevButtonInteractable => _currentPageIndex > 0;
    private bool _isNextButtonInteractable => _currentPageIndex < _pagesCount - 1;

    private void Awake()
    {
        dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        dataRowPool.Init();

        _dataServer = new DataServerMock();
        _tokenSource = new System.Threading.CancellationTokenSource();
        _currentPageIndex = 0;
    }

    public override void Show()
    {
        base.Show();
        test();
    }

    private async Task test()
    {
        await test1();
        RefreshNavigationButtons();
        await test2();
    }

    private async Task test1()
    {
        _tokenSource = new System.Threading.CancellationTokenSource();
        var t = _dataServer.DataAvailable(_tokenSource.Token);
        try
        {
            await t;
        }
        catch (OperationCanceledException ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            if (!_tokenSource.Token.IsCancellationRequested)
            {
                _pagesCount = Mathf.CeilToInt(t.Result / 5f);
                Debug.Log(t.Result);
            }
            _tokenSource.Dispose();
            _tokenSource = new System.Threading.CancellationTokenSource();
        }
    }

    private async Task test2()
    {
        _tokenSource = new System.Threading.CancellationTokenSource();
        Debug.Log($"_currentPage {_currentPageIndex}");
        var t = _dataServer.RequestData(_currentPageIndex, 5, _tokenSource.Token);
        try
        {
            await t;
        }
        catch (OperationCanceledException ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            if (!_tokenSource.Token.IsCancellationRequested)
            {
                _itemData = t.Result;
                PopulateView(_itemData);

                //RefreshNavigationButtons();
            }

            _tokenSource.Dispose();
            _tokenSource = new System.Threading.CancellationTokenSource();
        }
    }

    private void PopulateView(IList<DataItem> dataItems)
    {
        for (int i = 0; i < dataItems.Count;i++)
        {
            PopulateView(dataItems[i], i + 1);
        }
    }

    private void PopulateView(DataItem dataItem, int number)
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
        _tokenSource.Cancel();
        ClaerView();
        _currentPageIndex--;
        RefreshNavigationButtons();
        await test2();
    }

    private async void OnNextButtonClick()
    {
        _tokenSource.Cancel();
        ClaerView();
        _currentPageIndex++;
        RefreshNavigationButtons();
        await test2();
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
