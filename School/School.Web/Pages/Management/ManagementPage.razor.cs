using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Services;
using School.Web.PageModels;
using School.Web.PageModels.Managements;
using School.Web.PageModels.Students;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Management
{
    public class ManagementPageViewModel : BaseViewModel
    {
        [Inject] ManagementService ManagementService { get; set; }
        protected List<ManagementItemViewModel> Managements { get; set; } = new();
        protected EditManagementModel EditModel { get; set; } = new();
        protected DeleteManagementModel DeleteModel { get; set; } = new();
        protected bool ShowFilters { get; set; } = false;
        protected FilterManagementModel FilterManagement { get; set; }

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
                    FilterManagement = new FilterManagementModel();

                    Search();
                    //Managements = ManagementService.GetManagements();

                    //Toaster.Add("Руководство загружено.", MatBlazor.MatToastType.Info,
                    //null, null,
                    //conf =>
                    //{
                    //    conf.VisibleStateDuration = 15000;
                    //    conf.ShowProgressBar = true;
                    //});
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка ManagementPage /OnAfterRenderAsync. {e?.Message} {e?.StackTrace}");
                    ShowErrorDialog($"Ошибка: {e.Message}");
                }
                finally
                {
                    IsShowSpiner = false;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected void ToggleFilters()
        { 
            ShowFilters = !ShowFilters;
            StateHasChanged();
        }

        public async Task Search()
        {
            try
            {
                IsShowSpiner = true;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(1);

            Managements = ManagementService.GetManagementsFilter(
                FilterManagement.LastName,
                FilterManagement.FirstName,
                FilterManagement.Position
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /Search. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка при поиске: {e.Message}");
            }
            finally
            {
                IsShowSpiner = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task ResetFilter()
        {
            try
            {
                IsShowSpiner = true;
                await InvokeAsync(StateHasChanged);

                FilterManagement.LastName = "";
                FilterManagement.FirstName = "";
                FilterManagement.Position = "";

                Search();
                //Managements = ManagementService.GetManagements();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /ResetFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {
                IsShowSpiner = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        //protected void Update(ManagementItemViewModel management)
        //{
        //    management.MiddleName = management.;
        //    ManagementService.Update(management);
        //}

        protected void AddNewManagement()
        {
            try
            {
                EditModel = new();
                EditModel.Model = new ManagementItemViewModel(new Db.Models.ManagementModel());
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Создан новый руководитель.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /AddNewManagement. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void SelectManagement(ManagementItemViewModel management)
        {
            try
            {
                EditModel = new();
                EditModel.Model = (ManagementItemViewModel)management.Clone();
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Руководитель обновлен.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /SelectManagement. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void SaveChanges(ManagementItemViewModel item)
        {
            try
            {
                //throw new Exception("123");
                if (item != null)
                {
                    if (item.Id == 0)
                    {
                        var newManagement = ManagementService.AddManagement(item);
                        Managements.Add(newManagement);
                    }
                    else
                    {
                        var result = ManagementService.Update(item);

                        if (result == null)
                        {
                            ShowErrorDialog("Руководитель отсутствует в базе данных.");
                            EditModel.IsOpenDialog = false;
                            return;
                        }

                        var i = Managements.FindIndex(x => x.Id == item.Id);
                        Managements[i] = result;
                    }

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
                Console.WriteLine($"Ошибка ManagementPage /SaveChanges. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void DeleteManagement(ManagementItemViewModel management)
        {
            try
            {
                if (management != null)
                {
                    DeleteModel = new();
                    DeleteModel.ManagementDelete = management;
                    DeleteModel.IsOpenDialog = true;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /DeleteManagement. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected async Task DeleteAction(ManagementItemViewModel management, bool isDeleted)
        {
            try
            {
                if (management != null)
                {
                    var result = await ManagementService.RestoreAsync(management.Id, isDeleted);

                    if (result)
                    {
                        var updatedManagement = ManagementService.GetManagement(management.Id);
                        var index = Managements.FindIndex(m => m.Id == management.Id);
                        if (index >= 0)
                        {
                            Managements[index] = updatedManagement;
                        }
                        StateHasChanged();

                        var message = isDeleted ? "Руководитель перенесен в корзину." : "Руководитель восстановлен.";

                        Toaster.Add(message, MatBlazor.MatToastType.Info,
                           null, null,
                           conf =>
                           {
                               conf.VisibleStateDuration = 3000;
                               conf.ShowProgressBar = true;
                           });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /DeleteAction. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            try
            {
                if (confirmed && DeleteModel.ManagementDelete != null)
                {
                    ManagementService.DeleteManagement(DeleteModel.ManagementDelete);
                    Managements = ManagementService.GetManagements();
                    StateHasChanged();
                }
                DeleteModel.IsOpenDialog = false;
                DeleteModel.ManagementDelete = null;

                Toaster.Add("Руководитель удален.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 4000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /ConfirmDelete. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void HandleReload(ManagementItemViewModel item)
        {
            try
            {
                Managements = ManagementService.GetManagements();
                EditModel.Model = ManagementService.GetManagement(item.Id);
                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ManagementPage /HandleReload. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
            Managements?.Clear();
        }

        //protected string GetChangeLog(List<ChangeLogJson> changeLogJsons)
        //{
        //    if (changeLogJsons == null || !changeLogJsons.Any())
        //        return "Нет истории изменений";

        //    var changes = changeLogJsons
        //        .OrderByDescending(x => x.Date)
        //        .Select((change, index) => $"{change.Date:dd.MM.yy HH:mm} - {change.User}: {change.Operation}")
        //        .ToArray();

        //    return string.Join("\n", changes);
        //}
    }
}
