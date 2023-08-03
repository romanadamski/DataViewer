public class DataRowPoolingController : BaseObjectPoolingController<DataRow>
{
    public void Init(int defaultCapacity, int maxSize)
    {
        base.Init(defaultCapacity: defaultCapacity, maxSize: maxSize);
    }
}
