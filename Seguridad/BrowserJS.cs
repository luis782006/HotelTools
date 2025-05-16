using Microsoft.JSInterop;

namespace HotelTools.Seguridad
{
    public class BrowserJS
    {
        private readonly IJSRuntime _jsRuntime;


        public BrowserJS(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetCookie(string cookieName, string cookieToken)
        {
            await _jsRuntime.InvokeVoidAsync("extrasJS.SetCookie",cookieName,cookieToken);           
        }

        public async Task<string> GetCookie(string cookieName)
        {
            var cookieValue = await _jsRuntime.InvokeAsync<string>("extrasJS.GetCookie", cookieName);
            if (string.IsNullOrEmpty(cookieValue.ToString()))
            {
                return null;
            }
            return cookieValue;
        }
    }
}
