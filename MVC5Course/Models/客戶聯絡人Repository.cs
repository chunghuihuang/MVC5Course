using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public 客戶聯絡人 FindEmail(int ? CID,String email)
        {
            return this.All().FirstOrDefault(p => p.客戶Id == CID && p.Email == email);
        }

        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => false == p.刪除 );
        }

        public IQueryable<客戶聯絡人> All(bool showAll)
        {
            if (showAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public override void Delete(客戶聯絡人 entity)
        {
            this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            entity.刪除 = true;
        }

    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}