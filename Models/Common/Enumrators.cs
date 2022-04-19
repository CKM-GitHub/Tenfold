namespace Models
{
    public enum CounterKey: int
    {
        None = 0,
        MansionID = 1,
        SellerCD = 2,
    }

    public enum MailKBN : int
    {
        None = 0,
        MemberRegistration = 1,
        ResetPassword = 2,
    }

    public enum RegexFormat : int
    {
        None = 0,
        Number,
        Alphabet,
        NumAlphaLowUp
    }
}
