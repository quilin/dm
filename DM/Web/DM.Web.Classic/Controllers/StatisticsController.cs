using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class StatisticsController : DmControllerBase
    {
        public ActionResult Index(Guid moduleId)
        {
            return new EmptyResult();
        }
    }
}