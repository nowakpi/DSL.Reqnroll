namespace DSL.ReqnrollPlugin
{
    public interface IFunctionTransformer : ITransformer
    {
        string[] GetSupportedFunctions();
    }
}
