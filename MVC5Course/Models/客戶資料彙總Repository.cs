using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class 客戶資料彙總Repository : EFRepository<客戶資料彙總>, I客戶資料彙總Repository
	{

	}

	public  interface I客戶資料彙總Repository : IRepository<客戶資料彙總>
	{

	}
}