using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using School.Db.Models;
using School.Web.Data.Services;
using School.Web.PageModels;
using School.Web.PageModels.Classes;
using School.Web.PageModels.Students;

namespace School.Web.Pages.ClassModel
{
    public class ClassPageViewModel : BaseViewModel
    {
        [Inject] 
        public ClassModelService ClassModelService { get; set; }
        [Inject] 
        public StudentService StudentService { get; set; }
        protected List<ClassItemViewModel> Classes { get; set; } = new();
        protected EditClassModel EditModel { get; set; } = new();
        protected DeleteClassModel DeleteModel { get; set; } = new();
        protected FilterClassModel FilterClassModel { get; set; } = new();
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
                    FilterClassModel = new FilterClassModel();

                    Classes = ClassModelService.GetClasses();

                    Toaster.Add("Классы загружены.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 3000;
                        conf.ShowProgressBar = true;
                    });
                }

                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка ClassModelPage /OnAfterRenderAsync. {e?.Message} {e?.StackTrace}");
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

                Classes = ClassModelService.GetFilterClasses(
                    FilterClassModel.Number
                );
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /Search. {e?.Message} {e?.StackTrace}");
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
                await Task.Delay(1);

                FilterClassModel.Number = "";

                Classes = ClassModelService.GetClasses();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /ResetFilter. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
            finally
            {
                IsShowSpiner = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected void SelectClass(ClassItemViewModel classItem)
        {
            try
            {
                //SelectedStudent = new StudentItemViewModel(student.Item);
                EditModel = new();
                EditModel.Model = (ClassItemViewModel)classItem.Clone();
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Класс изменен.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 4000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /SelectClass. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void AddNewClass()
        {
            try
            {
                EditModel = new();
                EditModel.Model = new ClassItemViewModel(new School.Db.Models.ClassModel());
                EditModel.IsOpenDialog = true;
                StateHasChanged();

                Toaster.Add("Создан новый класс.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 4000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /AddNewClass. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected async Task DeleteAction(ClassItemViewModel classItem, bool isDeleted)
        {
            try
            {
                if (isDeleted)
                {
                    var hasStudents = await StudentService.HasStudentsInClassAsync(classItem.Id);
                    if (hasStudents)
                    {
                        ShowErrorDialog("Невозможно удалить класс: в классе есть студенты.");
                        return;
                    }
                }

                if (classItem != null)
                {
                    var result = await ClassModelService.RestoreAsync(classItem.Id, isDeleted);

                    if (result)
                    {
                        var updatedClass = ClassModelService.GetClass(classItem.Id);
                        var index = Classes.FindIndex(c => c.Id == classItem.Id);

                        if (index >= 0 && updatedClass != null)
                        {
                            Classes[index] = updatedClass;
                        }

                        StateHasChanged();

                        Toaster.Add(isDeleted ? "Класс перенесен в корзину." : "Класс восстановлен.",
                                   MatBlazor.MatToastType.Info,
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
                Console.WriteLine($"Ошибка ClassModelPage /DeleteAction. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected async Task DeleteClass(ClassItemViewModel classItem)
        {
            try
            {
                var hasStudents = await StudentService.HasStudentsInClassAsync(classItem.Id);

                if (hasStudents)
                {
                    ShowErrorDialog("Невозможно удалить класс: в классе есть студенты.");
                    return;
                }

                if (classItem != null)
                {
                    ClassModelService.DeleteClass(classItem);

                    Classes.RemoveAll(c => c.Id == classItem.Id);
                    StateHasChanged();

                    Toaster.Add("Класс удален.", MatBlazor.MatToastType.Info,
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
                Console.WriteLine($"Ошибка ClassModelPage /DeleteClass. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void ConfirmDelete(bool confirmed)
        {
            try
            {
                if (confirmed && DeleteModel.ClassDelete != null && !DeleteModel.HasStudents)
                {
                    ClassModelService.DeleteClass(DeleteModel.ClassDelete);

                    Classes.RemoveAll(c => c.Id == DeleteModel.ClassDelete.Id);
                    StateHasChanged();
                }

                DeleteModel.IsOpenDialog = false;
                DeleteModel.ClassDelete = null;
                DeleteModel.HasStudents = false;

                Toaster.Add("Класс окончательно удален.", MatBlazor.MatToastType.Info,
                    null, null,
                    conf =>
                    {
                        conf.VisibleStateDuration = 3000;
                        conf.ShowProgressBar = true;
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /ConfirmDelete. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }

        protected void SaveChanges(ClassItemViewModel item)
        {
            try
            {
                if (item != null)
                {
                    if (item.Id == 0)
                    {
                        var newClass = ClassModelService.AddClass(item);
                        Classes.Add(newClass);
                    }
                    else
                    {
                        var result = ClassModelService.Update(item);

                        if (result == null)
                        {
                            ShowErrorDialog("Элемент отсутствует в базе данных.");
                            EditModel.IsOpenDialog = false;
                            return;
                        }

                        var i = Classes.FindIndex(x => x.Id == item.Id);
                        Classes[i] = result;
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
                Console.WriteLine($"Ошибка ClassModelPage /SaveChanges. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }



        public void HandleReload(ClassItemViewModel item)
        {
            try
            {
                var updatedClass = ClassModelService.GetClass(item.Id);

                if (updatedClass != null)
                {
                    var index = Classes.FindIndex(c => c.Id == item.Id);
                    if (index >= 0)
                    {
                        Classes[index] = updatedClass;
                    }

                    EditModel.Model = updatedClass;
                }

                EditModel.IsConcurrency = false;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка ClassModelPage /HandleReload. {e?.Message} {e?.StackTrace}");
                ShowErrorDialog($"Ошибка: {e.Message}");
            }
        }
    }
}
