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
                args.ObjectContainer.RegisterTypeAs<CustomVariablesParameterTransformer, IParameterTransformer>(typeof(CustomVariablesParameterTransformer).FullName);
                args.ObjectContainer.RegisterTypeAs<EnvironmentVariablesParameterTransformer, IParameterTransformer>(typeof(EnvironmentVariablesParameterTransformer).FullName);
                args.ObjectContainer.RegisterTypeAs<ReqnrollPluginTestRunner, ITestRunner>();
            };
        }
    }
}