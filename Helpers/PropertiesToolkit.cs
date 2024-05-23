using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class PropertiesToolkit
    {
        /// <summary>
        /// Copies all the matching properties and fields from 'source' to 'destination'
        /// </summary>
        /// <param name="source">The source object to copy from</param>  
        /// <param name="destination">The destination object to copy to</param>
        public static void CopyPropsTo<T1, T2>(this T1 source, ref T2 destination)
        {
            var sourceMembers = GetMembers(source.GetType());
            var destinationMembers = GetMembers(destination.GetType());
            
            // Copy data from source to destination
            foreach (var sourceMember in sourceMembers)
            {
                if (!CanRead(sourceMember))
                {
                    continue;
                }
                var destinationMember = destinationMembers.FirstOrDefault(x => x.Name.ToLower() == sourceMember.Name.ToLower());
                if (destinationMember == null || !CanWrite(destinationMember))
                {
                    continue;
                }
                SetObjectValue(ref destination, destinationMember, GetMemberValue(source, sourceMember));
            }
        }

        private static void SetObjectValue<T>(ref T obj, System.Reflection.MemberInfo member, object value)
        {
            // Boxing method used for modifying structures
            var boxed = obj.GetType().IsValueType ? (object)obj : obj;
            SetMemberValue(ref boxed, member, value);
            obj = (T)boxed;
        }

        private static void SetMemberValue<T>(ref T obj, System.Reflection.MemberInfo member, object value)
        {
            if (IsProperty(member))
            {
                var prop = (System.Reflection.PropertyInfo)member;
                if (prop.SetMethod != null)
                {
                    prop.SetValue(obj, value);
                }
            }
            else if (IsField(member))
            {
                var field = (System.Reflection.FieldInfo)member;
                field.SetValue(obj, value);
            }
        }

        private static object GetMemberValue(object obj, System.Reflection.MemberInfo member)
        {
            object result = null;
            if (IsProperty(member))
            {
                var prop = (System.Reflection.PropertyInfo)member;
                result = prop.GetValue(obj, prop.GetIndexParameters().Count() == 1 ? new object[] { null } : null);
            }
            else if (IsField(member))
            {
                var field = (System.Reflection.FieldInfo)member;
                result = field.GetValue(obj);
            }
            return result;
        }

        private static bool CanWrite(System.Reflection.MemberInfo member)
        {
            return IsProperty(member) ? ((System.Reflection.PropertyInfo)member).CanWrite : IsField(member);
        }

        private static bool CanRead(System.Reflection.MemberInfo member)
        {
            return IsProperty(member) ? ((System.Reflection.PropertyInfo)member).CanRead : IsField(member);
        }

        private static bool IsProperty(System.Reflection.MemberInfo member)
        {
            return IsType(member.GetType(), typeof(System.Reflection.PropertyInfo));
        }

        private static bool IsField(System.Reflection.MemberInfo member)
        {
            return IsType(member.GetType(), typeof(System.Reflection.FieldInfo));
        }

        private static bool IsType(System.Type type, System.Type targetType)
        {
            return type.Equals(targetType) || type.IsSubclassOf(targetType);
        }

        private static List<System.Reflection.MemberInfo> GetMembers(System.Type type)
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.NonPublic;
            var members = new List<System.Reflection.MemberInfo>();
            members.AddRange(type.GetProperties(flags));
            members.AddRange(type.GetFields(flags));
            return members;
        }
    }
}
