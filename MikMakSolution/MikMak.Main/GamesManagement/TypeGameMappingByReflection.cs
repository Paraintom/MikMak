using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using MikMak.Main.InternalInterfaces;
using MikMak.Interfaces;
using MikMak.Configuration;

namespace MikMak.Main.GamesManagement
{
    /// <summary>
    /// Implementation of the interface ITypeGameMapping using reflection
    /// </summary>
    internal class TypeGameMappingByReflection : ITypeGameMapping
    {
        /// <summary>
        /// The path where are stocked the assemblies of the games
        /// </summary>
        private string path = MyConfiguration.GetString("gamesFolder", "./Games/");

        /// <summary>
        /// Internal representation of the data
        /// </summary>
        private Dictionary<int, IGameServices> datas = new Dictionary<int, IGameServices>();

        /// <summary>
        /// No comment
        /// </summary>
        public TypeGameMappingByReflection()
        {
            Initialize();
        }

        /// <summary>
        /// See Interface
        /// </summary>
        /// <returns>See Interface</returns>
        public Dictionary<int, IGameServices> GetAllMappings()
        {
            var exposedDictionary = datas.ToDictionary(entry => entry.Key, entry => entry.Value);
            return exposedDictionary;
        }

        /// <summary>
        /// See Interface
        /// Exceptions :
        ///   System.Collections.Generic.KeyNotFoundException
        /// </summary>
        /// <param name="typeGame">See Interface</param>
        /// <returns>See Interface</returns>
        public IGameServices GetGame(int typeGame)
        {
            return datas[typeGame];
        }

        private void Initialize()
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("The folder in config does not exist");
            }

            List<string> allAssembliesToLoad = GetAllFilesToLoad(path);

            foreach (string assemblyPath in allAssembliesToLoad)
            {
                //var assembly = Assembly.LoadFrom(assemblyPath);
                Assembly assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyPath));
                Type typeMatch = null;
                foreach (Type currentType in assembly.GetTypes())
                {
                    if (currentType.GetInterface(typeof(IGameServices).FullName, false) != null)
                    {
                        if (typeMatch != null)
                        {
                            throw new Exception(string.Format("The assembly [{0}] contains more than two type implementing IGameInterface"));
                        }
                        else
                        {
                            typeMatch = currentType;
                        }
                    }
                }
                if (typeMatch == null)
                {
                    //No type found?
                }
                else
                {
                    // Instance dynamique à  partir du type donné
                    object objInstanceDynamique = System.Activator.CreateInstance(typeMatch);
                    var myGame = objInstanceDynamique as IGameServices;
                    if (myGame != null)
                    {
                        if (!datas.ContainsKey(myGame.GetGameType()))
                        {
                            datas.Add(myGame.GetGameType(), myGame);
                        }
                        else
                        {
                            //already added?
                        }
                    }
                }
            }
        }

        private List<string> GetAllFilesToLoad(string dirPath)
        {
            List<string> result = new List<string>();
            try
            {
                var files = Directory.EnumerateFiles(dirPath, "*.dll", SearchOption.TopDirectoryOnly);

                foreach (var f in files)
                {
                    // Add log : Console.WriteLine("{0}\t{1}", f);
                    result.Add(f);
                }
                // Add log : Console.WriteLine("{0} files found.", files.Count().ToString());
            }
            catch (UnauthorizedAccessException UnauthoEx)
            {
                // Add log :Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                // Add log :Console.WriteLine(PathEx.Message);
            }
            return result;
        }
    }
}
