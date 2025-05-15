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

        public async Task<bool> SetCookie(string cookieName, string cookieToken)
        {
            await _jsRuntime.InvokeVoidAsync("extrasJS.SetCookie",cookieName,cookieToken);

            return true;
        }

        public async Task<object> GetCookie(string cookieName)
        {
            var cookieValue = await _jsRuntime.InvokeAsync<object>("extrasJS.GetCookie", cookieName);
            if (string.IsNullOrEmpty(cookieValue.ToString()))
            {
                return false;
            }
            return cookieValue;
        }
    }
}
