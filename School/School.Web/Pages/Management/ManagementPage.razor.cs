using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;
using School.Web.PageModels.Managements;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Management
{
    public class ManagementPageViewModel : ComponentBase
    {
        [Inject] ManagementService ManagementService { get; set; }
        protected List<ManagementItemViewModel> Managements { get; set; } = new();
        protected EditManagementModel EditModel { get; set; } = new();
        protected DeleteManagementModel DeleteModel { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Managements = ManagementService.GetManagements();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        //protected void Update(ManagementItemViewModel management)
        //{
        //    management.MiddleName = management.;
        //    ManagementService.Update(management);
        //}

        protected void AddNewManagement()
        {
            EditModel = new();
            EditModel.Model = new ManagementItemViewModel(new Db.Models.ManagementModel());
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void SelectManagement(ManagementItemViewModel management)
        {
            EditModel = new();
            EditModel.Model = (ManagementItemViewModel)management.Clone();
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void SaveChanges(ManagementItemViewModel item)
        {
            if (item != null)
            {
                if (item.Id == 0)
                {
                    ManagementService.AddManagement(item);
                }
                else
                {
                    ManagementService.Update(item);     
                }
                Managements = ManagementService.GetManagements();
                StateHasChanged();
            }
            EditModel.IsOpenDialog = false;  
        }

        protected void DeleteManagement(ManagementItemViewModel management)
        {
            if (management != null)
            { 
                DeleteModel = new();
                DeleteModel.ManagementDelete = management;
                DeleteModel.IsOpenDialog = true;
                StateHasChanged();
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            if (confirmed && DeleteModel.ManagementDelete != null)
            {
                ManagementService.DeleteManagement(DeleteModel.ManagementDelete);
                Managements = ManagementService.GetManagements();
                StateHasChanged();
            }
            DeleteModel.IsOpenDialog = false;
            DeleteModel.ManagementDelete = null;
        }

        protected string GetChangeLog(List<ChangeLogJson> changeLogJsons)
        {
            if (changeLogJsons == null || !changeLogJsons.Any())
                return "Нет истории изменений";

            var lastChange = changeLogJsons.OrderByDescending(x => x.Date).First();
            return $"{lastChange.Date:dd.MM.yyyy HH:mm} - {lastChange.User}: {lastChange.Operation}";
        }
    }
}
