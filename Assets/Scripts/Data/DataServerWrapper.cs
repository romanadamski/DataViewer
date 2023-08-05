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

    /// <summary>
    /// An async function that returns the amount of requested data.
    /// </summary>
    /// <returns>Requested data amount.</returns>
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

    /// <summary>
    /// An async function that returns requested data.
    /// </summary>
    /// <param name="startIndex">Starting index of the requested data.</param>
    /// <param name="requestedRows">Amount of requested data.</param>
    /// <returns>IList object of requested data.</returns>
    public async Task<IList<DataItem>> GetData(int startIndex, int requestedRows)
    {
        _tokenSource = new CancellationTokenSource();
        var requestDataTask = _dataServer.RequestData(startIndex, requestedRows, _tokenSource.Token);

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
