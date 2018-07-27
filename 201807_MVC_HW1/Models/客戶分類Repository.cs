using System;
using System.Linq;
using System.Collections.Generic;
	
namespace _201807_MVC_HW1.Models
{   
	public  class 客戶分類Repository : EFRepository<客戶分類>, I客戶分類Repository
	{
	    public override IQueryable<客戶分類> All()
	    {
	        return base.All().Where(x => x.是否已刪除 == false);
	    }

	    public override void Delete(客戶分類 entity)
	    {
	        entity.是否已刪除 = true;
	    }

	    public 客戶分類 Find(int id)
	    {
	        return All().FirstOrDefault(x => x.Id == id);
	    }
	}

	public  interface I客戶分類Repository : IRepository<客戶分類>
	{

	}
}