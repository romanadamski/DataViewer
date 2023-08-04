using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IDataServerWrapper
{
    Task<int> GetDataCount();
    Task<IList<DataItem>> GetData(int requestedRows, int requestedIndex);
}
