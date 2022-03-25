﻿using System;

namespace Models
{
    [Serializable]
    public class UserModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool RememberMe { get; set; }
    }
}
