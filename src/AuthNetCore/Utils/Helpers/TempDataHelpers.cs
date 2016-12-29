using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AuthNetCore.Utils.Helpers
{
    public static class TempDataHelpers
    {
        public static string SerializeModelState(ModelStateDictionary modelState)
        {
            var errorList = modelState.Select(ms => new ModelStateTransferValue
            {
                Key = ms.Key,
                AttemptedValue = ms.Value.AttemptedValue,
                RawValue = ms.Value.RawValue,
                ErrorMessages = ms.Value.Errors.Select(e => e.ErrorMessage)
            });

            return JsonConvert.SerializeObject(errorList);
        }

        public static ModelStateDictionary DeserializeModelState(string serialize)
        {
            var errorList = JsonConvert.DeserializeObject<IEnumerable<ModelStateTransferValue>>(serialize);
            var modelState = new ModelStateDictionary();

            foreach (var item in errorList)
            {
                modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);

                foreach (var error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }

            return modelState;
        }
    }

    public class ModelStateTransferValue
    {
        public string Key { get; set; }
        public string AttemptedValue { get; set; }
        public object RawValue { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
