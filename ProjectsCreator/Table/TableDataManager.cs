using System.Collections.Generic;
using Gtk;

namespace ProjectsCreator.Table
{
    public class TableDataManager
    {
        private readonly List<TableRecord> _records = new ();
        
        public TableDataManager()
        {
            _records.Add(new TableRecord("Данные 1", "КОД.123.001", 0.01f));
            _records.Add(new TableRecord("Данные 2", "КОД.123.002", 0.02f));
            _records.Add(new TableRecord("Данные 3", "КОД.123.003", 0.03f));
            _records.Add(new TableRecord("Данные 4", "КОД.123.004", 0.04f));
            _records.Add(new TableRecord("Данные 5", "КОД.123.005", 0.05f));
            _records.Add(new TableRecord("Данные 6", "КОД.123.006", 0.06f));
            _records.Add(new TableRecord("Данные 7", "КОД.123.007", 0.07f));
            _records.Add(new TableRecord("Данные 8", "КОД.123.008", 0.08f));
            _records.Add(new TableRecord("Данные 9", "КОД.123.009", 0.09f));
            _records.Add(new TableRecord("Данные 10","КОД.123.010", 0.00f));
            _records.Add(new TableRecord("Данные 11","КОД.123.011", 1.01f));
            _records.Add(new TableRecord("Данные 12","КОД.123.012", 2.01f));
            _records.Add(new TableRecord("Данные 13","КОД.123.013", 3.01f));
            _records.Add(new TableRecord("Данные 14","КОД.123.014", 4.01f));
            _records.Add(new TableRecord("Данные 15","КОД.123.015", 5.01f));
            _records.Add(new TableRecord("Данные 16","КОД.123.016", 6.01f));
            _records.Add(new TableRecord("Данные 17","КОД.123.017", 7.01f));
        }
        
        private ListStore TreeViewModel
        {
            get
            {
                var listStore = new ListStore(typeof(string), typeof(string), typeof(float));

                foreach (var record in _records)
                {
                    listStore.AppendValues(record.Name, record.Code, record.Quantity);
                }

                return listStore;
            }
        }

        public TreeView TreeView => new (TreeViewModel);
    }
}