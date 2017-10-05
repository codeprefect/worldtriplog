using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WorldTripLog.Web.Helpers
{
    public static class ModelStateDictionaryHelper
    {
        public static string ToStringResponse(this ModelStateDictionary model)
        {
            var result = new StringBuilder();
            result.Append("Hello you!");
            return result.ToString();
        }
    }
}
