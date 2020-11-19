using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RedisConsoleDesktop.Models;

namespace RedisConsoleDesktop.ModelBinders
{
    public class InstanceSettingsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));



            var name = bindingContext.ValueProvider.GetValue("Name").FirstValue;
            var host = bindingContext.ValueProvider.GetValue("Host").FirstValue;
            var port = int.Parse(bindingContext.ValueProvider.GetValue("Port").FirstValue);
            var auth = bindingContext.ValueProvider.GetValue("Auth").FirstValue;


            var result = new InstanceSettingsViewModel()
            {
                Name = name,
                Host = host,
                Port = port,
                Auth = auth

            };


            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}
