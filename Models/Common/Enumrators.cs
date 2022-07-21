namespace Models
{
    public enum CounterKey: int
    {
        None = 0,
        MansionID = 1,
        SellerCD = 2,
        MansionCD = 7,
        AssRegID =3,
    }

    public enum MailKBN : int
    {
        None = 0,
        MemberRegistration = 1,
        ResetPassword = 2,
        ContactTenfold = 3,
        ContactPerson = 4,
        ChangeMailAddress = 5,
        RealEstate_ContactTenfold=6,
        RealEstate_ContactPerson=7,
        Seller_MailSend =11,
    }

    public enum RegexFormat : int
    {
        None = 0,
        Number,
        Alphabet,
        NumAlphaLowUp
    }

}
