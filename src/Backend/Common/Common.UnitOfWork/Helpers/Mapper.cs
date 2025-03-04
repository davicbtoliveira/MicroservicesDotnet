using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Common.UnitOfWork.Helpers
{
    public class DataReaderMapper<T>
    {
        Dictionary<int, Either<FieldInfo, PropertyInfo>> mappings;
        bool IsPrimitiveish;
        public DataReaderMapper(IDataReader reader)
        {
            Type U = Nullable.GetUnderlyingType(typeof(T));
            this.IsPrimitiveish = (
                typeof(T).IsPrimitive ||
                (U != null && U.IsPrimitive)
            );
            if (!this.IsPrimitiveish)
            {
                this.mappings = Mappings(reader);
            }
        }

        public class MapMismatchException : Exception
        {
            public MapMismatchException(string arg) : base(arg) { }
        }

        private class JoinInfo
        {
            public Either<FieldInfo, PropertyInfo> info;
            public String name;
        }

        static Dictionary<int, Either<FieldInfo, PropertyInfo>> Mappings(IDataReader reader)
        {
            var columns = Enumerable.Range(0, reader.FieldCount);
            var fieldsAndProps = typeof(T).FieldsAndProps()
                .Select(fp => fp.Match(
                    f => new JoinInfo { info = f, name = f.Name },
                    p => new JoinInfo { info = p, name = p.Name }
                ));
            var joined = columns
                  .Join(fieldsAndProps, reader.GetName, x => x.name, (index, x) => new
                  {
                      index,
                      x.info
                  }, StringComparer.InvariantCultureIgnoreCase).ToList();
            if (columns.Count() != joined.Count() || fieldsAndProps.Count() != joined.Count())
            {
                throw new MapMismatchException($"Espera-se mapear todas as colunas no resultado. " +
                    $"Em vez de, {columns.Count()} colunas e {fieldsAndProps.Count()} " +
                    $"Sugestão: Certifique-se de que todas as colunas tenham _nomes_ e os nomes se correspondam.");
            }
            return joined
                 .ToDictionary(x => x.index, x => x.info);
        }

        public T MapFrom(IDataRecord record)
        {
            if (IsPrimitiveish)
            {
                return (T)record.GetValue(0);
            }
            var element = Activator.CreateInstance<T>();
            foreach (var map in mappings)
                map.Value.Match(
                    f => f.SetValue(element, ChangeType(record[map.Key], f.FieldType)),
                    p => p.SetValue(element, ChangeType(record[map.Key], p.PropertyType))
                );

            return element;
        }

        static object ChangeType(object value, Type targetType)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType);
        }
    }

    public static class FieldAndPropsExtension
    {
        public static IEnumerable<Either<FieldInfo, PropertyInfo>> FieldsAndProps(this Type T)
        {
            var lst = new List<Either<FieldInfo, PropertyInfo>>();
            lst.AddRange(
                T.GetFields()
                .Select(field => new Either<FieldInfo, PropertyInfo>.Left(field))
            );
            lst.AddRange(
                T.GetProperties()
                .Select(prop => new Either<FieldInfo, PropertyInfo>.Right(prop))
            );
            return lst;
        }
    }
}
