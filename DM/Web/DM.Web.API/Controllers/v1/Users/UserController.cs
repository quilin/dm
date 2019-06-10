using System;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    /// <inheritdoc />
    [Route("v1/users")]
    public class UserController : Controller
    {
        /// <summary>
        /// Get list of community users
        /// </summary>
        /// <response code="200"></response>
        [HttpGet("")]
        public void Get([FromQuery] UsersQuery query) => throw new NotImplementedException();
        
        /// <summary>
        /// Get certain user details
        /// </summary>
        /// <param name="login"></param>
        [HttpGet("{login}", Name = nameof(GetUser))]
        public void GetUser(string login) => throw new NotImplementedException();
    }
}