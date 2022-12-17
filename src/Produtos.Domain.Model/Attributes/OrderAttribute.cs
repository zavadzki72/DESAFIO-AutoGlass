using Produtos.Domain.Model.ApiContracts;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Produtos.Domain.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OrderAttribute : ValidationAttribute
    {
        public string InvalidFormatErrorMessage { get; }
        public string InvalidFieldErrorMessage { get; }
        public List<string> InvalidFields { get; } = new();

        public OrderAttribute(string invalidFormatErrorMessage, string invalidFieldErrorMessage)
        {
            InvalidFormatErrorMessage = invalidFormatErrorMessage;
            InvalidFieldErrorMessage = invalidFieldErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;

            string valueStr = value?.ToString();

            if (string.IsNullOrWhiteSpace(valueStr))
            {
                return result;
            }

            valueStr += " ";

            var isFormatValid = Regex.IsMatch(valueStr, @"^.*?\s(asc|desc|:?)$");

            if (!isFormatValid)
            {
                return new ValidationResult(InvalidFormatErrorMessage);
            }

            var genricType = GetGenericType(validationContext);

            if (genricType == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            Dictionary<string, string> fieldToSort = (Dictionary<string, string>)validationContext.ObjectInstance.GetType().GetProperty("FieldOrders")?.GetValue(validationContext.ObjectInstance);

            if (fieldToSort == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var listOrders = valueStr.Split(",");
            foreach (var field in listOrders)
            {
                if (!ValidateField(field.Trim(), genricType, fieldToSort))
                {
                    return InvalidFields.Any() ? new ValidationResult(InvalidFields.FirstOrDefault()) : new ValidationResult(ErrorMessage);
                }
            }

            return result;
        }

        private bool ValidateField(string field, Type dtoType, Dictionary<string, string> fieldToOrders)
        {
            var fieldSplit = field.Split(" ");
            string order = "asc";

            if (fieldSplit.Length < 1 || fieldSplit.Length > 2)
            {
                return false;
            }

            var fieldToOrder = fieldSplit[0];

            if (fieldSplit.Length == 2)
            {
                var ordering = fieldSplit[1].ToLower();

                if (ordering != "asc" && ordering != "desc")
                {
                    return false;
                }

                order = ordering;
            }

            var dtoField = dtoType.GetProperty(fieldToOrder, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (dtoField == null)
            {
                InvalidFields.Add(string.Format(InvalidFieldErrorMessage, fieldToOrder));
                return false;
            }

            fieldToOrders.Add(fieldToOrder, order);

            return true;
        }

        private static Type GetGenericType(ValidationContext validationContext)
        {
            var genericTypes = validationContext.ObjectType.BaseType?.GenericTypeArguments;
            Type genricType = null;

            if (genericTypes == null)
            {
                return genricType;
            }

            foreach (var type in genericTypes)
            {
                var baseType = type.BaseType;

                if (baseType == null)
                {
                    continue;
                }

                if (baseType.Name.Equals(nameof(PaginatedResult)))
                {
                    genricType = GetDtoType(type);
                    break;
                }
            }

            return genricType;
        }

        private static Type GetDtoType(Type paginatedResultType)
        {
            var listType = paginatedResultType.GenericTypeArguments[0];
            var type = listType.GenericTypeArguments[0];

            return type;
        }
    }
}
