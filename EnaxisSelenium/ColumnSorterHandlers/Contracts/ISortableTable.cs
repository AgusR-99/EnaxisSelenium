namespace EnaxisSelenium.ColumnSorterHandlers.Contracts
{
    public interface ISortableTable
    {
        bool IsLinkPresentInHeader(int columnIndex);
        void SortByColumn(int columnIndex);
    }
}
