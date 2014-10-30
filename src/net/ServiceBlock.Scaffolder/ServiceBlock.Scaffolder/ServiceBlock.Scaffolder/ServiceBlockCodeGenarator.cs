using ServiceBlock.Scaffolder.UI;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Linq;
using System.Collections.Generic;
using ServiceBlock.Scaffolder.Models;
using EnvDTE;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using EnvDTE80;

namespace ServiceBlock.Scaffolder
{
    public class ServiceBlockCodeGenarator : CodeGenerator
    {
        private readonly CustomViewModel _viewModel;
        private IEnumerable<string> _templateFolders;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public ServiceBlockCodeGenarator(
            CodeGenerationContext context,
            CodeGeneratorInformation information)
            : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }
        

        /// <summary>
        /// Any UI to be displayed after the scaffolder has been selected from the Add Scaffold dialog.
        /// Any validation on the input for values in the UI should be completed before returning from this method.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            // Bring up the selection dialog and allow user to select a model type
            //SelectModelWindow window = new SelectModelWindow(_viewModel);
            //bool? showDialog = window.ShowDialog();
            //return showDialog ?? false;
            return true;
        }

        public override IEnumerable<string> TemplateFolders
        {
            get { return _templateFolders; }
        }


        public override void GenerateCode()
        {
            using (var ot = new OverridableTemplates(
                        originalTemplateFolder: base.TemplateFolders.ToList()[0],
                        solutionFolder: Path.GetDirectoryName(Context.ActiveProject.DTE.Solution.FullName)))
            {
                _templateFolders = ot.TemplateFolders;

                var extendedProperties = new Dictionary<string, object>
                {
                    {"Global.UoWInterfaceName", "IUnitOfWorkEx"},
                    {"Global.UoWClassName", "DefaultUnitOfWork"},
                    {"Global.ConnectionStringName", "DefaultEntityFrameworkDataContext"},
                    {"Global.DataNamespace", "DataActivities.Activities"},
                    {"Global.DataAssemblyName", "DataActivities"},
                    {"Global.DomainAssemblyName", "Domain"},
                    {"Global.MessagesNamespace", "Domain.Messages"},
                    {"Global.ModelNamespace", "Domain.Models"},
                    {"Global.WorkflowAssemblyName", "Orchestrations"},
                    {"Global.WorkflowNamespace", "Orchestrations.Workflows"},
                    {"Global.WebApiControllerNamespace", "WebApi.Controllers"},
                    {"Global.WebUIControllerNamespace", "WebUI.Controllers"},                
                    {"Global.DomainProjectName", "Domain"},
                    {"Global.DataProjectName", "DataActivities"},
                    {"Global.WorkflowProjectName", "Orchestrations"},
                    {"Global.ServiceProjectName", "WebApi"},
                    {"Global.WebUIProjectName", "WebUI"}
                };

                var metadata = GetCodeMetadata(
                    extendedProperties["Global.ModelNamespace"].ToString(),
                    extendedProperties["Global.DomainProjectName"].ToString());

                extendedProperties["Global.EntityNames"] = metadata.Select(x => x.Key).ToList();
                extendedProperties["Global.WebApiBaseUri"] = GetServiceProjectBaseUri(
                    extendedProperties["Global.ServiceProjectName"].ToString());

                var context = new ModelContext { ExtendedProperties = extendedProperties };
                GenerateUoW(new CodeGenModel(extendedProperties["Global.UoWClassName"].ToString(), context));
                GenerateWebUIMenuLinks(new CodeGenModel(extendedProperties["Global.UoWClassName"].ToString(), context));
                GenerateWebUIConfig(new CodeGenModel(extendedProperties["Global.UoWClassName"].ToString(), context));

                foreach (var entityName in metadata.Keys)
                {
                    var ctx = new ModelContext { Properties = metadata[entityName], ExtendedProperties = extendedProperties };

                    GenerateWebUIViews(new CodeGenModel(entityName, ctx));
                    GenerateWebUIControllers(new CodeGenModel(entityName, ctx));
                    GenerateWebApiControllers(new CodeGenModel(entityName, ctx));
                    GenerateOrchestrations(new CodeGenModel(entityName, ctx));
                    GenerateDataActivities(new CodeGenModel(entityName, ctx));
                    GenerateDomainMessages(new CodeGenModel(entityName, ctx));
                }
            }
        }

        private string GetServiceProjectBaseUri(string serviceProjectName)
        {
            var project = FindProjectByName(serviceProjectName);
            var uri =  project.Properties.Item("WebApplication.IISUrl").Value + "/api/";
            return (uri == null) ? "http://localhost:9999/api/" : uri.ToString().Replace("//api/", "/api/");
        }

        private string FindConfigurationFilename(Project project)
        {
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
                    return item.get_FileNames(0);
            }

            return null;
        }



        private void GenerateUoW(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.DomainProjectName"));

            foreach (var template in Templates.UoWTemplates)
            {
                
                
                this.AddFileFromTemplate(
                    project,
                    model.EntityName,
                    @"Domain\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }

        private void GenerateDataActivities(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.DataProjectName"));
            
            foreach (var template in Templates.DataActivityTemplates)
            {
                var folder = model.GetClassFileFolderWithEntityName(model.DataActivityNamespace);

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    model.GetClassFileFolderWithEntityName(model.DataActivityNamespace) + @"\" + template,
                    @"DataActivities\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }


        private void GenerateDomainMessages(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.DomainProjectName"));

            foreach (var template in Templates.MessageTemplates)
            {
                var folder = model.GetClassFileFolderWithEntityName(model.MessagesNamespace);

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    model.GetClassFilePathWithEntityName(template, model.MessagesNamespace),
                    @"Domain\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }

        private void GenerateOrchestrations(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.WorkflowProjectName"));

            foreach (var template in Templates.OrchestrationTemplates)
            {
                var folder = model.GetClassFileFolderWithEntityName(model.OrchestrationNamespace);

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    model.GetClassFileFolderWithEntityName(model.OrchestrationNamespace) + @"\" + template,
                    @"Orchestrations\" + template,
                    parameters,
                    skipIfExists: false);


            }
        }

        private void GenerateWebApiControllers(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {                    
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.ServiceProjectName"));

            foreach (var template in Templates.WebApiTemplates)
            {
                var folder = model.GetClassFileFolder(model.WebApiControllerNamespace);

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    model.GetClassFilePath(model.EntityName + template, model.WebApiControllerNamespace),
                    @"WebApi\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }

        private void GenerateWebUIConfig(CodeGenModel model)
        {
            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.WebUIProjectName"));

            string configurationFilename = FindConfigurationFilename(project);

            var configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = configurationFilename;
            
            var configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(
                configFile, 
                ConfigurationUserLevel.None);

            configuration.AppSettings.Settings.Remove("serviceBlock:restEndpointUri");
            configuration.AppSettings.Settings.Add("serviceBlock:restEndpointUri", model.GetExtendedPropertyOrDefault("Global.WebApiBaseUri"));
            
            configuration.Save();
        }

        private void GenerateWebUIControllers(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };
            
            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.WebUIProjectName"));

            foreach (var template in Templates.WebUIControllerTemplates)
            {
                var folder = model.GetClassFileFolder(model.WebUIControllerNamespace);

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    model.GetClassFilePath(model.EntityName + template, model.WebUIControllerNamespace),
                    @"WebUI\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }

        private void GenerateWebUIViews(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.WebUIProjectName"));

            foreach (var template in Templates.WebUIViewTemplates)
            {
                var folder = @"Views\" + model.EntityName;

                this.AddFolder(project, folder);

                this.AddFileFromTemplate(
                    project,
                    folder + @"\Index",
                    @"WebUI\" + template,
                    parameters,
                    skipIfExists: false);
            }
        }

        private void GenerateWebUIMenuLinks(CodeGenModel model)
        {
            var parameters = new Dictionary<string, object>
                {
                    { "Model", model }
                };

            var project = FindProjectByName(model.GetExtendedPropertyOrDefault("Global.WebUIProjectName"));

            var folder = @"Views\MenuLinks";

            this.AddFolder(project, folder);

            this.AddFileFromTemplate(
                project,
                folder + @"\Index",
                @"WebUI\MenuLinks",
                parameters,
                skipIfExists: false);
        }

        private Dictionary<string, List<PropertyInfo>> GetCodeMetadata(string modelNamespace, string domainProjectName)
        {
            var domainProject = FindProjectByName(domainProjectName);

            var models = new Dictionary<string, List<PropertyInfo>>();

            var classes = FindClasses(domainProject, modelNamespace, null);

            foreach (var classs in classes)
            {
                var propertyList = new List<CodeProperty>();
                FindProperties(classs.Members, propertyList);

                var dataMemberProperties = FilterByAttribute(propertyList, typeof(DataMemberAttribute));

                var navigationProperties = FilterByAttribute(propertyList, typeof(ForeignKeyAttribute));

                var primaryKeyProperties = FilterByAttribute(dataMemberProperties, typeof(KeyAttribute));

                var foreignKeyProperties = dataMemberProperties.Where(x => x.Name.EndsWith("Id") && !primaryKeyProperties.Contains(x) && !navigationProperties.Contains(x)).ToList();

                var nonKeyProperties = dataMemberProperties.Where(x => !primaryKeyProperties.Contains(x) && !foreignKeyProperties.Contains(x) && !navigationProperties.Contains(x)).ToList();

                var props = new List<PropertyInfo>();

                foreach (var item in primaryKeyProperties)
                    props.Add(new PropertyInfo { Name = item.Name, TypeName = item.Type.AsString, IsPrimaryKey = true });

                foreach (var item in foreignKeyProperties)
                    props.Add(new PropertyInfo { Name = item.Name, TypeName = item.Type.AsString, IsForeignKey = true });

                foreach (var item in nonKeyProperties)
                    props.Add(new PropertyInfo { Name = item.Name, TypeName = item.Type.AsString, IsNonKey = true });

                foreach (var item in navigationProperties)
                    props.Add(new PropertyInfo { Name = item.Name, TypeName = item.Type.AsString, IsNavigation = true });

                models.Add(classs.Name, props);
            }

            return models;
        }

        private List<CodeProperty> FilterByAttribute(IList<CodeProperty> list, Type attributeType)
        {   
            var toReturn = new List<CodeProperty>();
            
            foreach (var element in list)
            {
                foreach (var attrib in element.Attributes)
                {
                    if ((attrib as CodeAttribute).FullName == attributeType.FullName)
                    {
                        toReturn.Add(element);
                        break;
                    }

                }
            }

            return toReturn;

        }

        private List<CodeClass> FindClasses(Project project, string nameSpace, string className)
        {
            List<CodeClass> result = new List<CodeClass>();
            FindClasses(project.CodeModel.CodeElements, className, nameSpace, result, false);
            return result;

        }

        private void FindClasses(CodeElements elements, string className, string searchNamespace, List<CodeClass> result, bool isNamespaceOk)
        {
            if (elements == null) return;
            foreach (CodeElement element in elements)
            {
                if (element is CodeNamespace)
                {
                    CodeNamespace ns = element as CodeNamespace;
                    if (ns != null)
                    {
                        if (ns.FullName.EndsWith(searchNamespace))
                            FindClasses(ns.Members, className, searchNamespace, result, true);
                        else
                            FindClasses(ns.Members, className, searchNamespace, result, false);
                    }
                }
                else if (element is CodeClass && isNamespaceOk)
                {
                    CodeClass c = element as CodeClass;
                    if (c != null)
                    {
                        if (className == null || c.FullName.Contains(className))
                            result.Add(c);

                        FindClasses(c.Members, className, searchNamespace, result, true);
                    }

                }
            }
        }

        private void FindProperties(CodeElements elements, IList<CodeProperty> properties)
        {
            foreach (CodeElement element in elements)
            {
                CodeProperty property = element as CodeProperty;
                if (property != null)
                {
                    properties.Add(property);
                }

                CodeElements children = null;

                try
                {
                    children = element.Children;
                }
                catch (Exception) { }

                if (children != null)
                    FindProperties(children, properties);
            }
        }


        private Project FindProjectByName(string projectName)
        {
            return GetAllProjects().Where(x => String.Compare(x.Name, projectName, true) == 0).FirstOrDefault();
            //foreach (Project project in Context.ActiveProject.DTE.Solution.Projects)
            //{
            //    if (String.Compare(project.Name, projectName, true) == 0)
            //        return project;
            //}

            //return null;
        }

        private IList<Project> GetAllProjects()
        {
            Projects projects = Context.ActiveProject.DTE.Solution.Projects;
            List<Project> list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        private IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }

        


    }
}
