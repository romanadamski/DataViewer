using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDataServerWrapper
{
    Task<int> GetDataCount();
    Task<IList<DataItem>> GetData(int startIndex, int requestedRows);
}
