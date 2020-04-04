using ProxyGeneration.Dto;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Common.Standard.ProxyGeneration
{
    /// <summary>
    /// Class for extracting class information from existing assembly
    /// </summary>
    public class ProxyExtractor
    {
        #region Constants
        #endregion

        #region Ctors and Dtors

        public ProxyExtractor()
        {

        }
        #endregion

        #region Publics

        public ProxyExtraction ExtractAssembly(Assembly assembly)
        {
            if(assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var types = assembly.GetTypes();

            ProxyExtraction extraction = new ProxyExtraction();
            extraction.SourceAssembly = assembly.FullName;
            extraction.TypeExtractions = new List<TypeExtraction>();

            foreach (var type in types)
            {
                ExtractFromType(type, extraction);

            }

            extraction.Timestamp = DateTime.Now;

            return extraction;
        }

        #endregion

        #region Private execution

        private void ExtractFromType(Type type, ProxyExtraction extraction)
        {
            var name = type.Name;
            if (type.IsGenericType)
            {
                int pos = name.IndexOf("`");
                name = $"{type.Name.Substring(0, pos)}<T>";
            }

            TypeExtraction typExtracted = new TypeExtraction()
            {
                Name = name,
                Namespace = type.Namespace,
                Fields = new List<FieldExtraction>(),
                Properties = new List<PropertyExtraction>(),
                Methods = new List<MethodExtraction>()
            };

            foreach (var fieldInfo in type.GetFields())
            {
                ExtractFromField(fieldInfo, typExtracted);
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                ExtractFromProperty(propertyInfo, typExtracted);
            }

            foreach (var methodInfo in type.GetMethods())
            {
                ExtractFromMethod(methodInfo, typExtracted);
            }

            extraction.TypeExtractions.Add(typExtracted);
        }

        private void ExtractFromField(FieldInfo fieldInfo, TypeExtraction typExtracted)
        {
            FieldExtraction fextraction = new FieldExtraction()
            {
                Name = fieldInfo.Name,
                TypeName = fieldInfo.FieldType.ToString()
            };

            typExtracted.Fields.Add(fextraction);
        }

        private void ExtractFromProperty(PropertyInfo propInfo, TypeExtraction typExtracted)
        {
            FieldExtraction fextraction = new FieldExtraction()
            {
                Name = $"_{propInfo.Name}",
                TypeName = propInfo.PropertyType.ToString()
            };

            typExtracted.Fields.Add(fextraction);

            PropertyExtraction pextraction = new PropertyExtraction()
            {
                Name = propInfo.Name,
                TypeName = propInfo.PropertyType.ToString(),
                HasGet = propInfo.CanRead,
                HasSet = propInfo.CanWrite
            };

            typExtracted.Properties.Add(pextraction);
        }

        private void ExtractFromMethod(MethodInfo methodInfo, TypeExtraction typExtracted)
        {
            MethodExtraction mextraction = new MethodExtraction()
            {
                Name = methodInfo.Name,
                ReturnValueType = methodInfo.ReturnType.Name,
                Signature = methodInfo.ToString(),
                Arguments = new List<MethodArgument>()
                {

                }
            };

            var parameters = methodInfo.GetParameters();

            foreach (var parameterInfo in parameters)
            {
                MethodArgument arg = new MethodArgument()
                {
                    Name = parameterInfo.Name,
                    Type = parameterInfo.ParameterType.ToString()
                };

                foreach (var parameterInfoCustomAttribute in parameterInfo.CustomAttributes)
                {
                    if (parameterInfoCustomAttribute.AttributeType == typeof(ParamArrayAttribute))
                    {
                        arg.IsParams = true;
                        break;
                    }
                }

                mextraction.Arguments.Add(arg);
            }

            typExtracted.Methods.Add(mextraction);
        }
        #endregion

        #region Private Name creation

        #endregion

    }
}
