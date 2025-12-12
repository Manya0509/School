using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using School.Db.Models;
using School.Web.Data.Services;
using School.Web.PageModels;
using School.Web.PageModels.Students;

namespace School.Web.Pages.Student
{
    public class StudentPageViewModel : BaseViewModel
    {
        [Inject] 
        public StudentService StudentService { get; set; }
        [Inject]
        public ClassModelService ClassModelService { get; set; }

        protected List<StudentItemViewModel> Students { get; set; } = new();
        protected StudentItemViewModel? SelectedStudent { get; set; }

        protected EditStudentModel EditModel { get; set; } = new();
        protected DeleteStudentModel DeleteModel { get; set; } = new();
        protected FilterStudentModel FilterStudent { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
             if (firstRender)
                {
                InitFilter();
                    Students = StudentService.GetStudents();
                    StateHasChanged();
             }
            return base.OnAfterRenderAsync(firstRender);
        }

        public void InitFilter()
        {
            try
            {
                FilterStudent = new();
                FilterStudent.Classes = ClassModelService.GetFilterModels();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /InitFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        public void Search()
        {
            Students = StudentService.GetStudents(FilterStudent.FirstName, FilterStudent.LastName, FilterStudent.ClassId);
        }

        protected void SelectStudent(StudentItemViewModel student)
        {
            try
            {
                //SelectedStudent = new StudentItemViewModel(student.Item);
                EditModel = new();
                EditModel.Model = (StudentItemViewModel)student.Clone();
                EditModel.Classes = ClassModelService.GetClassesModel();
                EditModel.IsOpenDialog = true;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /SelectStudent. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void SaveChanges(StudentItemViewModel item)
        {
            try
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
                EditModel.IsConcurrency = false;
            }
            catch (DbUpdateConcurrencyException)
            {
                EditModel.IsConcurrency = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /SaveChanges. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void Update(StudentItemViewModel student)
        {
            //student.MiddleName = student.;
            //StudentService.Update(student);
        }

        protected void AddNewStudent()
        {
            try
            {
                //SelectedStudent = new StudentItemViewModel(new StudentModel());
                EditModel = new();
                EditModel.Classes = ClassModelService.GetClassesModel();
                EditModel.Model = new StudentItemViewModel(new StudentModel());
                EditModel.IsOpenDialog = true;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /AddNewStudent. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void DeleteStudent(StudentItemViewModel student)
        {
            try
            {
                if (student != null)
                {
                    DeleteModel = new();
                    DeleteModel.StudentDelete = student;
                    DeleteModel.IsOpenDialog = true;
                    StateHasChanged();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Ошибка StudentPage /DeleteStudent. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /ConfirmDelete. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        public void HandleReload(StudentItemViewModel item)
        {
            try
            {
                Students = StudentService.GetStudents();
                EditModel.Model = StudentService.GetStudent(item.Id);
                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /HandleReload. {e?.Message} {e?.StackTrace}");
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
