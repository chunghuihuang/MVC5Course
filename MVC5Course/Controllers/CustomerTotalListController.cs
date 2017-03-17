using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using PagedList;

namespace MVC5Course.Controllers
{
   // [Authorize]
    public class CustomerTotalListController : Controller
    {
        客戶資料彙總Repository repo = RepositoryHelper.Get客戶資料彙總Repository();

        // GET: 客戶資料彙總
        public ActionResult Index(string sortBy, string keyword, int pageNo = 1)
        {
            var data = repo.All();

           
           

            return View(data.ToList());
        }


    }
}
