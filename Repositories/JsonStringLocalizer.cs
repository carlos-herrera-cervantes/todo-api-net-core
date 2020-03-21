using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        List<JsonLocalization> localization = new List<JsonLocalization>();

        public JsonStringLocalizer()
        {
            var serializer = new JsonSerializer();
            localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(File.ReadAllText(@"localization.json"));

        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        #region snippet_GetAll

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name)).Select(l => new LocalizedString(l.Key, l.LocalizedValue[CultureInfo.CurrentCulture.Name], true));
        }

        #endregion

        #region snippet_CreateJsonStringLocalizer

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonStringLocalizer();
        }

        #endregion

        #region snippet_GetString

        private string GetString(string name)
        {
            var query = localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name));
            var value = query.FirstOrDefault(l => l.Key == name);
            return value.LocalizedValue[CultureInfo.CurrentCulture.Name];
        }

        #endregion
    }
}