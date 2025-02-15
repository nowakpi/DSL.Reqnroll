using System;
using System.Threading.Tasks;
using Reqnroll;
using Reqnroll.Infrastructure;

namespace DSL.ReqnrollPlugin
{
    internal sealed class DSLTestRunner : ITestRunner
    {
        private readonly ITestRunner _testRunner;
        private readonly IParameterTransformer _transformer;
        private string _testWorkerId;

        public DSLTestRunner(ITestExecutionEngine executionEngine, IParameterTransformer tranformer)
        {
            _testRunner = new TestRunner(executionEngine);
            _transformer = tranformer;
        }

        [Obsolete]
        public void InitializeTestRunner(string testWorkerId)
        {
            _testWorkerId = testWorkerId;
            _testRunner?.InitializeTestRunner(testWorkerId);
        }

        public string Transform(in string obj)
        {
            return _transformer?.Transform(obj, ScenarioContext);
        }

        public Table Transform(in Table table)
        {
            if (table == null) return table;

            foreach (var row in table.Rows)
            {
                foreach (var k in row.Keys)
                    row[k] = _transformer?.Transform(row[k], ScenarioContext);
            }
            return table;
        }

        public FeatureContext FeatureContext => _testRunner.FeatureContext;

        public ScenarioContext ScenarioContext => _testRunner.ScenarioContext;

        public ITestThreadContext TestThreadContext => _testRunner.TestThreadContext;

        string ITestRunner.TestWorkerId { get => _testWorkerId; }

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
            ((IParameterTransformer)ScenarioContext?.GetBindingInstance(typeof(IParameterTransformer)))?.ClearBespokeTransformers();
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