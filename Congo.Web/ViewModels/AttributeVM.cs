using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congo.Web.ViewModels
{
    /// <summary>
    /// The view model for a single product attribute. 
    /// </summary>
    public class AttributeVM
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }
}