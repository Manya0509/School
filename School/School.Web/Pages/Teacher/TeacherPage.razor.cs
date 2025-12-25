using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Services;
using School.Web.PageModels;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Teacher
{
    public class TeacherPageViewModel : BaseViewModel
    {
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected TeacherItemViewModel? SelectedTeacher { get; set; }
        protected EditTeacherModel EditModel { get; set; } = new();
        protected DeleteTeacherModel DeleteModel { get; set; } = new();
        protected FilterTeacherModel FilterTeacher { get; set; }
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
                    FilterTeacher = new FilterTeacherModel();

                    Teachers = TeacherService.GetTeachers();

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
                    Console.WriteLine($"Ошибка TeacherPage /OnAfterRenderAsync. {e?.Message} {e?.StackTrace}");
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

                Teachers = TeacherService.GetTeachersFilter(
                FilterTeacher.FirstName,
                FilterTeacher.LastName,
                FilterTeacher.SubjectName
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /Search. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
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
                await Task.Delay(1);

                FilterTeacher.FirstName = "";
                FilterTeacher.LastName = "";
                FilterTeacher.SubjectName = "";

                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /ResetFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {
                IsShowSpiner = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected void SelectTeacher(TeacherItemViewModel teacher)
        {
            try
            {
                //SelectedTeacher = new TeacherItemViewModel(teacher.Item);
                EditModel = new();
                EditModel.Model = (TeacherItemViewModel)teacher.Clone();
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Преподаватель обновлен.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /SelectTeacher. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void Update(TeacherItemViewModel teacher)
        {
            //teacher.FirstName = teacher.FirstName + "2";
            //TeacherService.Update(teacher);
        }

        protected void SaveChanges(TeacherItemViewModel item)
        {
            try
            {
                if (item != null)
                {
                    if (item.Id == 0)
                    {
                        TeacherService.AddTeacher(item);
                    }
                    else
                    {
                        TeacherService.Update(item);
                    }
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
                Console.WriteLine($"Ошибка TeacherPage /SaveChanges. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void AddNewTeacher()
        {
            try
            {
                EditModel = new();
                EditModel.Model = new TeacherItemViewModel(new Db.Models.TeacherModel());
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Создан новый преподаватель.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /AddNewTeacher. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void DeleteTeacher(TeacherItemViewModel teacher)
        {
            try
            {
                if (teacher != null)
                {
                    DeleteModel = new();
                    DeleteModel.TeacherDelete = teacher;
                    DeleteModel.IsOpenDialog = true;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /DeleteTeacher. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }
        protected void ConfirmDelete(bool confirmed)
        {
            try
            {
                if (confirmed && DeleteModel.TeacherDelete != null)
                {
                    TeacherService.DeleteTeacher(DeleteModel.TeacherDelete);
                    Teachers = TeacherService.GetTeachers();
                    StateHasChanged();
                }
                DeleteModel.IsOpenDialog = false;
                DeleteModel.TeacherDelete = null;

                Toaster.Add("Преподаватель был удален.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 75000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {

                Console.WriteLine($"Ошибка TeacherPage /ConfirmDelete. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        public void HandleReload(TeacherItemViewModel item)
        {
            try
            {
                Teachers = TeacherService.GetTeachers();
                EditModel.Model = TeacherService.GetTeacher(item.Id);
                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка TeacherPage /HandleReload. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
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
