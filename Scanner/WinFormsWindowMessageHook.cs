﻿using System;
using System.Windows.Forms;
using Scanner.Twain;

namespace Scanner
{
    /// <summary>
    /// A windows message hook for WinForms applications.
    /// </summary>
    public class WinFormsWindowMessageHook : IWindowsMessageHook, IMessageFilter
    {
        IntPtr _windowHandle;
        bool _usingFilter;

        public WinFormsWindowMessageHook(Form window)
        {
            _windowHandle = window.Handle;
        }

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (FilterMessageCallback != null)
            {
                bool handled = false;
                FilterMessageCallback(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
                return handled;
            }

            return false;
        }

        public IntPtr WindowHandle { get { return _windowHandle; } }

        public bool UseFilter
        {
            get
            {
                return _usingFilter;
            }
            set
            {
                if (!_usingFilter && value == true)
                {
                    Application.AddMessageFilter(this);
                    _usingFilter = true;
                }

                if (_usingFilter && value == false)
                {
                    Application.RemoveMessageFilter(this);
                    _usingFilter = false;
                }
            }
        }

        public FilterMessage FilterMessageCallback { get; set; }
    }
}
