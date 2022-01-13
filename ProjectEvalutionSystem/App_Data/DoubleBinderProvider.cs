using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeddle.Business.Binders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    namespace Skeddle.Business
    {
        public class DoubleBinderProvider : IModelBinderProvider
        {
            public IModelBinder GetBinder(ModelBinderProviderContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (context.Metadata.ModelType == typeof(double))
                {
                    return new DoubleBinder();
                }
                if (context.Metadata.ModelType == typeof(double?))
                {
                    return new DoubleBinder();
                }

                return null;
            }
        }
    }

}
