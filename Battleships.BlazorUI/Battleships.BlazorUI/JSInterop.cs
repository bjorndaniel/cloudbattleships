using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Battleships.BlazorUI
{
    public class JsInterop
    {
        public static string InitSignalR()
        {
            return RegisteredFunction.Invoke<string>(
                "Battleships.BlazorUI.JsInterop.InitSignalR");
        }
        public static string Alert(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "Battleships.BlazorUI.JsInterop.Alert", message);
        }
    }
}
