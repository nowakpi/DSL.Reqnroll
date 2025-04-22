using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public interface ITransformerAggregator
    {
        string Transform(string inputString, ScenarioContext context);
    }
}