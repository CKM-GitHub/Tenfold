using System.Collections.Generic;

namespace Models
{
    public class a_loginModel : BaseModel
    {
        public string MailAddress { get; set; }

        public Dictionary<string, string> ValidationResult { get; set; } = new Dictionary<string, string>();
    }
}
