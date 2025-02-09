using System;
using System.Threading.Tasks;
using Reqnroll;
using Reqnroll.Infrastructure;

namespace DSL
{
    internal sealed class DSLTestRunner : ITestRunner
    {
        private readonly ITestRunner _TestRunner;
        private readonly IParameterTransform _Transform;
        private string _testWorkerId;

        public DSLTestRunner(ITestExecutionEngine executionEngine, IParameterTransform tranform)
        {
            _TestRunner = new TestRunner(executionEngine);
            _Transform = tranform;
        }

        [Obsolete]
        public void InitializeTestRunner(string testWorkerId)
        {
            _testWorkerId = testWorkerId;
            _TestRunner.InitializeTestRunner(testWorkerId);
        }

        public string Transform(string obj)
        {
            return _Transform.Transform(obj, ScenarioContext);
        }

        public Table Transform(Table table)
        {
            if (table == null) return table;

            foreach (var row in table.Rows)
            {
                foreach (var k in row.Keys)
                    row[k] = _Transform.Transform(row[k], ScenarioContext);
            }
            return table;
        }

        public FeatureContext FeatureContext => _TestRunner.FeatureContext;

        public ScenarioContext ScenarioContext => _TestRunner.ScenarioContext;

        public ITestThreadContext TestThreadContext => _TestRunner.TestThreadContext;

        string ITestRunner.TestWorkerId { get => _testWorkerId; }

        public async Task AndAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
        {
            await _TestRunner.AndAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
        }

       public async Task ButAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
       {
           await _TestRunner.ButAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
       }

       public async Task GivenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
       {
           await _TestRunner.GivenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
       }

       public async Task OnFeatureEndAsync()
       {
           await _TestRunner.OnFeatureEndAsync();
       }

       public async Task OnFeatureStartAwait(FeatureInfo featureInfo)
       {
           await _TestRunner.OnFeatureStartAsync(featureInfo);
       }

       public async Task OnScenarioEndAsync()
       {
           await _TestRunner.OnScenarioEndAsync();
       }

       public async Task OnTestRunEndAsync()
       {
           await _TestRunner.OnTestRunEndAsync();
       }

       public async Task OnTestRunStartAsync()
       {
           await _TestRunner.OnTestRunStartAsync();
       }

       public void Pending()
       {
           _TestRunner.Pending();
       }

       public async Task ThenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
       {
           await _TestRunner.ThenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
       }

       public async Task WhenAsync(string text, string multilineTextArg, Table tableArg, string keyword = null)
       {
           await _TestRunner.WhenAsync(Transform(text), Transform(multilineTextArg), Transform(tableArg), keyword);
       }

       public void OnScenarioInitialize(ScenarioInfo scenarioInfo)
       {
           _TestRunner.OnScenarioInitialize(scenarioInfo);
       }

       public async Task OnScenarioStartAsync()
       {
            ((IParameterTransform) ScenarioContext.GetBindingInstance(typeof(IParameterTransform)))?.ClearTransformers();
            await _TestRunner.OnScenarioStartAsync();
       }

       public void SkipScenario()
       {
           _TestRunner.SkipScenario();
       }

        public async Task OnFeatureStartAsync(FeatureInfo featureInfo)
        {
            await _TestRunner.OnFeatureStartAsync(featureInfo);
        }

        public async Task CollectScenarioErrorsAsync()
        {
            await _TestRunner.CollectScenarioErrorsAsync();
        }
    }
}