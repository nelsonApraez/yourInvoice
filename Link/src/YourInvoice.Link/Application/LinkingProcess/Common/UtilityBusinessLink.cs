///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using System.Reflection;
using yourInvoice.Common.Business.CatalogModule;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.Common
{
    public static class UtilityBusinessLink
    {
        public static bool ValidateIsFieldEmpty<T>(T entity, List<string> fieldNotValidated = default)
        {
            bool isNotValidateEmpty = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                PropertyInfo propertyData = typeof(T).GetProperty(property.Name);
                var valueProperty = propertyData?.GetValue(entity);
                isNotValidateEmpty = fieldNotValidated is null ? isNotValidateEmpty : fieldNotValidated.Exists(a => a.Equals(property.Name));
                if (isNotValidateEmpty)
                {
                    continue;
                }
                if (valueProperty is null || string.IsNullOrEmpty(Convert.ToString(valueProperty)))
                {
                    return true;
                }
                var validValueData = Convert.ToString(valueProperty);
                var typeData = propertyData?.PropertyType?.Name ?? string.Empty;

                if (typeData.Equals("Guid") && Guid.Parse(validValueData) == Guid.Empty)
                {
                    return true;
                }
                else if (typeData.Equals("String") && string.IsNullOrEmpty(validValueData))
                {
                    return true;
                }
            }
            return false;
        }

        public static Guid ValidateIsFieldEmptyStatus<T>(T entity, List<string> fieldNotValidated = default)
        {
            if (entity == null)
            {
                return CatalogCodeLink_StatusForm.WithoutStarting;
            }
            bool isNotValidateEmpty = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                PropertyInfo propertyData = typeof(T).GetProperty(property.Name);
                var valueProperty = propertyData?.GetValue(entity);
                isNotValidateEmpty = fieldNotValidated is null ? isNotValidateEmpty : fieldNotValidated.Exists(a => a.Equals(property.Name));
                if (isNotValidateEmpty)
                {
                    continue;
                }
                if (valueProperty is null || string.IsNullOrEmpty(Convert.ToString(valueProperty)))
                {
                    return CatalogCodeLink_StatusForm.InProgress;
                }
                var validValueData = Convert.ToString(valueProperty);
                var typeData = propertyData?.PropertyType?.Name ?? string.Empty;

                if (typeData.Equals("Guid") && Guid.Parse(validValueData) == Guid.Empty)
                {
                    return CatalogCodeLink_StatusForm.InProgress;
                }
                else if (typeData.Equals("String") && string.IsNullOrEmpty(validValueData))
                {
                    return CatalogCodeLink_StatusForm.InProgress;
                }
            }
            return CatalogCodeLink_StatusForm.Complete;
        }

        public static TD PassDataOriginDestiny<TO, TD>(TO dataOrigin, TD dataDestiny)
        {
            var properties = typeof(TO).GetProperties();
            var objTInstanceTD = (TD)Activator.CreateInstance(typeof(TD));
            foreach (var property in properties)
            {
                if (property.Name != "OperationsTypes")
                {
                    var propertyDataOrigin = typeof(TO).GetProperty(property.Name);
                    var valuePropertyOrigin = propertyDataOrigin?.GetValue(dataOrigin);
                    var propertyDataDestiny = typeof(TD).GetProperty(property.Name);
                    propertyDataDestiny.SetValue(objTInstanceTD, valuePropertyOrigin);
                }
            }

            return objTInstanceTD;
        }
    }
}