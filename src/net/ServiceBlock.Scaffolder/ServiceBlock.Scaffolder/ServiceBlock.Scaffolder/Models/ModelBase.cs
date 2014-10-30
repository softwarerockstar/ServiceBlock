using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder.Models
{
    [Serializable]
    public abstract class ModelBase
    {
        private readonly ModelContext _context;

        public ModelBase(string entityName, ModelContext context)
        {
            this.EntityName = entityName;
            _context = context;
        }

        public ModelContext Context { get { return _context; } }

        public string GetExtendedPropertyOrDefault(string propertyName, string defaultValue = "")
        {
            return (_context.ExtendedProperties[propertyName] ?? defaultValue).ToString();
        }

        public IList<PropertyInfo> Properties { get { return _context.Properties; } }

        public IList<PropertyInfo> PrimaryKeyProperties
        {
            get { return _context.Properties.Where(x => x.IsPrimaryKey).ToList(); }
        }

        public IList<PropertyInfo> ForeignKeyProperties
        {
            get { return _context.Properties.Where(x => x.IsForeignKey).ToList(); }
        }

        public IList<PropertyInfo> NonKeyProperties 
        { 
            get { return _context.Properties.Where(x => x.IsNonKey).ToList(); } 
        }

        public IList<PropertyInfo> NavigationProperties
        {
            get { return _context.Properties.Where(x => x.IsNavigation).ToList(); }
        }


        public virtual string EntityName { get; set; }

        public string EntityNamePlural
        {
            get
            {
                return Pluralizer.Pluralize(this.EntityName);
            }
        }
        
        public string GetClassFileFolder(string targetNamespace)
        {
            var s = targetNamespace.Split('.');
            return String.Join(@"\", s, 1, s.Length - 1);
        }


        public string GetClassFileFolderWithEntityName(string targetNamespace)
        {
            return GetClassFileFolder(targetNamespace) + @"\" + this.EntityName;
        }

        public string GetClassFilePathWithEntityName(string target, string targetNamespace)
        {
            var path = GetClassFileFolderWithEntityName(targetNamespace);
            return path + @"\" + this.EntityName + target;
        }

        public string GetClassFilePath(string target, string targetNamespace)
        {
            return GetClassFileFolder(targetNamespace) + @"\" + target;
        }

    }
}
