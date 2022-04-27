namespace Models
{
    public class a_mailregisterModel : BaseModel
    {
        public string MailAddress { get; set; }
        public string NewMailAddress { get; set; }
        public string Password { get; set; }
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
    }
}
