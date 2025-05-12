using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface ITransformerAggregator
    {
        string Transform(string inputString, ScenarioContext context);
    }
}