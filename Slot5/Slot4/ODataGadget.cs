using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Slot4.model;

namespace Slot4
{
    public class ODataGadget
    {
        public static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Gadgets>("GadgetsOData");
            return modelBuilder.GetEdmModel();
        }
    }
}
