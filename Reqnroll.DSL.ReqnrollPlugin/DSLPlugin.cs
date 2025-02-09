using Reqnroll.Infrastructure;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;

[assembly: RuntimePlugin(typeof(Reqnroll.DSL.DSLPlugin))]

namespace Reqnroll.DSL
{
    public class DSLPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<DSLTestRunner, ITestRunner>();
                args.ObjectContainer.RegisterTypeAs<ParameterTransform, IParameterTransform>();
            };
        }
    }
}