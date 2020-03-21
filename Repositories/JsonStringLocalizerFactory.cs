using System;
using Microsoft.Extensions.Localization;

namespace TodoApiNet.Repositories
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        #region snippet_CreateByResource

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer();
        }

        #endregion

        #region snippet_CreateByBaseNameAndLocation

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer();
        }

        #endregion
    }
}