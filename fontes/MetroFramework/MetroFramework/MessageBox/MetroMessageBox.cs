using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Interfaces;

namespace MetroFramework
{
    /// <summary>
    /// Metro-styled message notification.
    /// </summary>
    public static class MetroMessageBox
    {
        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message)
        { return Show(owner, message, "Notification"); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title)
        { return Show(owner, message, title, MessageBoxButtons.OK); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons)
        { return Show(owner, message, title, buttons, MessageBoxIcon.None); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
        { return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1); }

        /*
        private static MetroForm DummyForm()
        {
            MetroForm form = new MetroForm();
            form.Size = new Size(550, 230);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.SizeGripStyle = SizeGripStyle.Hide;
            form.ShowIcon = false;
            form.ShadowType = MetroFormShadowType.AeroShadow;
            form.MinimizeBox = form.MaximizeBox = false;
            form.ControlBox = form.DisplayHeader = false;
            form.Padding = new Padding(0, 0, 0, 0);
            form.AutoSize = true;

            return form;
        }
        */

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultbutton"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, 
            MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {
            DialogResult _result = DialogResult.None;

            //bool freeowner = false;
            if (owner == null)
            {
                //freeowner = true;
                //owner = DummyForm();
                //((Form)owner).Show();
            }

            //if (owner != null)
            {
                Form _owner = (Form)owner;

                //if (_owner.WindowState == FormWindowState.Minimized)
                    //_owner.WindowState = FormWindowState.Normal;
                
                //int _minWidth = 500;
                //int _minHeight = 350;

                //if (_owner.Size.Width < _minWidth ||
                //    _owner.Size.Height < _minHeight)
                //{
                //    if (_owner.Size.Width < _minWidth && _owner.Size.Height < _minHeight) {
                //            _owner.Size = new Size(_minWidth, _minHeight);
                //    }
                //    else
                //    {
                //        if (_owner.Size.Width < _minWidth) _owner.Size = new Size(_minWidth, _owner.Size.Height);
                //        else _owner.Size = new Size(_owner.Size.Width, _minHeight);
                //    }

                //    int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_owner.Size.Width / 2)));
                //    int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_owner.Size.Height / 2)));
                //    _owner.Location = new Point(x, y);
                //}

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        SystemSounds.Hand.Play(); break;
                    case MessageBoxIcon.Exclamation:
                        SystemSounds.Exclamation.Play(); break;
                    case MessageBoxIcon.Question:
                        SystemSounds.Beep.Play(); break;
                    default:
                        SystemSounds.Asterisk.Play(); break;
                }

                MetroMessageBoxControl _control = new MetroMessageBoxControl();
                _control.Properties.Buttons = buttons;
                _control.Properties.DefaultButton = defaultbutton;
                _control.Properties.Icon = icon;
                _control.Properties.Message = message;
                _control.Properties.Title = title;
                _control.Padding = new Padding(0, 0, 0, 0);
                _control.ControlBox = false;
                _control.ShowInTaskbar = false;
                

                //if (freeowner)
                    //_control.Dock = DockStyle.Fill;

                //_owner.Controls.Add(_control);
                //if (_owner is IMetroForm)
                //{
                //    //if (((MetroForm)_owner).DisplayHeader)
                //    //{
                //    //    _offset += 30;
                //    //}
                //    _control.Theme = ((MetroForm)_owner).Theme;
                //    _control.Style = ((MetroForm)_owner).Style;
                //}

                _control.ArrangeApperance();

                int lt = message.TrimEnd('\r').Split(new char[] { '\r' }).Length * 23;

                if (owner != null && _owner.WindowState != FormWindowState.Minimized && _owner.Visible)
                {
                    _control.Size = new Size(_owner.Size.Width, Math.Min(_owner.Size.Height, Math.Max(lt, _control.Height)));
                    _control.Location = new Point(_owner.Location.X, _owner.Location.Y + (_owner.Height - _control.Height) / 2);
                    //int _overlaySizes = Convert.ToInt32(Math.Floor(_control.Size.Height * 0.28));
                    //_control.OverlayPanelTop.Size = new Size(_control.Size.Width, _overlaySizes - 30);
                    //_control.OverlayPanelBottom.Size = new Size(_control.Size.Width, _overlaySizes);
                    _control.ShowDialog();
                }
                else
                {
                    _control.ShowInTaskbar = true;
                    _control.StartPosition = FormStartPosition.Manual;
                    int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_control.Size.Width / 2)));
                    int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_control.Size.Height / 2)));

                    _control.Location = new Point(x, y);
                    _control.Size = new Size(_control.Size.Width, 
                            Math.Min(Screen.PrimaryScreen.WorkingArea.Size.Height, Math.Max(_control.Size.Height, lt)));
                    //_control.Show();

                    _control.ShowDialog();
                }
                _control.BringToFront();
                _control.SetDefaultButton();

                bool _cancelled = false;
                /*
                Action<MetroMessageBoxControl> _delegate = new Action<MetroMessageBoxControl>(ModalState);
                IAsyncResult _asyncresult = _delegate.BeginInvoke(_control, null, _delegate);

                try
                {
                    while (!_asyncresult.IsCompleted)
                    { 
                        Thread.Sleep(1); 
                        Application.DoEvents(); 
                    }
                }
                catch 
                {
                    _cancelled = true;

                    if (!_asyncresult.IsCompleted)
                    {
                        try { _asyncresult = null; }
                        catch { }
                    }

                    _delegate = null;
                }
                */
                if (!_cancelled)
                {
                    _result = _control.Result;
                    //_owner.Controls.Remove(_control);
                    _control.Dispose(); 
                    _control = null;
                }
                 
            }
            //if (freeowner)
                //((Form)owner).Dispose();

            return _result;
        }

        private static void ModalState(MetroMessageBoxControl control)
        {
            while (control.Visible)
            { }
        }

    }
}
