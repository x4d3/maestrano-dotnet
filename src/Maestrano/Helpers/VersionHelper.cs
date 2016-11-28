using System;
using System.Reflection;

namespace Maestrano.Helpers
{
    class VersionHelper
    {
        public static Version GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(assembly.FullName);
            return assemblyName.Version;
        }
    }
}
