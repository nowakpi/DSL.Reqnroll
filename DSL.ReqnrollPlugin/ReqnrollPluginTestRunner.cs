using System;
using Reqnroll;
using Reqnroll.Infrastructure;

namespace DSL.ReqnrollPlugin
{
    public sealed partial class ReqnrollPluginTestRunner : ITestRunner
    {
        private readonly ITestRunner _testRunner;
        private readonly ITransformerAggregator _transformer;
        private string _testWorkerId;

        public FeatureContext FeatureContext => _testRunner.FeatureContext;
        public ScenarioContext ScenarioContext => _testRunner.ScenarioContext;
        public ITestThreadContext TestThreadContext => _testRunner.TestThreadContext;
        string ITestRunner.TestWorkerId { get => _testWorkerId; }

        public ReqnrollPluginTestRunner(ITestExecutionEngine executionEngine, ITransformerAggregator tranformer)
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

        private string Transform(in string obj)
        {
            return _transformer?.Transform(obj, ScenarioContext);
        }

        private Table Transform(in Table table)
        {
            if (table == null) return table;

            foreach (var row in table.Rows)
            {
                foreach (var k in row.Keys)
                    row[k] = _transformer?.Transform(row[k], ScenarioContext);
            }

            return table;
        }
    }
}