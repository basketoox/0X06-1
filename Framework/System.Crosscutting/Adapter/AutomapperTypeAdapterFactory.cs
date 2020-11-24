using System.Linq;
using AutoMapper;

namespace System.Crosscutting.Adapter
{
    public class AutomapperTypeAdapterFactory
        : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        ///     Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType == typeof(Profile));

            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.Configuration.MapperConfigurationExpression" &&
                        item.FullName != "AutoMapper.Configuration.MapperConfigurationExpression+NamedProfile")
                    {
                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                    }
                }
            });
        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}