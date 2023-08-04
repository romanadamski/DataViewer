using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


public class DataServerWrapper : IDataServerWrapper
{
#pragma warning disable 0649
    [Inject]
    private readonly IDataServer _dataServer;
#pragma warning restore 0649

    private CancellationTokenSource _tokenSource;

    public async Task<int> GetDataCount()
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

    public async Task<IList<DataItem>> GetData(int requestedRows, int requestedIndex)
    {
        _tokenSource = new CancellationTokenSource();
        var requestDataTask = _dataServer.RequestData(requestedIndex, requestedRows, _tokenSource.Token);

        try
        {
            await requestDataTask;
        }
        catch (OperationCanceledException ex)
        {
            Debug.LogError(ex.Message);
            return new List<DataItem>();
        }
        finally
        {
            _tokenSource.Dispose();
        }

        return requestDataTask.Result;
    }
}
