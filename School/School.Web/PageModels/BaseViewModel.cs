using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.AspNetCore.Components;

namespace School.Web.PageModels
{
    public class BaseViewModel : ComponentBase
    {
        [Inject] protected MatBlazor.IMatToaster Toaster { get; set; }
        protected bool IsOpenErrorDialog { get; set; }
        protected string ErrorMessage { get; set; }
        protected bool IsShowSpiner { get; set; } = false;
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
