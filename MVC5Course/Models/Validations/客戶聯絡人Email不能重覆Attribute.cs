using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.Validations
{
    public class 客戶聯絡人Email不能重覆Attribute : DataTypeAttribute
    {
        public 客戶聯絡人Email不能重覆Attribute() : base(DataType.Text)
        {
            this.ErrorMessage = "客戶連絡人Email不能重覆";
        }

        public override bool IsValid(object value)
        {
            /*
            客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
            客戶聯絡人 客戶聯絡人 = repo.FindEmail(value.ToString());

            if (客戶聯絡人!= null)
            {
                return false;
            }
            */
            return true;
        }

    }
}