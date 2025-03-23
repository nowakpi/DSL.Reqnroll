using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public interface ITransformerAggregator
    {
        string Transform(in string pattern, in ScenarioContext context);
    }
}