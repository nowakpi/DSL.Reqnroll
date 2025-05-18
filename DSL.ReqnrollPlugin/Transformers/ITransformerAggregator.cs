using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface ITransformerAggregator
    {
        ScenarioContext ScenarioContext { get; set; }
        string Transform(string inputString, ScenarioContext context);
    }
}