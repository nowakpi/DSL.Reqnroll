using Reqnroll;
using System;

namespace DSL.ReqnrollPlugin
{
    public interface ITransformer
    {
        string Transform(in string pattern, in ScenarioContext context);
    }
}