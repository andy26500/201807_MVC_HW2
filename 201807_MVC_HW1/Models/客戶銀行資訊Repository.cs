using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace _201807_MVC_HW1.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(x => x.是否已刪除 == false);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }

        public 客戶銀行資訊 Find(int id)
        {
            return All().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<客戶銀行資訊> Search(string filterCustomerName)
        {
            var result = All();

            if (!string.IsNullOrEmpty(filterCustomerName))
                result = result
                    .Where(x => x.客戶資料.客戶名稱.Contains(filterCustomerName));

            return result;
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}