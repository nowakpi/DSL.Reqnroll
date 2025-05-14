using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface ITransformer
    {
        string Transform(in string inputString, in ScenarioContext scenarioContext);
    }
}