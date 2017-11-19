namespace Utilities.SeedWork
{
    public abstract class ConfigurationBase
    {
        protected static string FkColName(string fieldName) => $"{fieldName}Id";
    }
}
