namespace Resco.Controls.MessageBox
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class MessageBoxEx
    {
        private static Dictionary<DialogResult, string> m_defaultButtonLabels;
        private static Resco.Controls.MessageBox.MenuItem[] m_yesNoCancel;

        static MessageBoxEx()
        {
            if (((Environment.OSVersion.Platform != PlatformID.Win32NT) && (Environment.OSVersion.Platform != PlatformID.Win32S)) && (Environment.OSVersion.Platform != PlatformID.Win32Windows))
            {
                RescoLicenseMessage.ShowEvaluationMessage(typeof(MessageBoxEx), "");
            }
        }

        private static void MakeButtonCache()
        {
            if (m_yesNoCancel == null)
            {
                m_yesNoCancel = new Resco.Controls.MessageBox.MenuItem[] { new Resco.Controls.MessageBox.MenuItem(string.Empty, DialogResult.Yes), new Resco.Controls.MessageBox.MenuItem(string.Empty, DialogResult.No), new Resco.Controls.MessageBox.MenuItem(string.Empty, DialogResult.Cancel) };
            }
        }

        public static void Show(string text)
        {
            MakeButtonCache();
            m_yesNoCancel[0].Set("OK", DialogResult.OK);
            Show(MessageBoxIcon.Asterisk, true, text, new Resco.Controls.MessageBox.MenuItem[] { m_yesNoCancel[0] });
        }

        public static void Show(string text, string caption)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = caption;
            }
            Show(text);
        }

        public static DialogResult Show(MessageBoxIcon icon, bool bMultiLine, string text, params Resco.Controls.MessageBox.MenuItem[] items)
        {
            return PopupMenuForm.Show<DialogResult>(icon, bMultiLine, text, DialogResult.Cancel, items);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            DialogResult result;
            DialogResult[] resultArray;
            MakeButtonCache();
            if (string.IsNullOrEmpty(text))
            {
                text = caption;
            }
            if (text == null)
            {
                text = string.Empty;
            }
            bool bMultiLine = (text.Length > 0x10) || (text.IndexOf('\n') >= 0);
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    resultArray = new DialogResult[] { DialogResult.OK };
                    break;

                case MessageBoxButtons.OKCancel:
                    resultArray = new DialogResult[] { DialogResult.OK, DialogResult.Cancel };
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    resultArray = new DialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };
                    break;

                case MessageBoxButtons.YesNoCancel:
                    resultArray = new DialogResult[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
                    break;

                case MessageBoxButtons.YesNo:
                    resultArray = new DialogResult[] { DialogResult.Yes, DialogResult.No };
                    break;

                case MessageBoxButtons.RetryCancel:
                    resultArray = new DialogResult[] { DialogResult.Retry, DialogResult.Cancel };
                    break;

                default:
                    throw new ArgumentOutOfRangeException("buttons");
            }
            if (defaultButton == MessageBoxDefaultButton.Button1)
            {
                result = resultArray[0];
            }
            else if ((defaultButton == MessageBoxDefaultButton.Button2) && (resultArray.Length > 1))
            {
                result = resultArray[1];
            }
            else if ((defaultButton == MessageBoxDefaultButton.Button3) && (resultArray.Length > 2))
            {
                result = resultArray[2];
            }
            else
            {
                result = ~DialogResult.None;
            }
            for (int i = 0; i < resultArray.Length; i++)
            {
                DialogResult result2 = resultArray[i];
                string str;
                    DefaultButtonLabels.TryGetValue(result2,out str);
                m_yesNoCancel[i].Set(str, result2);
            }
            Resco.Controls.MessageBox.MenuItem[] items = new Resco.Controls.MessageBox.MenuItem[resultArray.Length];
            for (int j = 0; j < resultArray.Length; j++)
            {
                items[j] = m_yesNoCancel[j];
            }
            return PopupMenuForm.Show<DialogResult>(icon, bMultiLine, text, result, items);
        }

        public static int? ShowList(string title, string defaultText, params string[] items)
        {
            return ShowList(title, false, defaultText, items);
        }

        public static int? ShowList(string title, bool multiline, string defaultText, params string[] items)
        {
            List<Resco.Controls.MessageBox.MenuItem> list = new List<Resco.Controls.MessageBox.MenuItem>();
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(new Resco.Controls.MessageBox.MenuItem(items[i], i));
            }
            if (!string.IsNullOrEmpty(defaultText))
            {
                list.Add(new Resco.Controls.MessageBox.MenuItem(defaultText, null));
            }
            return PopupMenuForm.Show<int?>(MessageBoxIcon.None, multiline, title, null, list.ToArray());
        }

        public static bool ShowYesCancel(string title, params string[] items)
        {
            MakeButtonCache();
            m_yesNoCancel[0].Set(items[0], DialogResult.Yes);
            m_yesNoCancel[2].Set(items[1], DialogResult.Cancel);
            return (Show(MessageBoxIcon.Question, false, title, new Resco.Controls.MessageBox.MenuItem[] { m_yesNoCancel[0], m_yesNoCancel[2] }) == DialogResult.Yes);
        }

        public static DialogResult ShowYesNoCancel(string title, params string[] items)
        {
            MakeButtonCache();
            m_yesNoCancel[0].Set(items[0], DialogResult.Yes);
            m_yesNoCancel[1].Set(items[1], DialogResult.No);
            m_yesNoCancel[2].Set(items[2], DialogResult.Cancel);
            return Show(MessageBoxIcon.Question, false, title, m_yesNoCancel);
        }

        public static Dictionary<DialogResult, string> DefaultButtonLabels
        {
            get
            {
                if (m_defaultButtonLabels == null)
                {
                    m_defaultButtonLabels = new Dictionary<DialogResult, string>();
                    m_defaultButtonLabels.Add(DialogResult.OK, "OK");
                    m_defaultButtonLabels.Add(DialogResult.Yes, "Yes");
                    m_defaultButtonLabels.Add(DialogResult.No, "No");
                    m_defaultButtonLabels.Add(DialogResult.Cancel, "Cancel");
                    m_defaultButtonLabels.Add(DialogResult.Retry, "Retry");
                    m_defaultButtonLabels.Add(DialogResult.Ignore, "Ignore");
                    m_defaultButtonLabels.Add(DialogResult.Abort, "Abort");
                }
                return m_defaultButtonLabels;
            }
        }

        public static MessageBoxSettings Settings
        {
            get
            {
                return PopupMenuForm.Settings;
            }
            set
            {
                PopupMenuForm.Settings = value;
            }
        }
    }
}

