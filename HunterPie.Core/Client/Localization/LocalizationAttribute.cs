using System;

namespace HunterPie.Core.Client.Localization;

public class LocalizationAttribute(string xpath) : Attribute
{
    public string XPath { get; } = xpath;
}