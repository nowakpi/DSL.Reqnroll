using DSL.ReqnrollPlugin.Transformers;
using Reqnroll;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    public sealed partial class ReqnrollPluginTestRunner : ITestRunner
    {
        public async Task AndAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            if (_testRunner != null) await _testRunner.AndAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task ButAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            if (_testRunner != null) await _testRunner.ButAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task GivenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            if (_testRunner != null) await _testRunner.GivenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task OnFeatureEndAsync()
        {
            if (_testRunner != null) await _testRunner.OnFeatureEndAsync();
        }

        public async Task OnFeatureStartAwait(FeatureInfo featureInfo)
        {
            if (_testRunner != null) await _testRunner.OnFeatureStartAsync(featureInfo);
        }

        public async Task OnScenarioEndAsync()
        {
            if (_testRunner != null) await _testRunner.OnScenarioEndAsync();
        }

        public async Task OnTestRunEndAsync()
        {
            if (_testRunner != null) await _testRunner.OnTestRunEndAsync();
        }

        public async Task OnTestRunStartAsync()
        {
            if (_testRunner != null) await _testRunner.OnTestRunStartAsync();
        }

        public void Pending()
        {
            if (_testRunner != null) _testRunner.Pending();
        }

        public async Task ThenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            if (_testRunner != null) await _testRunner.ThenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public async Task WhenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            if (_testRunner != null) await _testRunner.WhenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

        public void OnScenarioInitialize(ScenarioInfo scenarioInfo)
        {
            if (_testRunner != null) _testRunner.OnScenarioInitialize(scenarioInfo);
        }

        public async Task OnScenarioStartAsync()
        {
            var paramTransformer = ScenarioContext?.GetBindingInstance(typeof(IUserVariableTransformer)) as IUserVariableTransformer;
            paramTransformer?.ClearBespokeTransformers();
            await _testRunner?.OnScenarioStartAsync();
        }

        public void SkipScenario()
        {
            if (_testRunner != null) _testRunner.SkipScenario();
        }

        public async Task OnFeatureStartAsync(FeatureInfo featureInfo)
        {
            if (_testRunner != null) await _testRunner.OnFeatureStartAsync(featureInfo);
        }

        public async Task CollectScenarioErrorsAsync()
        {
            if (_testRunner != null) await _testRunner.CollectScenarioErrorsAsync();
        }
    }
}
