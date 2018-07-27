using System;
using System.Linq;
using System.Collections.Generic;

namespace _201807_MVC_HW1.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(x => x.是否已刪除 == false);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public 客戶聯絡人 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<客戶聯絡人> Search(string filterName, string filterTitle)
        {
            var result = All();

            if (!string.IsNullOrEmpty(filterName))
                result = result.Where(x => x.姓名.Contains(filterName));

            if (!string.IsNullOrEmpty(filterTitle))
                result = result.Where(x => x.職稱 == filterTitle);

            return result;
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}