using DSL.ReqnrollPlugin;
using Reqnroll;
using Reqnroll.Plugins;
using Reqnroll.UnitTestProvider;

[assembly: RuntimePlugin(typeof(DSL.ReqnrollPlugin.ReqnrollPlugin))]

namespace DSL.ReqnrollPlugin
{
    public sealed class ReqnrollPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters, UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<ReqnrollPluginTestRunner, ITestRunner>();
                args.ObjectContainer.RegisterTypeAs<CustomVariablesParameterTransformer, IParameterTransformer>();
            };
        }
    }
}