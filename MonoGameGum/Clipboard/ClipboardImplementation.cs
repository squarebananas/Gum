using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextCopy;

namespace MonoGameGum.Clipboard
{
    public class ClipboardImplementation
    {
        public static IClipboard InjectedClipboard;
        private static Task<string?>? _injectedClipboardTask;

        internal static string? GetText(Action? callback)
        {
            if (InjectedClipboard != null)
            {
                if (_injectedClipboardTask == null)
                {
                    _injectedClipboardTask = Task.Run(async () => await InjectedClipboard.GetTextAsync());
                    _injectedClipboardTask.ContinueWith((t) => callback?.Invoke());
                }
                else
                {
                    if (_injectedClipboardTask.IsCompleted)
                    {
                        string? result = _injectedClipboardTask.Result;
                        _injectedClipboardTask = null;
                        return result;
                    }
                }

                return null;
            }

            return ClipboardService.GetText();
        }

        internal static void PushStringToClipboard(string text)
        {
            if (InjectedClipboard != null)
            {
                InjectedClipboard.SetTextAsync(text);
                return;
            }

            ClipboardService.SetText(text);
        }
    }
}
