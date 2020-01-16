using IPA.Loader;
using IPA.Old;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CustomWalls.Utilities
{
    public class Utils
    {
        /// <summary>
        /// Gets every file matching the filter in a path.
        /// </summary>
        /// <param name="path">Directory to search in.</param>
        /// <param name="filters">Pattern(s) to search for.</param>
        /// <param name="searchOption">Search options.</param>
        /// <param name="returnShortPath">Remove path from filepaths.</param>
        public static IEnumerable<string> GetFileNames(string path, IEnumerable<string> filters, SearchOption searchOption, bool returnShortPath = false)
        {
            IList<string> filePaths = new List<string>();

            foreach (string filter in filters)
            {
                IEnumerable<string> directoryFiles = Directory.GetFiles(path, filter, searchOption);

                if (returnShortPath)
                {
                    foreach (string directoryFile in directoryFiles)
                    {
                        string filePath = directoryFile.Replace(path, "");
                        if (filePath.Length > 0 && filePath.StartsWith(@"\"))
                        {
                            filePath = filePath.Substring(1, filePath.Length - 1);
                        }

                        if (!string.IsNullOrWhiteSpace(filePath) && !filePaths.Contains(filePath))
                        {
                            filePaths.Add(filePath);
                        }
                    }
                }
                else
                {
                    filePaths = filePaths.Union(directoryFiles).ToList();
                }
            }

            return filePaths.Distinct();
        }

        /// <summary>
        /// Loads an embedded resource from the calling assembly
        /// </summary>
        /// <param name="resourcePath">Path to resource</param>
        public static byte[] LoadFromResource(string resourcePath)
        {
            return GetResource(Assembly.GetCallingAssembly(), resourcePath);
        }

        /// <summary>
        /// Loads an embedded resource from an assembly
        /// </summary>
        /// <param name="assembly">Assembly to load from</param>
        /// <param name="resourcePath">Path to resource</param>
        public static byte[] GetResource(Assembly assembly, string resourcePath)
        {
            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;
        }

        public static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    return null;
                }

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static Texture2D defaultIcon = null;
        public static Texture2D GetDefaultIcon()
        {
            if (!defaultIcon)
            {
                try
                {
                    byte[] resource = LoadFromResource($"CustomWalls.Resources.default.png");
                    defaultIcon = LoadTextureRaw(resource);
                }
                catch { }
            }

            return defaultIcon;
        }

        private static Texture2D errorIcon = null;
        public static Texture2D GetErrorIcon()
        {
            if (!errorIcon)
            {
                byte[] resource = LoadFromResource($"CustomWalls.Resources.error.png");
                errorIcon = LoadTextureRaw(resource);
            }

            return errorIcon;
        }

        /// <summary>
        /// Loads an Texture2D from byte[]
        /// </summary>
        /// <param name="file"></param>
        public static Texture2D LoadTextureRaw(byte[] file)
        {
            if (file.Length > 0)
            {
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(file))
                {
                    return texture;
                }
            }

            return null;
        }

        /// <summary>
        /// Safely unescape \n and \t
        /// </summary>
        /// <param name="text"></param>
        public static string SafeUnescape(string text)
        {
            string unescapedString;

            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    unescapedString = string.Empty;
                }
                else
                {
                    // Unescape just some of the basic formatting characters
                    unescapedString = text;
                    unescapedString = unescapedString.Replace("\\n", "\n");
                    unescapedString = unescapedString.Replace("\\t", "\t");
                }
            }
            catch
            {
                unescapedString = text;
            }

            return unescapedString;
        }

        /// <summary>
        /// Check if a BSIPA plugin is enabled
        /// </summary>
        public static bool IsPluginEnabled(string PluginName)
        {
            if (IsPluginPresent(PluginName))
            {
                PluginLoader.PluginInfo pluginInfo = PluginManager.GetPluginFromId(PluginName);
                if (pluginInfo?.Metadata != null)
                {
                    return PluginManager.IsEnabled(pluginInfo.Metadata);
                }
            }

            return false;
        }

        /// <summary>
        /// Check if a plugin exists
        /// </summary>
        public static bool IsPluginPresent(string PluginName)
        {
            // Check in BSIPA
            if (PluginManager.GetPlugin(PluginName) != null ||
                PluginManager.GetPluginFromId(PluginName) != null)
            {
                return true;
            }

#pragma warning disable CS0618 // IPA is obsolete
            // Check in old IPA
            foreach (IPlugin plugin in PluginManager.Plugins)
            {
                if (plugin.Name == PluginName)
                {
                    return true;
                }
            }
#pragma warning restore CS0618 // IPA is obsolete

            return false;
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return string.Format($"{assembly.GetName().Name}.{resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".")}");
        }
    }
}
