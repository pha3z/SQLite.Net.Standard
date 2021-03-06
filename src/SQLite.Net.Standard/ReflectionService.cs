﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SQLite.Net.Interop;

namespace SQLite.Net
{
    public class ReflectionService : IReflectionService
    {
        public IEnumerable<PropertyInfo> GetPublicInstanceProperties(Type mappedType)
        {
            return from p in mappedType.GetRuntimeProperties()
                   where
                       ((p.GetMethod != null && p.GetMethod.IsPublic) || (p.SetMethod != null && p.SetMethod.IsPublic) ||
                        (p.GetMethod != null && p.GetMethod.IsStatic) || (p.SetMethod != null && p.SetMethod.IsStatic))
                   select p;
        }

        public object GetMemberValue(object obj, Expression expr, MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (member is PropertyInfo m)
            {
                return m.GetValue(obj, null);
            }
            if (member is FieldInfo m2)
            {
                return m2.GetValue(obj);
            }
            throw new NotSupportedException("MemberExpr: " + member.DeclaringType);
        }
    }
}