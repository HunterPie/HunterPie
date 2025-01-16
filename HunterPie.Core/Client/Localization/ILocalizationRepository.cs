using HunterPie.Core.Client.Localization.Entity;
using System;

namespace HunterPie.Core.Client.Localization;

public interface ILocalizationRepository
{
    public LocalizationData FindBy(string path);

    public LocalizationData FindByEnum<T>(T value) where T : Enum;
}