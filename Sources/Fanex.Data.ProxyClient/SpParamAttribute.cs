namespace Fanex.Data;

using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class SpParamAttribute : Attribute
{
    public string SqlParamName { get; set; }

    public bool Ignored { get; set; }

    public bool IsSpecialType { get; set; }

    public bool AllowNull
    {
        get => ForcePassingNull ?? false;
        set => ForcePassingNull = value;
    }

    private bool? ForcePassingNull { get; set; }
}