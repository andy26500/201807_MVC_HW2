using System;
using System.Linq;
using System.Collections.Generic;
	
namespace _201807_MVC_HW1.Models
{   
	public  class 客戶統計資訊Repository : EFRepository<客戶統計資訊>, I客戶統計資訊Repository
	{
	    
	}

	public  interface I客戶統計資訊Repository : IRepository<客戶統計資訊>
	{

	}
}