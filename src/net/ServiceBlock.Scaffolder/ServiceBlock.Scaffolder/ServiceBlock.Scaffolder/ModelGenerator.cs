using BasicScaffolder1.Models;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicScaffolder1
{
    public class ModelGenerator
    {
        private CodeGenerationContext _context;
        public ModelGenerator(CodeGenerationContext context)
        {
            _context = context;
        }

        public IList<MessageModel> GetMessagesModels()
        {
            var models = new List<MessageModel>
            {
                new MessageModel {EntityName = "Category"},
                new MessageModel {EntityName = "Product"}
            };

            return models;
        }

        public IList<UoWModel> GetUowModels()
        {
            var models = new List<UoWModel>
            {
                new UoWModel()
            };

            return models;
        }

        public IList<DataActivityModel> GetDataActivityModels()
        {
            var models = new List<DataActivityModel>
            {
                new DataActivityModel {EntityName = "Category", NonKeyProperties = new List<PropertyInfo> { new PropertyInfo {Name = "Name"} }},
                new DataActivityModel {EntityName = "Product", NonKeyProperties = new List<PropertyInfo> { new PropertyInfo {Name = "Name"} }}
            };


            return models;
        }

        public IList<OrchestrationModel> GetOrchestrationModels()
        {
            var models = new List<OrchestrationModel>
            {
                new OrchestrationModel {EntityName = "Category"},
                new OrchestrationModel {EntityName = "Product",}
            };


            return models;
        }

        public IList<WebApiModel> GetWebApiModels()
        {
            var models = new List<WebApiModel>
            {
                new WebApiModel {EntityName = "Category"},
                new WebApiModel {EntityName = "Product",}
            };


            return models;
        }
        public IList<WebUIControllerModel> GetWebUIControllerModels()
        {
            var models = new List<WebUIControllerModel>
            {
                new WebUIControllerModel {EntityName = "Category"},
                new WebUIControllerModel {EntityName = "Product",}
            };


            return models;
        }

        public IList<WebUIViewModel> GetWebUIViewModels()
        {
            var models = new List<WebUIViewModel>
            {
                new WebUIViewModel {EntityName = "Category"},
                new WebUIViewModel {EntityName = "Product",}
            };


            return models;
        }

    }
}
