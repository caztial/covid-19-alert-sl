using Dashboard.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Data.Services
{
    public class AppData
    {
        public string PageTitle { get; set; }

        public string AppTitle { get; set; }


        public Dictionary<string, PageData> PageDataHash { get; }

        public AppData()
        {
            this.AppTitle = "Covid-19 LK";
            this.PageDataHash = new Dictionary<string, PageData>
            {
                { "LocalData", new PageData("Local Report", "fa-street-view",true,"local_data") },
                { "GlobalData", new PageData("Global Report", "fa-globe",false,"global_data") }
            };


        }


        public void SetActivePage(string pageId)
        {
            PageData pageRef = this.GetPageTitleRef(pageId);
            if (pageRef!=null)
            {
                this.PageTitle = this.GetPageTitleRef(pageId).GetPageName();
            }

            foreach(KeyValuePair<string,PageData> dataRef in this.PageDataHash)
            {
                if (dataRef.Key==pageId)
                {
                    dataRef.Value.SetActiveStatus(true);
                }
                else
                {
                    dataRef.Value.SetActiveStatus(false);
                }
            }
            
        }

        public PageData GetPageTitleRef(string pageId)
        {
            return this.PageDataHash.GetValueOrDefault(pageId);
        }

        public string GetTemplateTitle()
        {
            return ((this.PageTitle != null) ? this.PageTitle + " - " : "") + this.AppTitle;
        }

        public Dictionary<string,PageData> GetMenuData()
        {
            return this.PageDataHash;
        }

    }
}
