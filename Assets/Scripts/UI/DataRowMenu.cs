using System;
using System.Collections.Generic;
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

    private List<DataItem> _itemData;
    private DataRowPoolingController dataRowPool;

    private void Awake()
    {
        dataRowPool = GetComponent<DataRowPoolingController>();

        previousButton.onClick.AddListener(OnPreviousButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        dataRowPool.Init();
    }
    //todo read and display data
    private void Start()
    {
        _itemData = testGetData();
        PopulateView(_itemData);
    }

    private List<DataItem> testGetData()
    {
        Array values = Enum.GetValues(typeof(DataItem.CategoryType));
        System.Random random = new System.Random();
        List<DataItem> items = new List<DataItem>();
        for (int i = 0; i < 5; i++)
        {
            items.Add(new DataItem(
                    (DataItem.CategoryType)values.GetValue(random.Next(values.Length)),
                    random.NextDouble().ToString(),
                    i % 2 == 0
                ));
        }
        return items;
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

    private void OnPreviousButtonClick()
    {
        ClaerView();
        _itemData = testGetData();
        PopulateView(_itemData);
    }

    private void OnNextButtonClick()
    {
        ClaerView();
        _itemData = testGetData();
        PopulateView(_itemData);
    }
}
