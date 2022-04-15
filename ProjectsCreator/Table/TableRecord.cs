namespace ProjectsCreator.Table
{
    public class TableRecord
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public float Quantity { get; set; }

        public TableRecord(string name, string code, float quantity)
        {
            Name = name;
            Code = code;
            Quantity = quantity;
        }
    }
}