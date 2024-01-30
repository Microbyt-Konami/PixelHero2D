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
        //private string _culture;

        //public Locale GetStartupLocale(ILocalesProvider availableLocales)
        //{
        //    return availableLocales.GetLocale(_culture);
        //}

        public void SetCulture(string culture) //=> LocalizationSettings.AvailableLocales.Locales.f[locale];
        {
            print($"setCulture {culture}");
            PlayerPrefs.SetString("selected-locale", culture);
            //StartCoroutine(SetCultureCourotine(culture));
        }

        //IEnumerator SetCultureCourotine(string culture)
        //{
        //    yield return null;

        //    var locales = LocalizationSettings.AvailableLocales.Locales;

        //    for (var i = 0; i < locales.Count; i++)
        //    {
        //        var locale = locales[i];

        //        if (locale.Identifier.Code == culture)
        //        {
        //            print($"Culture change {culture}");
        //            LocalizationSettings.SelectedLocale = locale;

        //            yield break;
        //        }
        //    }
        //}

        // Start is called before the first frame update
        //void Start()
        //{
        //    if (!LocalizationSettings.InitializationOperation.IsDone)
        //        StartCoroutine(Preload(null));
        //}

        // Update is called once per frame
        //void Update()
        //{

        //}


        //IEnumerator Preload(Locale locale)
        //{
        //    var operation = LocalizationSettings.InitializationOperation;

        //    do
        //    {
        //        // When we first initialize the Selected Locale will not be available however
        //        // it is the first thing to be initialized and will be available before the InitializationOperation is finished.
        //        if (locale == null)
        //            locale = LocalizationSettings.SelectedLocaleAsync.Result;

        //        print($"{locale?.Identifier.CultureInfo.NativeName} {operation.PercentComplete * 100}%");
        //        yield return null;
        //    }
        //    while (!operation.IsDone);

        //    if (operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        //    {
        //        print(operation.OperationException.ToString());
        //    }
        //    else
        //    {
        //        print("{locale?.Identifier.CultureInfo.NativeName} loaded");
        //    }
        //}
    }
}