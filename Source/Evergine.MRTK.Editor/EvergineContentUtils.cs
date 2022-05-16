﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Evergine.MRTK.Editor
{
    public static class EvergineContentUtils
    {
        private static IEnumerable<TypeInfo> EvergineContentTypes;

        static EvergineContentUtils()
        {
            EvergineContentTypes = GetEvergineContentTypes().ToList();
        }

        public static Dictionary<string, string> FindFonts(string assetsRootPath)
        {
            return EvergineContentTypes
                    .SelectMany(x => GetClasses(x, string.Empty))
                    .SelectMany(x => GetFontFamilyNameFields(x.type, x.basePath, assetsRootPath))
                    .Distinct()
                    .ToDictionary(x => x.name, x => x.sourcePath);
        }

        private static IEnumerable<TypeInfo> GetEvergineContentTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                                          .SelectMany(x => x.DefinedTypes.Where(y => y.Name == "EvergineContent"));
        }

        private static IEnumerable<(TypeInfo type, string basePath)> GetClasses(TypeInfo x, string basePath = null, Func<TypeInfo, bool> filter = null)
        {
            if (filter?.Invoke(x) != false)
            {
                yield return (x, basePath);
            }

            foreach (var item in x.DeclaredNestedTypes)
            {
                var subBasePath = basePath == null ? null : $"{basePath}/{item.Name}";
                foreach (var nestedType in GetClasses(item, subBasePath, filter))
                {
                    yield return nestedType;
                }
            }
        }

        private static IEnumerable<(string name, string sourcePath)> GetFontFamilyNameFields(TypeInfo type, string basePath, string assetsRootPath)
        {
            return type.DeclaredFields.Where(field => field.Name.ToLowerInvariant().EndsWith("_ttf"))
                                      .Select(field => field.Name.Remove(field.Name.LastIndexOf('_')))
                                      .Distinct()
                                      .Where(name => name != "Arial") // Remove default font as a workaround
                                      .Select(name =>
                                      {
                                          using (var fontCollection = new System.Drawing.Text.PrivateFontCollection())
                                          {
                                              fontCollection.AddFontFile($"{assetsRootPath}{basePath}/{name}.ttf");
                                              var fontFamilyName = fontCollection.Families[0].Name;
                                              return (fontFamilyName, $"{basePath}/#{fontFamilyName}");
                                          }
                                      });
        }
    }
}
