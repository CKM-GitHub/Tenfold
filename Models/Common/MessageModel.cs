namespace Models
{
    public class MessageModel
    {
        public string MessageID { get; set; }
        public string MessageText1 { get; set; }
        public string MessageText2 { get; set; }
        public string MessageText3 { get; set; }
        public string MessageText4 { get; set; }
        public int MessageMark { get; set; }
        public string MessageIcon { get; set; }
        public int MessageButton { get; set; }
        public int ButtonValues { get; set; }

        public MessageModel()
        {
        }

        public MessageModel(string messageText1)
        {
            MessageText1 = messageText1;
        }
    }
}
