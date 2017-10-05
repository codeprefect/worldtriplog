using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WorldTripLog.Web.Helpers
{
    public static class ModelStateDictionaryHelper
    {
        public static List<string> ToStringResponse(this ModelStateDictionary model)
        {
            var result = new List<string>();
            foreach (var value in model.Values)
            {
                result.AddRange(value.Errors.Select(x => x.ErrorMessage));
            }
            return result;
        }
    }
}
