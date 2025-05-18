using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface ITransformer
    {
        byte OrderId { get; }
        string Transform(in string inputString, in ScenarioContext scenarioContext);
    }
}