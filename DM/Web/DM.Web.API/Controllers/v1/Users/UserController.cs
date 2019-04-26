using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    /// <inheritdoc />
    [Route("v1/users")]
    public class UserController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        [HttpGet("{login}", Name = nameof(GetUser))]
        public void GetUser(string login) => throw new NotImplementedException();
    }
}