using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public interface ITransformer
    {
        string Transform(in string inputString, in ScenarioContext scenarioContext);
    }
}