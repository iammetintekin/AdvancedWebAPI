using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.Entity.Models;
using Project.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        // you must new object
        public PropertyInfo[] Properties { get; set; }
        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        /// <summary>
        /// Shapping for list
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="fieldString"></param>
        /// <returns></returns>
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldString)
        {
            var requiredPRoperties = GetRequiredPropertyInfos(fieldString);
            return FetchData(entities, requiredPRoperties);
        }
        /// <summary>
        /// Shapping for entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldString"></param>
        /// <returns></returns>
        public ShapedEntity ShapeData(T entity, string fieldString)
        {
            var requiredPRoperties = GetRequiredPropertyInfos(fieldString);
            return FetchDataForEntity(entity, requiredPRoperties);
        }
        /// <summary>
        /// returns the property list for entity that user want
        /// </summary>
        /// <param name="fieldString">Prop names must be seperated by comma</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetRequiredPropertyInfos(string fieldString)
        {
            var requiredFields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldString))
            {
                var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach ( var field in fields)
                {
                    var prop = Properties.FirstOrDefault(pi=>pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (prop is null)
                        continue;
                    requiredFields.Add(prop);
                }
            }
            else
            {
                requiredFields = Properties.ToList();
            }
            return requiredFields;
        }
        /// <summary>
        /// Gönderilen propertilere göre nesneyi şekillendirir.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="requiredProperties"></param>
        /// <returns></returns>
        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ShapedEntity();
            foreach ( var requiredProperty in requiredProperties)
            {
                var objectPropValue = requiredProperty.GetValue(entity);
                shapedObject.Entity.TryAdd(requiredProperty.Name, objectPropValue);
            }
            var objectProperty = entity.GetType().GetProperty("Id");
            shapedObject.Id = (int)objectProperty.GetValue(entity);
            return shapedObject;
        }
        /// <summary>
        /// Göndeirlen propertilere göre listedeki nesneleri şekillendirir.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="requiredProperties"></param>
        /// <returns></returns>
        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObjectList = new List<ShapedEntity>();
            foreach (var requiredProperty in entities)
            {
                var objectPropValue = FetchDataForEntity(requiredProperty, requiredProperties);
                shapedObjectList.Add(objectPropValue);
            }
            return shapedObjectList;
        }
    }
}
