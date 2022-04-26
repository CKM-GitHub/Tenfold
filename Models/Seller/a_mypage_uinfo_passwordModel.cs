namespace Models
{
    public class a_mypage_uinfo_passwordModel : BaseModel
    {
        public string SellerCD { get; set; }
        public string SellerName { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
