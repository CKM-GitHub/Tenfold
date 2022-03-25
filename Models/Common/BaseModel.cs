using System;

namespace Models
{
    [Serializable]
    public class BaseModel
    {
        public string Operator { get; set; }
        public string IPAddress { get; set; }
    }
}
