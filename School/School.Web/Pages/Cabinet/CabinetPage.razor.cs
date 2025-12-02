using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;
using School.Web.PageModels.Cabinets;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Cabinet
{
    public class CabinetPageViewModel : ComponentBase
    {
        [Inject] 
        public CabinetService CabinetService { get; set; }
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected List<CabinetItemViewModel> Cabinets { get; set; } = new();
        protected EditCabinetModel EditModel { get; set; } = new(); 
        protected DeleteCabinetModel DeleteModel { get; set; } = new();
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Cabinets = CabinetService.GetCabinets();
                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SelectCabinet(CabinetItemViewModel cabinet)
        {
            EditModel = new EditCabinetModel
            {
                Model = (CabinetItemViewModel)cabinet.Clone(),
                Teachers = Teachers,
                IsOpenDialog = true
            };
            StateHasChanged();
        }

        protected void AddNewCabinet()
        {
            EditModel = new EditCabinetModel
            {
                Model = new CabinetItemViewModel(new Db.Models.CabinetModel()),
                Teachers = Teachers,
                IsOpenDialog = true
            };
            StateHasChanged();
        }

        protected void DeleteCabinet(CabinetItemViewModel cabinet)
        {
            if (cabinet != null)
            {
                DeleteModel = new();
                DeleteModel.CabinetDelete = cabinet;
                DeleteModel.IsOpenDialog = true;
                StateHasChanged();
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            if (confirmed && DeleteModel.CabinetDelete != null)
            {
                CabinetService.DeleteCabinet(DeleteModel.CabinetDelete);
                Cabinets = CabinetService.GetCabinets();
                StateHasChanged(); 
            }
            DeleteModel.IsOpenDialog = false; 
            DeleteModel.CabinetDelete = null; 
        }

        protected void SaveChanges(CabinetItemViewModel item)
        {
            if (item != null)
            {
                if (item.Id == 0)
                {
                    CabinetService.AddCabinet(item);  
                }
                else
                {
                    CabinetService.Update(item);    
                }
                Cabinets = CabinetService.GetCabinets();
                StateHasChanged();
            }
            EditModel.IsOpenDialog = false;
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
