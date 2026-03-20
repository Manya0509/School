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
        protected EditStudentModel EditModel { get; set; } = new();
        protected DeleteStudentModel DeleteModel { get; set; } = new();
        protected FilterStudentModel FilterStudent { get; set; }
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
                    FilterStudent = new FilterStudentModel();
                    FilterStudent.Clear();
                    InitFilter();

                    Search();

                    //Students = StudentService.GetStudents();

                    //Toaster.Add("Студенты загружены.", MatBlazor.MatToastType.Info,
                    //null, null,
                    //conf =>
                    //{
                    //    conf.VisibleStateDuration = 3000;
                    //    conf.ShowProgressBar = true;
                    //});
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка StudentPage /OnAfterRenderAsync. {e?.Message} {e?.StackTrace}");
                    ShowErrorDialog($"Ошибка: {e.Message}");
                }
                finally
                {
                    IsShowSpiner = false;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected async Task InitFilter()
        {
            FilterStudent.Classes = ClassModelService.GetFilterModels();
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

                Students = StudentService.GetStudentsFilter(
                    FilterStudent.FirstName,
                    FilterStudent.LastName,
                    FilterStudent.ClassId
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /Search. {e?.Message} {e?.StackTrace}");
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
                FilterStudent.FirstName = "";
                FilterStudent.LastName = "";
                FilterStudent.ClassId = 0;

                Search();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /ResetFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
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

                Toaster.Add("Студент изменен.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 4000;
                        conf.ShowProgressBar = true;
                    });
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
                        var a = StudentService.AddStudent(item);
                        Students.Add(a);
                    }
                    else
                    {
                        var result = StudentService.Update(item);

                        if (result == null)
                        {
                            ShowErrorDialog("Студент отсутствует в базе данных.");
                            EditModel.IsOpenDialog = false;
                            return;
                        }

                        var i = Students.FindIndex(x => x.Id == item.Id);
                        Students[i] = result;
                    }

                    //Students = StudentService.GetStudents();
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

                Toaster.Add("Создан новый студент.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 15000;
                        conf.ShowProgressBar = true;
                    });
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
                    StudentService.DeleteStudent(student);


                    Students.RemoveAll(s => s.Id == student.Id);
                    StateHasChanged();
                        Toaster.Add("Студент удален.", MatBlazor.MatToastType.Info,
                           null, null,
                           conf =>
                           {
                               conf.VisibleStateDuration = 3000;
                               conf.ShowProgressBar = true;
                           });
                   
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Ошибка StudentPage /DeleteStudent. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected async Task DeleteAction(StudentItemViewModel student, bool isDeleted)
        {
            try
            {
                if (student != null)
                {
                        var result = await StudentService.RestoreAsync(student.Id, isDeleted);

                        if (result)
                        {
                        var updatedStudent = StudentService.GetStudent(student.Id);
                        var index = Students.FindIndex(s => s.Id == student.Id);
                        if (index >= 0)
                        {
                            Students[index] = updatedStudent;
                        }
                        StateHasChanged();

                        var message = isDeleted ? "Студент перенесен в корзину." : "Студент восстановлен.";

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
                Console.WriteLine($"Ошибка StudentPage /DeleteAction. {e?.Message} {e?.StackTrace}");
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
                    Students.RemoveAll(s => s.Id == DeleteModel.StudentDelete.Id);
                    StateHasChanged();
                }
                DeleteModel.IsOpenDialog = false;
                DeleteModel.StudentDelete = null;

                Toaster.Add("Студент удален.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 75000;
                        conf.ShowProgressBar = true;
                    });
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
                var updatedStudent = StudentService.GetStudent(item.Id);

                if (updatedStudent != null)
                {
                    var index = Students.FindIndex(s => s.Id == item.Id);
                    if (index >= 0)
                    {
                        Students[index] = updatedStudent;
                    }

                    EditModel.Model = updatedStudent;
                }

                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка StudentPage /HandleReload. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
            Students?.Clear();
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
