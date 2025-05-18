using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public interface ITransformer
    {
        byte TransformerId { get; }
        string Transform(in string inputString, in ScenarioContext scenarioContext);
    }
}