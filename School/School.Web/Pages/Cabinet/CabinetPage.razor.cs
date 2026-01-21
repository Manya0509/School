using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using School.Db.Views;
using School.Web.Data.Services;
using School.Web.PageModels;
using School.Web.PageModels.Cabinets;
using School.Web.PageModels.Students;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Cabinet
{
    public class CabinetPageViewModel : BaseViewModel
    {
        [Inject]
        public CabinetService CabinetService { get; set; }
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected List<CabinetItemViewModel> Cabinets { get; set; } = new();
        protected EditCabinetModel EditModel { get; set; } = new();
        protected DeleteCabinetModel DeleteModel { get; set; } = new();
        protected FilterCabinetModel FilterCabinet { get; set; }
        protected bool ShowFilters { get; set; } = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    IsShowSpiner = true;
                    await InvokeAsync(StateHasChanged);
                    await Task.Delay(1);
                    FilterCabinet = new FilterCabinetModel();
                    FilterCabinet.Teachers = TeacherService.GetFilterModels();

                    Cabinets = CabinetService.GetCabinets();

                    Toaster.Add("TEXT.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
                }

                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка CabinetPage /OnAfterRenderAsync. {e?.Message} {e?.StackTrace}");
                    ShowErrorDialog($"Ошибка: {e.Message}");
                }
                finally
                {
                    IsShowSpiner = false;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public void InitFilter()
        {
            try
            {
                FilterCabinet = new();
                FilterCabinet.Teachers = TeacherService.GetFilterModels();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /InitFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void ToggleFilters()
        { 
            ShowFilters = !ShowFilters;
            StateHasChanged();
        }

        public void Search()
        {
            Cabinets = CabinetService.GetCabinetsFilters(
             FilterCabinet.Number,
             FilterCabinet.TeacherId,
             FilterCabinet.TeacherName
             );
            StateHasChanged();
        }

        public void ResetFilter()
        {
            try
            {
                var teachersList = FilterCabinet.Teachers;

                FilterCabinet = new FilterCabinetModel
                {
                    Teachers = teachersList,
                    Number = "",
                    TeacherName = "",
                    TeacherId = null
                };

                Cabinets = CabinetService.GetCabinets();
                StateHasChanged();
            }
            catch (Exception e)
            {
                ShowErrorDialog($"Ошибка сброса фильтра: {e.Message}");
            }
        }

        protected void SelectCabinet(CabinetItemViewModel cabinet)
        {
            try
            {
                EditModel = new EditCabinetModel
                {
                    Model = (CabinetItemViewModel)cabinet.Clone(),
                    Teachers = Teachers,
                    IsOpenDialog = true
                };
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /SelectCabinet. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void AddNewCabinet()
        {
            try
            {
                EditModel = new EditCabinetModel
                {
                    Model = new CabinetItemViewModel(new Db.Models.CabinetModel()),
                    Teachers = Teachers,
                    IsOpenDialog = true
                };
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /AddNewCabinet. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void DeleteCabinet(CabinetItemViewModel cabinet)
        {
            try
            {
                if (cabinet != null)
                {
                    DeleteModel = new();
                    DeleteModel.CabinetDelete = cabinet;
                    DeleteModel.IsOpenDialog = true;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /DeleteCabinet. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            try
            {
                if (confirmed && DeleteModel.CabinetDelete != null)
                {
                    CabinetService.DeleteCabinet(DeleteModel.CabinetDelete);
                    Cabinets = CabinetService.GetCabinets();
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /ConfirmDelete. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {
                DeleteModel.IsOpenDialog = false;
                DeleteModel.CabinetDelete = null;
            }
        }

        protected void SaveChanges(CabinetItemViewModel item)
        {
            try
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
                    Teachers = TeacherService.GetTeachers();
                    StateHasChanged();
                }
                EditModel.IsOpenDialog = false;
                EditModel.IsConcurrency = false;
            }
            catch (DbUpdateConcurrencyException)
            {
                EditModel.IsConcurrency = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /SaveChanges. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {

            }
        }


        protected void HandleReload(CabinetItemViewModel item)
        {
            try
            {
                Cabinets = CabinetService.GetCabinets();
                Teachers = TeacherService.GetTeachers();
                EditModel.Model = CabinetService.GetCabinet(item.Id);
                EditModel.Teachers = Teachers.ToList();
                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка CabinetPage /HandleReload. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

    }
}
