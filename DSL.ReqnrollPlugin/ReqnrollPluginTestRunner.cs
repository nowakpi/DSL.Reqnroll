using System;
using DSL.ReqnrollPlugin.Transformers;
using Reqnroll;
using Reqnroll.Infrastructure;

namespace DSL.ReqnrollPlugin
{
    public sealed partial class ReqnrollPluginTestRunner : ITestRunner
    {
        private readonly ITestRunner _testRunner;
        private readonly ITransformerAggregator _transformerAggregator;
        private string _testWorkerId;

        public FeatureContext FeatureContext => _testRunner.FeatureContext;
        public ScenarioContext ScenarioContext => _testRunner.ScenarioContext;
        public ITestThreadContext TestThreadContext => _testRunner.TestThreadContext;
        
        string ITestRunner.TestWorkerId { get => _testWorkerId; }

        public ReqnrollPluginTestRunner(ITestExecutionEngine executionEngine, ITransformerAggregator transformerAggregator)
        {
            _testRunner = new TestRunner(executionEngine);
            _transformerAggregator = transformerAggregator; 
            _transformerAggregator.ScenarioContext = _testRunner.ScenarioContext;
        }

        [Obsolete("When building Reqnroll plugins one need to supply this method which is itself marked as obsolete in ITestRunner interface.")]
        public void InitializeTestRunner(string testWorkerId)
        {
            _testWorkerId = testWorkerId;
            if (_testRunner != null)
            {
                _testRunner.InitializeTestRunner(testWorkerId);
                _transformerAggregator.ScenarioContext = _testRunner.ScenarioContext;
            }
        }

        private string Transform(in string obj) => _transformerAggregator?.Transform(obj, ScenarioContext);

        private Table Transform(in Table table)
        {
            if (table != null)
                foreach (var row in table.Rows)
                    foreach (var k in row.Keys) row[k] = _transformerAggregator?.Transform(row[k], ScenarioContext);

            return table;
        }
    }
}