using AuthNetCore.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthNetCore.Filters
{
    public abstract class ModelStateFilterAttribute : ActionFilterAttribute
    {
        protected const string KEY = nameof(ModelStateFilterAttribute);
    }

    public class ExportModelStateAttribute : ModelStateFilterAttribute
    {
        public ModelStateDictionary ModelStateHelpers { get; private set; }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var modelState = context.ModelState;
            var controller = context.Controller as Controller;

            if (
                !(context.ModelState is null) && 
                !(context.Controller is null) && 
                !modelState.IsValid && 
                (
                    context.Result is RedirectResult ||
                    context.Result is RedirectToActionResult ||
                    context.Result is RedirectToRouteResult
                )
               )
            {
                controller.TempData[KEY] = TempDataHelpers.SerializeModelState(modelState);
            }

            base.OnActionExecuted(context);
        }
    }

    public class ImportModelStateAttribute : ModelStateFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            var serialized = controller?.TempData[KEY] as string;

            if (!(serialized is null) && context.Result is ViewResult)
            {
                var modelState = TempDataHelpers.DeserializeModelState(serialized);
                context.ModelState.Merge(modelState);
            }
            else if (!(controller is null))
                controller.TempData.Remove(KEY);

            base.OnActionExecuted(context);
        }
    }
}
