using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;

namespace School.Web.PageModels
{
    public class BaseViewModel : ComponentBase, IDisposable
    {
        [Inject] protected MatBlazor.IMatToaster Toaster { get; set; }
        protected bool IsOpenErrorDialog { get; set; }
        protected string ErrorMessage { get; set; }
        protected bool IsShowSpiner { get; set; } = false;
        bool disposed = false;
        protected void ShowErrorDialog(string message)
        {
            ErrorMessage = message;
            IsOpenErrorDialog = true;
            StateHasChanged();
        }

        protected void CloseErrorDialog()
        {
            IsOpenErrorDialog = false;
            ErrorMessage = string.Empty;
            StateHasChanged();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
            //GC.Collect(2);
        }

        ~BaseViewModel()
        {
            Dispose(false);
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
