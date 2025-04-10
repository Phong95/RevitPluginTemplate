using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace RevitPlugin2025.Helper.AssemblyLoader
{
    public class AssemblyLoader : IDisposable
    {
        private static string _executingPath;
        public AssemblyName _assemblyName;

        private DirectoryInfo _fileInfo;
        private IEnumerable<string> _assemblies;

        readonly AppDomain currentDomain = AppDomain.CurrentDomain;

        private IEnumerable<Assembly> _domainAssemblies;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssemblyLoader()
        {
            // Store the loaded assemblies
            _executingPath = Assembly.GetExecutingAssembly().Location;
            _fileInfo = new FileInfo(_executingPath).Directory;

            // Return a list of all DLLS from the executing directory
            _assemblies = EnumerateFiles.GetFiles(_fileInfo);
            _domainAssemblies = currentDomain.GetAssemblies().Where(a => !a.IsDynamic);

            AppDomain.CurrentDomain.AssemblyResolve += LoadApplicationAssemblies;
        }

        /// <summary>
        /// Event handler to return an assembly
        /// </summary>
        /// <param name="sender">The object that raises the event</param>
        /// <param name="args">Supplementary parameters</param>
        /// <returns>The name of the a assembly</returns>
        public Assembly LoadApplicationAssemblies(object sender, ResolveEventArgs args)
        {
            if (string.IsNullOrEmpty(_executingPath))
                return null;

            try
            {
                // Ignore missing resources
                if (args.Name.Contains(".resources"))
                    return null;

                // Check for assemblies already loaded
                Assembly loadedAssembly = _domainAssemblies.FirstOrDefault(x => x.FullName == args.Name);
                if (loadedAssembly != null)
                    return loadedAssembly;

                var requestedAssembly = args.Name.Split(',')[0];
                var assemblyToLoad = _assemblies.Where(x => x.Contains(requestedAssembly)).FirstOrDefault();

                return Assembly.LoadFrom(assemblyToLoad);
            }

            catch (Exception e)
            {
                // or rethrow the exception
                // throw;
                return null;
            }
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= LoadApplicationAssemblies;
        }
    }
    class EnumerateFiles
    {
        /// <summary>
        /// Return the list of files in fileInfo
        /// </summary>
        /// <param name="fileInfo">The directory</param>
        /// <returns>A read-only enumerable (list) that contains a list of files in use</returns>
        public static IEnumerable<string> GetFiles(DirectoryInfo fileInfo)
        {
            return from file in fileInfo.EnumerateFiles()
                   where (file.Name.EndsWith(".dll") || file.Name.EndsWith(".exe"))
                   select file.FullName;
        }
    }
}
