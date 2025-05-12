namespace DSL.ReqnrollPlugin.Transformers
{
    public interface IEnvironmentVariableTransformer : ITransformer
    {
        string GetEnvironmentVariable(string key);
    }
}