using Microsoft.AspNetCore.Components;
using School.Db.Models;
using School.Web.Data.Services;

namespace School.Web.Pages.Student
{
    public class StudentPageViewModel : ComponentBase
    {
        [Inject] 
        public StudentService StudentService { get; set; }

        protected List<StudentItemViewModel> Students { get; set; } = new();
        protected StudentItemViewModel? SelectedStudent { get; set; }

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
            SelectedStudent = new StudentItemViewModel(student.Item);
        }

        protected void SaveChanges()
        {
            if (SelectedStudent != null)
            {
                if (SelectedStudent.Id == 0)
                {
                    StudentService.AddStudent(SelectedStudent);
                }
                else
                {
                    StudentService.Update(SelectedStudent);
                }

                Students = StudentService.GetStudents();
                SelectedStudent = null;
                StateHasChanged();
            }
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
            SelectedStudent = new StudentItemViewModel(new StudentModel());
        }
    }
}
