using AQLI.Data.Models.ViewComponentModels;
using AQLI.DataServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQLI.UI.Views.Widget.ViewComponents
{
    public class DatabaseRecordInfoViewComponent : ViewComponent
    {
        private readonly DataFactory DataSource;

        public DatabaseRecordInfoViewComponent(DataFactory _dataSource)
        {
            DataSource = _dataSource;
        }

        public async Task<IViewComponentResult> InvokeAsync(dynamic Model, Type t)
        {
            DatabaseRecordInfoVCModel viewDataModel = new DatabaseRecordInfoVCModel();

            var props = t.GetProperties();            
                        
            //Determine primary key property name and value for Model 
            string keyPropertyName = props
                .Where(p => 
                    p.Name.EndsWith("Id") || 
                    p.Name.EndsWith("ID")
                    )
                .ToList()
                .Select(p => p.Name)
                .FirstOrDefault();

            if (keyPropertyName != null)
            {
                viewDataModel.PKID = t.GetProperty(keyPropertyName).GetValue(Model);

                //Added date 
                if (t.GetProperty("Added") != null)
                {
                    viewDataModel.Added = t.GetProperty("Added").GetValue(Model);
                }

                //Added by first name and last name
                var addedByProp = t.GetProperty("AddedBy");
                if (t.GetProperty("AddedBy") != null)
                {
                    var recordData = DataSource.List_Users().Where(u => u.UserId == t.GetProperty("AddedBy").GetValue(Model)).First();
                    viewDataModel.AddedBy = $@"{recordData.FirstName} {recordData.LastName}";
                }

                //Last Modified date
                var modifiedProp = t.GetProperty("Modified");
                if (modifiedProp.GetValue(Model) != null)
                {
                    viewDataModel.Modified = t.GetProperty("Modified").GetValue(Model);
                }

                //Modified by first name and last name
                var modifiedByProp = t.GetProperty("ModifiedBy");
                if (modifiedByProp.GetValue(Model) != null)
                {
                    var recordData = DataSource.List_Users().Where(u => u.UserId == t.GetProperty("ModifiedBy").GetValue(Model)).First();
                    viewDataModel.ModifiedBy = $@"{recordData.FirstName} {recordData.LastName}";
                }
            }
                                    
            return View(viewDataModel);
        }

    }
}
