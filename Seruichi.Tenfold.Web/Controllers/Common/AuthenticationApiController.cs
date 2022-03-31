using Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Seruichi.Seller.Web.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SignIn(UserModel model)
        {
            if (model != null && model.Id == "hoge" && model.Password == "hoge")
            {
                FormsAuthentication.SetAuthCookie(model.Id, model.RememberMe);

                return this.Request.CreateResponse(HttpStatusCode.OK, new
                {
                    IsSuccess = true,
                    Message = "ログインに成功しました。"
                });
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.Accepted, new
                {
                    IsSuccess = false,
                    Message = "ID または パスワード が違います。"
                });
            }
        }


        [HttpPost]
        public HttpResponseMessage SignOut()
        {
            FormsAuthentication.SignOut();
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
