namespace OpenBots.Core.Script
{
    public interface IScriptVariable
    {
        bool IsSecureString { get; set; }
        string VariableName { get; set; }
        object VariableValue { get; set; }
    }
}