using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace AuthNetCore.Utils.Extensions
{
    public static class TempDataExtensions
    {
        static readonly string VIEWMODEL_KEY = "ViewModel";

        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return (o is null) ? null : JsonConvert.DeserializeObject<T>(o as string);
        }

        public static void PutViewModel<T>(this ITempDataDictionary tempData, T value) where T : class
        {
            tempData.Put(VIEWMODEL_KEY, value);
        }

        public static T GetViewModel<T>(this ITempDataDictionary tempData) where T : class => tempData.Get<T>(VIEWMODEL_KEY);
    }
}
