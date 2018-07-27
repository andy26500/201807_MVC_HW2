using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace _201807_MVC_HW1.Models.Attributes
{
    public class CellPhoneValidationAttribute : ValidationAttribute
    {
        public CellPhoneValidationAttribute()
        {
            ErrorMessage = "手機格式錯誤";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var regex = new Regex("\\d{4}-\\d{6}");
            string cellPhone = value as string;
           
            return string.IsNullOrEmpty(cellPhone) || regex.IsMatch(cellPhone);
        }
    }
}