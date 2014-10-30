using Microsoft.AspNet.Scaffolding;
using ServiceBlock.Scaffolder;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ServiceBlock.Scaffolder
{
    [Export(typeof(CodeGeneratorFactory))]
    public class ServiceBlockCodeGenFactory : CodeGeneratorFactory
    {
        /// <summary>
        ///  Information about the code generator goes here.
        /// </summary>
        private static CodeGeneratorInformation _info = new CodeGeneratorInformation(
            displayName: "ServiceBlock Scaffolder",
            description: "Generates multi-tier solution for a given domain model based on ServiceBlock.",
            author: "Muhammad Haroon",
            version: new Version(1, 0, 0, 0),
            id: typeof(ServiceBlockCodeGenarator).Name,
            icon: ToImageSource(Resources._TemplateIconSample),
            gestures: new[] { "Controller", "View", "Area" },
            categories: new[] { Categories.Common, Categories.MvcController, Categories.Other });

        public ServiceBlockCodeGenFactory()
            : base(_info)
        {
        }
        /// <summary>
        /// This method creates the code generator instance.
        /// </summary>
        /// <param name="context">The context has details on current active project, project item selected, Nuget packages that are applicable and service provider.</param>
        /// <returns>Instance of CodeGenerator.</returns>
        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new ServiceBlockCodeGenarator(context, Information);            
        }

        /// <summary>
        /// Provides a way to check if the custom scaffolder is valid under this context
        /// </summary>
        /// <param name="codeGenerationContext">The code generation context</param>
        /// <returns>True if valid, False otherwise</returns>
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            var references = GetProjectReferences(codeGenerationContext.ActiveProject);

            // Must be a C# project and 
            // must reference System.Web.Http, which is generally only referenced in WebApi projects

            return (
                codeGenerationContext.ActiveProject.CodeModel.Language == EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp && 
                references.Where(x => x.Name == "System.Web.Http").Count() == 1);
        }

        private static IList<AssemblyName> GetProjectReferences(EnvDTE.Project project)
        {
            var toReturn = new List<AssemblyName>();
            var vsproject = project.Object as VSLangProj.VSProject;
            // note: you could also try casting to VsWebSite.VSWebSite

            foreach (VSLangProj.Reference reference in vsproject.References)
            {
                if (reference.SourceProject == null)
                {
                    // This is an assembly reference
                    var fullName = GetFullName(reference);
                    var assemblyName = new AssemblyName(fullName);
                    toReturn.Add(assemblyName);
                }
            }

            return toReturn;
        }

        private static string GetFullName(VSLangProj.Reference reference)
        {
            return string.Format("{0}, Version={1}.{2}.{3}.{4}, Culture={5}, PublicKeyToken={6}",
                                    reference.Name,
                                    reference.MajorVersion, reference.MinorVersion, reference.BuildNumber, reference.RevisionNumber,
                                    DefaultIfNull(reference.Culture, "neutral"),
                                    DefaultIfNull(reference.PublicKeyToken, "null"));
        }

        private static string DefaultIfNull(string text, string alternative)
        {
            return string.IsNullOrWhiteSpace(text) ? alternative : text;
        }

        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
