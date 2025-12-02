using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using School.Web.Data.Services;
using School.Web.PageModels.Teachers;

namespace School.Web.Pages.Teacher
{
    public class TeacherPageViewModel : ComponentBase
    {
        [Inject]
        public TeacherService TeacherService { get; set; }
        protected List<TeacherItemViewModel> Teachers { get; set; } = new();
        protected TeacherItemViewModel? SelectedTeacher { get; set; }
        protected EditTeacherModel EditModel { get; set; } = new();
        protected DeleteTeacherModel DeleteModel { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SelectTeacher(TeacherItemViewModel teacher)
        {
            //SelectedTeacher = new TeacherItemViewModel(teacher.Item);
            EditModel = new();
            EditModel.Model = (TeacherItemViewModel)teacher.Clone();
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void Update(TeacherItemViewModel teacher)
        {
            //teacher.FirstName = teacher.FirstName + "2";
            //TeacherService.Update(teacher);
        }

        protected void SaveChanges(TeacherItemViewModel item)
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
        }

        protected void AddNewTeacher()
        {
            EditModel = new();
            EditModel.Model = new TeacherItemViewModel(new Db.Models.TeacherModel());
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void DeleteTeacher(TeacherItemViewModel teacher)
        {
            if (teacher != null)
            {
                DeleteModel = new();
                DeleteModel.TeacherDelete = teacher;
                DeleteModel.IsOpenDialog = true;
                StateHasChanged();
            }
        }
        protected void ConfirmDelete(bool confirmed)
        {
            if (confirmed && DeleteModel.TeacherDelete != null)
            {
                TeacherService.DeleteTeacher(DeleteModel.TeacherDelete);
                Teachers = TeacherService.GetTeachers();
                StateHasChanged();
            }
            DeleteModel.IsOpenDialog = false;
            DeleteModel.TeacherDelete = null;
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
