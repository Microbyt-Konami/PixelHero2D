using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Unity.Localization.MicrobytKonamic
{
    public class LocatizationMessages : MonoBehaviour //, IStartupLocaleSelector
    {
        public void SetCulture(string culture) //=> LocalizationSettings.AvailableLocales.Locales.f[locale];
        {
            print($"setCulture {culture}");
            PlayerPrefs.SetString("selected-locale", culture);
        }
    }
}