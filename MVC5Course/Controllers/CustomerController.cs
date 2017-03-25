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
using ClosedXML.Excel;
using System.IO;
using System.Reflection;

namespace MVC5Course.Controllers
{
   // [Authorize]
    public class CustomerController : Controller
    {
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        客戶分類Repository repo客戶分類 = RepositoryHelper.Get客戶分類Repository();
        客戶聯絡人Repository repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository();
        // GET: 客戶資料
        public ActionResult Index(string sortBy, string 客戶分類, int pageNo = 1)
        {
            var data = repo.All().AsQueryable();

            if (!String.IsNullOrEmpty(客戶分類))
            {
                data = data.Where(p => p.客戶分類==客戶分類);
            }

            if (sortBy == "+統一編號")
            {
                data = data.OrderBy(p => p.統一編號);
            }
            else
            {
                data = data.OrderByDescending(p => p.統一編號);
            }


            
            ViewBag.客戶分類 = 客戶分類;
            ViewBag.sortBy = sortBy;
            return View(data.ToPagedList(pageNo, 10));
        }
        [HttpPost]
        public ActionResult Index(客戶聯絡人[] 客戶聯絡人data, string sortBy, string 客戶分類, int pageNo = 1)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in 客戶聯絡人data)
                {
                    var db = repo客戶聯絡人.Find(item.Id);
                    db.職稱 = item.職稱;
                    db.手機 = item.手機;
                    db.電話 = item.電話;
                 
                }
                repo客戶聯絡人.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            var data = repo.All().AsQueryable();

            if (!String.IsNullOrEmpty(客戶分類))
            {
                data = data.Where(p => p.客戶分類 == 客戶分類);
            }

            if (sortBy == "+統一編號")
            {
                data = data.OrderBy(p => p.統一編號);
            }
            else
            {
                data = data.OrderByDescending(p => p.統一編號);
            }



            ViewBag.客戶分類 = 客戶分類;
            ViewBag.sortBy = sortBy;
            return View(data.ToPagedList(pageNo, 10));
        }
        public JsonResult 客戶分類()
        {
            var 客戶分類 = repo客戶分類.All();

            return Json(客戶分類.ToList(), JsonRequestBehavior.AllowGet);
        }
        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            List<SelectListItem> 客戶分類 = new List<SelectListItem>();

            客戶分類.Add(new SelectListItem()
            {
                Text = "零售",
                Value = "零售",
                Selected = true
            });

            客戶分類.Add(new SelectListItem()
            {
                Text = "製造",
                Value = "製造",
                Selected = false
            });

            客戶分類.Add(new SelectListItem()
            {
                Text = "3C",
                Value = "3C",
                Selected = false
            });



            ViewBag.客戶分類 = 客戶分類;
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include =  "客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> 客戶分類 = new List<SelectListItem>();

            客戶分類.Add(new SelectListItem()
            {
                Text = "零售",
                Value = "零售",
                Selected = true
            });

            客戶分類.Add(new SelectListItem()
            {
                Text = "製造",
                Value = "製造",
                Selected = false
            });

            客戶分類.Add(new SelectListItem()
            {
                Text = "3C",
                Value = "3C",
                Selected = false
            });



            ViewBag.客戶分類 = 客戶分類;
            return View(客戶資料);
        }
       
        [HttpPost]
        public FileResult Export()
        {
            var data = repo.All();
            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = ConvertObjectsToDataTable(data);
                wb.Worksheets.Add(dt, "customer");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
                }
            }
        }
        public static DataTable ConvertObjectsToDataTable(IEnumerable<object> objects)
        {
            DataTable dt = null;

            if (objects != null && objects.Count() > 0)
            {
                Type type = objects.First().GetType();
                dt = new DataTable(type.Name);

                foreach (PropertyInfo property in type.GetProperties())
                {
                    dt.Columns.Add(new DataColumn(property.Name));
                }

                foreach (FieldInfo field in type.GetFields())
                {
                    dt.Columns.Add(new DataColumn(field.Name));
                }

                foreach (object obj in objects)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(column.ColumnName);
                        if (propertyInfo != null)
                        {
                            dr[column.ColumnName] = propertyInfo.GetValue(obj, null);
                        }

                        FieldInfo fieldInfo = type.GetField(column.ColumnName);
                        if (fieldInfo != null)
                        {
                            dr[column.ColumnName] = fieldInfo.GetValue(obj);
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        // POST: 客戶資料s/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")]  客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = repo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult 客戶聯絡人()
        {
            return View();
        }
    }
}
