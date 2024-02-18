using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace Shopnear.Models
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
