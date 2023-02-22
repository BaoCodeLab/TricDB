using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.platform
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //{2} is area, {1} is controller,{0} is the action
            var definedViewLocation = context.ActionContext.ActionDescriptor.FilterDescriptors.Where(w => w.Filter.GetType() == typeof(ViewLocationAttribute));
            if (definedViewLocation.Any())
            {
                var viewLocation = definedViewLocation.First().Filter as ViewLocationAttribute;
                if (viewLocations.Any(w => w == viewLocation.Path))
                {
                    return null;

                }
                else
                {
                    string[] locations = new string[] { viewLocation.Path };
                    return locations;
                }
            }
            else
            {
                return viewLocations;
            }
        }


        public void PopulateValues(ViewLocationExpanderContext context)
        {

            context.Values["customviewlocation"] = nameof(ViewLocationExpander);
        }
    }
}
