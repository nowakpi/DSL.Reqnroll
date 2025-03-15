using System;
using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public interface IParameterTransformer
    {
        string Transform(in string pattern, in ScenarioContext context);
        IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer);
        void ClearBespokeTransformers();
    }
}
