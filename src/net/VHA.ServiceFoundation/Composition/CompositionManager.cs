using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace VHA.ServiceFoundation.Composition
{
    public class CompositionManager
    {
        private static readonly CompositionContainer _container;

        public static CompositionContainer Container { get { return _container; } }

        static CompositionManager()
        {
            _container = new CompositionContainer(new DirectoryCatalog(Path.Combine                                                 (AppDomain.CurrentDomain.BaseDirectory, "bin")));
        }
    }
}
