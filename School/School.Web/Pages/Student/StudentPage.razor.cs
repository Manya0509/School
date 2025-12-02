using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;
using School.Web.PageModels.Students;

namespace School.Web.Pages.Student
{
    public class StudentPageViewModel : ComponentBase
    {
        [Inject] 
        public StudentService StudentService { get; set; }
        [Inject]
        public ClassModelService ClassModelService { get; set; }

        protected List<StudentItemViewModel> Students { get; set; } = new();
        protected StudentItemViewModel? SelectedStudent { get; set; }

        protected EditStudentModel EditModel { get; set; } = new();
        protected DeleteStudentModel DeleteModel { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Students = StudentService.GetStudents();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected void SelectStudent(StudentItemViewModel student)
        {
            //SelectedStudent = new StudentItemViewModel(student.Item);
            EditModel = new();
            EditModel.Model = (StudentItemViewModel)student.Clone();
            EditModel.Classes = ClassModelService.GetClassesModel();
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void SaveChanges(StudentItemViewModel item)
        {
            if (item != null)
            {
                if (item.Id == 0)
                {
                    StudentService.AddStudent(item);
                }
                else
                {
                    StudentService.Update(item);
                }

                Students = StudentService.GetStudents();
                StateHasChanged();
            }
            EditModel.IsOpenDialog = false;
        }

        protected void Update(StudentItemViewModel student)
        {
            //student.MiddleName = student.;
            //StudentService.Update(student);
        }

        protected void AddNewStudent()
        {
            //SelectedStudent = new StudentItemViewModel(new StudentModel());
            EditModel = new();
            EditModel.Classes = ClassModelService.GetClassesModel();
            EditModel.Model = new StudentItemViewModel(new StudentModel());
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }

        protected void DeleteStudent(StudentItemViewModel student)
        {
            if (student != null)
            { 
                DeleteModel = new();
                DeleteModel.StudentDelete = student;
                DeleteModel.IsOpenDialog = true;
                StateHasChanged();
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            if (confirmed && DeleteModel.StudentDelete != null)
            {
                StudentService.DeleteStudent(DeleteModel.StudentDelete);
                Students = StudentService.GetStudents();
                StateHasChanged();
            }
            DeleteModel.IsOpenDialog = false;
            DeleteModel.StudentDelete = null;
        }

        protected string GetChangeLog(List<ChangeLogJson> changeLogJsons)
        {
            if (changeLogJsons == null || !changeLogJsons.Any())
                return "Нет истории изменений";

            var changes = changeLogJsons
                .OrderByDescending(x => x.Date)
                .Select((change, index) => $"{change.Date:dd.MM.yy HH:mm} - {change.User}: {change.Operation}")
                .ToArray();

            return string.Join("\n", changes);
        }
    }
}
