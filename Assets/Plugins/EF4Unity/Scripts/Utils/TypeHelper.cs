using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class TypeHelper
    {
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            if (!generic.IsGenericTypeDefinition)
            {
                return false;
            }
            
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        
        public static List<Type> GetDerivedTypes<TBase>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && (typeof(TBase).IsAssignableFrom(type)
                                                                    || IsSubclassOfRawGeneric(typeof(TBase), type)))
                .ToList();
        }
        
        public static List<Type> GetDerivedTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && (baseType.IsAssignableFrom(type) 
                               || IsSubclassOfRawGeneric(baseType, type)))
                .ToList();
        }
    }
}