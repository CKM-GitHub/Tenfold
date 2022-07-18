using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seruichi.Tenfold.Web.Controllers.Common
{
    public class CustomException :Exception
    {
        public CustomException() : base() { }

        public CustomException(string message) : base(message) { }
    }
}