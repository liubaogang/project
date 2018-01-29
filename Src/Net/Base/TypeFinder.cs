namespace Net.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Text.RegularExpressions;

    public class TypeFinder
    {
        private static IEnumerable<Type> FromCheckedAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany<Assembly, Type>(new Func<Assembly, IEnumerable<Type>>(TypeFinder.GetTypes));
        }

        private static IEnumerable<Assembly> GetAssembliesInBasePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return (from an in GetAssemblyNames(baseDirectory, true).Union<string>(GetAssemblyNames(Path.Combine(baseDirectory, "bin"), true))
                    select LoadAssembly(Path.GetFileNameWithoutExtension(an), true) into a
                    where (a != null) && !a.FullName.StartsWith("System.")
                    select a);
        }

        private static IEnumerable<string> GetAssemblyNames(string path, bool skipOnError)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    return new string[0];
                }
                return (from w in Directory.EnumerateFiles(path, "*.dll").Concat<string>(Directory.EnumerateFiles(path, "*.exe"))
                        where !Regex.IsMatch(w, ".vshost.")
                        select w);
            }
            catch (Exception exception)
            {
                if (!skipOnError || ((!(exception is DirectoryNotFoundException) && !(exception is IOException)) && (!(exception is SecurityException) && !(exception is UnauthorizedAccessException))))
                {
                    throw;
                }
                return new string[0];
            }
        }

        public static IEnumerable<Type> GetTypes()
        {
            return FromCheckedAssemblies(GetAssembliesInBasePath());
        }

        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            IEnumerable<TypeInfo> enumerable;
            if (assembly == null)
            {
                return Enumerable.Empty<Type>();
            }
            try
            {
                enumerable = assembly.DefinedTypes.ToArray<TypeInfo>();
            }
            catch (ReflectionTypeLoadException exception1)
            {
                enumerable = (from t in exception1.Types
                              where t != null
                              select t.GetTypeInfo()).ToArray<TypeInfo>();
            }
            return (from ti in enumerable
                    where ((ti != null) && (ti.IsClass & !ti.IsAbstract)) && !ti.IsValueType
                    select ti.AsType()).ToArray<Type>();
        }

        public static IEnumerable<Type> GetTypes(string productName)
        {
            return FromCheckedAssemblies((from w in GetAssembliesInBasePath()
                                          where IsAssembly(w, productName)
                                          select w).ToArray<Assembly>());
        }

        public static IEnumerable<Type> GetTypes(Type baseType)
        {
            return FromCheckedAssemblies(GetAssembliesInBasePath().ToArray<Assembly>()).Where<Type>(new Func<Type, bool>(baseType.IsAssignableFrom));
        }

        public static IEnumerable<Type> GetTypes(Assembly assembly, Type baseType)
        {
            return GetTypes(assembly).Where<Type>(new Func<Type, bool>(baseType.IsAssignableFrom));
        }

        private static bool IsAssembly(Assembly assembly, string productName)
        {
            AssemblyProductAttribute customAttribute = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            return ((customAttribute != null) && (string.Compare(productName, customAttribute.Product, StringComparison.Ordinal) == 0));
        }

        private static Assembly LoadAssembly(string assemblyName, bool skipOnError)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception exception)
            {
                if (!skipOnError || ((!(exception is FileNotFoundException) && !(exception is FileLoadException)) && !(exception is BadImageFormatException)))
                {
                    throw;
                }
                return null;
            }
        }
    }
}
