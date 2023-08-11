namespace EnaxisSelenium.Foo.Contracts
{
    public interface ISortableTable
    {
        bool IsLinkPresentInHeader(int columnIndex);
        void SortByColumn(int columnIndex);
    }
}
