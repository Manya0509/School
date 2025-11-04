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

        protected List<StudentItemViewModel> Students { get; set; } = new();
        protected StudentItemViewModel? SelectedStudent { get; set; }

        protected EditStudentModel EditModel { get; set; } = new();

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
                item = null;
                StateHasChanged();
            }
            EditModel.IsOpenDialog = false;
        }

        protected void CancelEdit()
        {
            SelectedStudent = null;
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
            EditModel.Model = new StudentItemViewModel(new StudentModel());
            EditModel.IsOpenDialog = true;
            StateHasChanged();
        }
    }
}
