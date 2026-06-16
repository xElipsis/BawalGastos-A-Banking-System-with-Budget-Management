namespace wpfBudgetSys.Model
{
    public class Transaction
    {
        public int      TransactionId       { get; set; }
        public int      AccountId           { get; set; }
        public int?     CategoryId          { get; set; }
        public int?     RelatedAccountId    { get; set; }
        public string   Type                { get; set; } = string.Empty;
        public decimal  Amount              { get; set; }
        public string   Description         { get; set; } = string.Empty;
        public string   ReferenceNumber     { get; set; } = string.Empty;
        public DateTime Date                { get; set; }
        public string?  CategoryName        { get; set; }
    }
}
