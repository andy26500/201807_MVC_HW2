using System;
using System.Linq;
using System.Collections.Generic;
	
namespace _201807_MVC_HW1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
		public override IQueryable<客戶資料> All()
		{
			return base.All().Where(x => x.是否已刪除 == false);
		}

	    public override void Delete(客戶資料 entity)
	    {
	        entity.是否已刪除 = true;
	    }

	    public 客戶資料 Find(int id)
	    {
	        return All().FirstOrDefault(x => x.Id == id);
	    }

	    public IQueryable<客戶資料> Search(string filterName, int filterCategoryId)
	    {
	        var result = All();

	        if (!string.IsNullOrEmpty(filterName))
	            result = result.Where(x => x.客戶名稱.Contains(filterName));

	        if (filterCategoryId != 0)
	            result = result.Where(x => x.客戶分類 == filterCategoryId);

	        return result;
	    }
	}

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}