namespace ApiTransation.Models
{
    public class CartaoCredito
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public DateTime ExperitationDate { get; set; }
    }
}
