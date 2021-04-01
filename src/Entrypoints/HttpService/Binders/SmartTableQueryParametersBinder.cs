using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyHome.Dashboard.ViewModels;

namespace MyHome.Dashboard.Binders
{
    public class SmartTableQueryParametersBinder : IModelBinder
    {
        private static readonly Regex FILTER_PATTERN = new Regex(@"^(?<field>\w+)_(?<op>\w+)$");

        public SmartTableQueryParametersBinder()
        {
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelType = bindingContext.ModelType;

            var parameter = Activator.CreateInstance(modelType) as SmartTableQueryParameters;

            var properties = modelType.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var fromQuery = (FromQueryAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(FromQueryAttribute));

                var value = bindingContext.ValueProvider.GetValue(fromQuery.Name).FirstValue;
                if (!string.IsNullOrEmpty(value))
                {
                    propertyInfo.SetValue(parameter, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }

            foreach (var query in bindingContext.HttpContext.Request.Query)
            {
                var match = FILTER_PATTERN.Match(query.Key);
                if (match.Success)
                {
                    var field = match.Groups["field"];
                    var op = match.Groups["op"];
                    parameter.AddFilter(field.Value, op.Value, query.Value.ToString());
                }
            }

            bindingContext.Result = ModelBindingResult.Success(parameter);
            return Task.CompletedTask;
        }
    }
}