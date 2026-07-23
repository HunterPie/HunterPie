using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Observability.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace HunterPie.Features.Languages.Repository;

internal class LocalizationRepository(
    IConfiguration config
) : ILocalizationRepository, ILocalizationRegistry
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly Lock _lock = new();
    private XmlDocument _document = new();

    public void Load()
    {
        lock (_lock)
        {
            _document = LocalizationDocumentFactory();
        }
    }

    public void Register(string path)
    {
        if (!File.Exists(path))
            return;

        XmlDocument document = new();
        document.Load(path);

        lock (_lock)
        {
            ImportNodes(
                source: document,
                target: _document
            );
        }
    }

    public LocalizationData FindBy(string path)
    {
        XmlAttributeCollection? attributes;
        lock (_lock)
        {
            attributes = _document.SelectSingleNode(path)?.Attributes;
        }

        if (attributes is not { } || attributes["String"]?.Value is not { } stringValue)
            return CreateDefault(path);

        return new LocalizationData(
            String: stringValue,
            Description: attributes["Description"]?.Value ?? path,
            Affixation: Enum.TryParse(attributes["Affixation"]?.Value, out Affixation afx) ? afx : Affixation.Prefix,
            Order: short.TryParse(attributes["Order"]?.Value, out short ord) ? ord : (short)0
        );
    }

    public bool ExistsBy(string path)
    {
        XmlAttributeCollection? attributes;
        lock (_lock)
        {
            attributes = _document.SelectSingleNode(path)?.Attributes;
        }

        return attributes?["String"]?.Value != null;
    }

    public string FindStringBy(string path)
    {
        XmlAttributeCollection? attributes;
        lock (_lock)
        {
            attributes = _document.SelectSingleNode(path)?.Attributes;
        }

        if (attributes is not { } || attributes["String"]?.Value is not { } stringValue)
            return path;

        return stringValue;
    }

    public LocalizationData FindByEnum<T>(T value) where T : notnull
    {
        string stringValue = value.ToString() ?? string.Empty;

        MemberInfo? memberInfo = value.GetType()
            .GetMember(stringValue)
            .FirstOrDefault();

        if (memberInfo is null)
            return CreateDefault(stringValue);

        LocalizationAttribute? attribute = memberInfo.GetCustomAttribute<LocalizationAttribute>();

        return attribute switch
        {
            { } => FindBy(attribute.XPath),
            _ => CreateDefault(stringValue)
        };
    }

    public IScopedLocalizationRepository WithScope(string scope) =>
        new ScopedLocalizationRepository(
            scopePath: scope,
            localizationRepository: this
        );

    #region Loading localization document

    private static LocalizationData CreateDefault(string path) => new(
        String: path,
        Description: path,
        Affixation: Affixation.Prefix,
        Order: 0
    );

    private XmlDocument LocalizationDocumentFactory()
    {
        string defaultLangPath = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");

        if (!File.Exists(defaultLangPath))
            throw new FileNotFoundException("Default localization file not found");

        var document = new XmlDocument();
        document.Load(defaultLangPath);

        string selectedLanguageDocument = Path.Combine(ClientInfo.LanguagesPath, config.Client.Language);

        if (!File.Exists(selectedLanguageDocument))
            throw new FileNotFoundException(
                $"Failed to find localization {Path.GetFileNameWithoutExtension(selectedLanguageDocument)}");

        if (defaultLangPath == selectedLanguageDocument)
        {
            _logger.Info("Loaded default language successfully");
            return document;
        }

        XmlDocument otherLanguage = new();
        otherLanguage.Load(selectedLanguageDocument);

        MergeExistingNodes(
            defaultDocument: document,
            newDocument: otherLanguage
        );

        _logger.Info($"Loaded localization {Path.GetFileNameWithoutExtension(selectedLanguageDocument)} successfully");

        return document;
    }

    private static void MergeExistingNodes(XmlDocument defaultDocument, XmlDocument newDocument)
    {
        if (defaultDocument.DocumentElement?.SelectNodes("//*") is not { } defaultNodes)
            return;

        foreach (XmlNode node in defaultNodes)
        {
            string? id = node.Attributes?["Id"]?.Value;

            if (id is null)
                continue;

            string path = GetFullParentPath(node);

            XmlNode? match = newDocument.DocumentElement?.SelectSingleNode($"//{path}/*[@Id='{id}']");

            if (match?.Attributes?["String"] is { } stringAttribute)
                node.Attributes!["String"]!.Value = stringAttribute.Value;

            if (match?.Attributes?["Description"] is { } descriptionAttribute)
                node.Attributes!["Description"]!.Value = descriptionAttribute.Value;
        }
    }

    private static void ImportNodes(XmlDocument source, XmlDocument target)
    {
        if (source.DocumentElement?.SelectNodes("//*") is not { } newNodes)
            return;

        if (target.DocumentElement is not { } targetElement)
            return;

        foreach (XmlNode node in newNodes)
        {
            string? id = node.Attributes?["Id"]?.Value;

            if (id is null)
                continue;

            string path = GetFullParentPath(node);
            XmlNode? match = targetElement.SelectSingleNode($"//{path}/*[@Id='{id}']");

            if (match is not null)
                continue;

            XmlNode nodeCopy = target.ImportNode(node, true);

            targetElement.SelectSingleNode($"//{path}")?.AppendChild(nodeCopy);
        }
    }

    private static string GetFullParentPath(XmlNode node, string path = "")
    {
        if (node.ParentNode?.Name is null || node.ParentNode.Name == "#document")
            return path;

        return GetFullParentPath(
            node: node.ParentNode,
            path: string.IsNullOrEmpty(path) ? node.ParentNode.Name : $"{node.ParentNode.Name}/{path}"
        );
    }
    #endregion
}