using Battleships.BlazorUi;
using Microsoft.AspNetCore.Blazor.Browser.Rendering;
using Microsoft.AspNetCore.Blazor.Browser.Services;

namespace Battleships.BlazorUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            new BrowserRenderer(new BrowserServiceProvider()).AddComponent<App>("app");
        }
    }
}
