//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("Shopnear.Views.SearchPop.xaml", "Views/SearchPop.xaml", typeof(global::Shopnear.Views.SearchPop))]

namespace Shopnear.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\SearchPop.xaml")]
    public partial class SearchPop : global::Rg.Plugins.Popup.Pages.PopupPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.SearchBar search;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.CollectionView Products;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(SearchPop));
            search = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.SearchBar>(this, "search");
            Products = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.CollectionView>(this, "Products");
        }
    }
}