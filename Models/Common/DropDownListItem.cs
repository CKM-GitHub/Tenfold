namespace Models
{
    public class DropDownListItem
    {
        // Key
        public string Value { get; set; }

        public string DisplayText { get; set; } = "";

        public int DisplayOrder { get; set; } = 0;

        public string HiddenItem { get; set; }
    }
}
