using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDataServerWrapper
{
    Task<int> GetDataCount();
    Task<IList<DataItem>> GetData(int requestedRows, int requestedIndex);
}
