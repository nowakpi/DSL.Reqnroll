namespace DSL.ReqnrollPlugin.Transformers
{
    public interface IFunctionTransformer : ITransformer
    {
        string[] GetSupportedFunctions();
    }
}
