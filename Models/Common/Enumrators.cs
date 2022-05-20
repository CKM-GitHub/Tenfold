namespace Models
{
    public enum CounterKey: int
    {
        None = 0,
        MansionID = 7,
        SellerCD = 2,
    }

    public enum MailKBN : int
    {
        None = 0,
        MemberRegistration = 1,
        ResetPassword = 2,
        ContactTenfold = 3,
        ContactPerson = 4,
        ChangeMailAddress = 5,
        RealEstate_ContactTenfold=3,
        RealEstate_ContactPerson=4,
    }

    public enum RegexFormat : int
    {
        None = 0,
        Number,
        Alphabet,
        NumAlphaLowUp
    }

    public enum Regions : int
    {
        None = 0,
        /// <summary>
        /// 北海道
        /// </summary>
        Hokkaido = 1,
        /// <summary>
        /// 東北
        /// </summary>
        Tohoku = 2,
        /// <summary>
        /// 関東
        /// </summary>
        Kanto = 3,
        /// <summary>
        /// 東海
        /// </summary>
        Tokai = 4,
        /// <summary>
        /// 北陸
        /// </summary>
        Hokuriku = 5,
        /// <summary>
        /// 関西
        /// </summary>
        Kansai = 6,
        /// <summary>
        /// 中国
        /// </summary>
        Chugoku = 7,
        /// <summary>
        /// 四国
        /// </summary>
        Shikoku = 8,
        /// <summary>
        /// 九州
        /// </summary>
        Kyushu = 9,
        /// <summary>
        /// 沖縄
        /// </summary>
        Okinawa = 10
    }
}
