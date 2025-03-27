using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public interface ITransformerAggregator
    {
        string Transform(string pattern, ScenarioContext context);
    }
}