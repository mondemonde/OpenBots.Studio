using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicenseManager.Domain
{
    public static class ConfirmExtensions
    {
        public async static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
        {
            return await jsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
