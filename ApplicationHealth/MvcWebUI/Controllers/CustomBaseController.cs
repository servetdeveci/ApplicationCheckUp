using ApplicationHealth.Domain.DataTable.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApplicationHealth.MvcWebUI.Controllers
{
    public abstract class CustomBaseController : Controller
    {
        #region utils
        public BaseFilterParameters LoadDataTableSortParameters(BaseFilterParameters dataTableSendParameters)
        {
            SqlInjectionProtectDataTableSearchInput(dataTableSendParameters.mainFilter);
            string temp = "order[0][column]";
            temp = Request.Form[temp][0];
            temp = $"columns[{temp}][data]";
            dataTableSendParameters.sortColumnName = FirstCharToUpper(Request.Form[temp][0]);
            temp = "order[0][dir]";
            dataTableSendParameters.sortColumnDirection = Request.Form[temp][0];

            return dataTableSendParameters;
        }
        public string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
        private void SqlInjectionProtectDataTableSearchInput(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                param = "";
            }
            else
            {
                if (Regex.IsMatch(param, @"('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)", RegexOptions.IgnoreCase))
                {
                    param = "";
                }
            }
        }
        public string SetTime(DateTime dateTime)
        {
            var diff = DateTime.Now - dateTime;

            if (diff.TotalSeconds < 60)
            {
                return Math.Round(diff.TotalSeconds, 0).ToString() + " Sn";
            }
            else if (diff.TotalSeconds >= 60 && diff.TotalSeconds < 3600)
            {
                string second = (Convert.ToInt32(diff.Seconds) % 60).ToString();
                return Math.Round(diff.TotalMinutes, 0).ToString() + " Dk " + second + " Sn Önce";
            }
            else if (diff.TotalSeconds >= 3600 && diff.TotalSeconds < 86400)
            {
                string minute = (Convert.ToInt32(diff.Minutes) % 60).ToString();
                return Math.Round(diff.TotalHours, 0).ToString() + " Sa " + minute + " Dk";
            }
            else
            {
                return dateTime.ToString();
            }
        }
      
        #endregion
    }
}
