using Reqnroll;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    public sealed partial class ReqnrollPluginTestRunner : ITestRunner
    {
        public async Task AndAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _testRunner?.AndAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task ButAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _testRunner?.ButAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task GivenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _testRunner?.GivenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task OnFeatureEndAsync()
        {
            await _testRunner?.OnFeatureEndAsync();
        }

        public async Task OnFeatureStartAwait(FeatureInfo featureInfo)
        {
            await _testRunner?.OnFeatureStartAsync(featureInfo);
        }

        public async Task OnScenarioEndAsync()
        {
            await _testRunner?.OnScenarioEndAsync();
        }

        public async Task OnTestRunEndAsync()
        {
            await _testRunner?.OnTestRunEndAsync();
        }

        public async Task OnTestRunStartAsync()
        {
            await _testRunner?.OnTestRunStartAsync();
        }

        public void Pending()
        {
            _testRunner?.Pending();
        }

        public async Task ThenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _testRunner?.ThenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task WhenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _testRunner?.WhenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void OnScenarioInitialize(ScenarioInfo scenarioInfo)
        {
            _testRunner?.OnScenarioInitialize(scenarioInfo);
        }

        public async Task OnScenarioStartAsync()
        {
            var paramTransformer = ScenarioContext?.GetBindingInstance(typeof(IParameterTransformer)) as IParameterTransformer;
            paramTransformer?.ClearBespokeTransformers();
            await _testRunner?.OnScenarioStartAsync();
        }

        public void SkipScenario()
        {
            _testRunner?.SkipScenario();
        }

        public async Task OnFeatureStartAsync(FeatureInfo featureInfo)
        {
            await _testRunner?.OnFeatureStartAsync(featureInfo);
        }

        public async Task CollectScenarioErrorsAsync()
        {
            await _testRunner?.CollectScenarioErrorsAsync();
        }
    }
}
