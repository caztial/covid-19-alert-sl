using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Data.Model
{
    public class PageData
    {
        private string PageName { get; set; }
        private string FontAwesomeIcon { get; set; }

        private bool IsActive { get; set; }

        private string Url { get;}

        public PageData(string pageName, string icon,bool isActive,string url)
        {
            this.PageName = pageName;
            this.FontAwesomeIcon = icon;
            this.IsActive = isActive;
            this.Url = url;
        }

        public string GetPageName()
        {
            return this.PageName;
        }

        public string GetPageIcon()
        {
            return this.FontAwesomeIcon;
        }
        public string GetActiveClass()
        {
            return (this.IsActive) ? "active" : "";
        }
        
        public void SetActiveStatus(bool status)
        {
            this.IsActive = status;
        }

        public string GetUrl()
        {
            return this.Url;
        }

        public bool GetIsActive()
        {
            return this.IsActive;
        }
    }
}
