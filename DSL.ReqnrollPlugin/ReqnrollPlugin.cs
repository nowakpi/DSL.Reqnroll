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
                args.ObjectContainer.RegisterTypeAs<UserVariableTransformer, IParameterTransformer>();
                args.ObjectContainer.RegisterTypeAs<EnvironmentVariableTransformer, IEnvironmentVariableTransformer>();
                args.ObjectContainer.RegisterTypeAs<FunctionParameterTransformer, IFunctionTransformer>();
                args.ObjectContainer.RegisterTypeAs<TransformerAggregator, ITransformerAggregator>();
                args.ObjectContainer.RegisterTypeAs<ReqnrollPluginTestRunner, ITestRunner>();
            };
        }
    }
}