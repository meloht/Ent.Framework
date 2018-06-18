using Ent.Framework.Ioc.Configuration.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Ent.Framework.Ioc.Configuration
{
    /// <summary>
    /// packing配置节.
    /// </summary>
    public class PackingSection : ConfigurationSection
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        public const string SectionName = "packing";
        public const string configFileName = "Unity.config";

        static PackingSection()
        {

            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFileName
            };

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            Content = configuration.GetSection(SectionName) as PackingSection;
        }
        /// <summary>
        /// 读取配置节内容
        /// </summary>
        public static readonly PackingSection Content;


        /// <summary>
        /// 配置集合名称
        /// </summary>
        private const string ModulesAssembliesProperty = "moduleAssemblies";

        /// <summary>
        /// 指定要加载的模块
        /// </summary>
        [ConfigurationProperty(ModulesAssembliesProperty, IsRequired = true)]
        public NamedElementCollection<ModuleDefinition> ModuleAssemblies
        {
            get
            {
                return (NamedElementCollection<ModuleDefinition>)base[ModulesAssembliesProperty];
            }
        }



    }
}
