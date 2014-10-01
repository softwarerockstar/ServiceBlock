using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHA.ServiceFoundation
{
    public static class DbDataReaderExtensions
    {
        public static IList<T> To<T>(this DbDataReader reader)
        {
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, T>();
                return AutoMapper.Mapper.Map<List<T>>(reader);
            }
            catch
            {
                return null;
            }
        }
    }
}
