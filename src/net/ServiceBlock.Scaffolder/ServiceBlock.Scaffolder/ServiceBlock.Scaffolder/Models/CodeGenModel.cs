using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBlock.Scaffolder.Models
{
    [Serializable]
    public class CodeGenModel : ModelBase
    {
        public CodeGenModel(string entityName, ModelContext context) : base(entityName, context) { }
        public string UoWInterfaceName { get { return base.GetExtendedPropertyOrDefault("Global.UoWInterfaceName"); } }
        public string DataAssemblyName { get { return base.GetExtendedPropertyOrDefault("Global.DataAssemblyName"); } }
        public string DomainAssemblyName { get { return base.GetExtendedPropertyOrDefault("Global.DomainAssemblyName"); } }
        public string WorkflowAssemblyName { get { return base.GetExtendedPropertyOrDefault("Global.WorkflowAssemblyName"); } }
        public string IncludeProperties { get { return String.Join(",", base.NavigationProperties.Select(x => x.Name)); } }
        public string ConnectionName { get { return base.GetExtendedPropertyOrDefault("Global.ConnectionStringName"); } }
        public string UoWClassName { get { return base.GetExtendedPropertyOrDefault("Global.UoWClassName"); } }
        public string OrchestrationNamespace { get { return base.GetExtendedPropertyOrDefault("Global.WorkflowNamespace"); } }        
        public string ModelNamespace { get { return base.GetExtendedPropertyOrDefault("Global.ModelNamespace"); } }
        public string DataActivityNamespace { get { return base.GetExtendedPropertyOrDefault("Global.DataNamespace"); } }
        public string DataNamespace { get { return base.GetExtendedPropertyOrDefault("Global.DataNamespace"); } }
        public string MessagesNamespace { get { return base.GetExtendedPropertyOrDefault("Global.MessagesNamespace"); } }
        public string WebApiControllerNamespace { get { return base.GetExtendedPropertyOrDefault("Global.WebApiControllerNamespace"); } }
        public string WebUIControllerNamespace { get { return base.GetExtendedPropertyOrDefault("Global.WebUIControllerNamespace"); } }

        public Dictionary<string, string> DomainClasses
        {
            get
            {
                var toReturn = new Dictionary<string, string>();
                var entities = base.Context.ExtendedProperties["Global.EntityNames"] as IList<string>;

                if (entities != null)
                {
                    foreach (var entity in entities)
                        toReturn[entity] = Pluralizer.Pluralize(entity);
                }

                return toReturn;
            }
        }



    }
}
