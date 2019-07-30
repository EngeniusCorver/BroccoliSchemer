using BroccoliSchemer.Entities;
using System.Collections.Generic;
using System.IO;

namespace BroccoliSchemer.Actions
{
    public class ComponentFilterAction
    {
        public static List<string> GetComponentFilters()
        {
            string[] filters = Directory.GetDirectories(BaseComponent.BASE_PATH);
            List<string> filterNames = new List<string>();
            foreach (var filterName in filters)
            {
                filterNames.Add(filterName.Substring(BaseComponent.BASE_PATH.Length + 3));
            }
            return filterNames;
        }
    }
}
