using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Helpers
{
    public static class EnumSelectListHelper
    {
        /// <summary>
        /// Retorna SelectListItems para um enum, excluindo membros marcados com [ObsoleteAttribute].
        /// Mantém a ordem natural do enum.
        /// </summary>
        public static IEnumerable<SelectListItem> GetEnumSelectListExcludeObsolete<T>() where T : struct, Enum
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();

            foreach (var val in values)
            {
                var fi = type.GetField(val.ToString());
                if (fi == null) continue;

                // se marcado Obsolete, pular
                var isObsolete = fi.GetCustomAttribute<ObsoleteAttribute>() != null;
                if (isObsolete) continue;

                // display name (se existir DisplayAttribute)
                var displayAttr = fi.GetCustomAttribute<DisplayAttribute>();
                var text = displayAttr != null ? displayAttr.Name : val.ToString();

                yield return new SelectListItem
                {
                    Text = text,
                    Value = Convert.ToInt32(val).ToString()
                };
            }
        }
    }
}