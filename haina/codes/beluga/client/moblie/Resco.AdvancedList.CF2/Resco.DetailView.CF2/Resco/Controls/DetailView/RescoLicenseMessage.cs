namespace Resco.Controls.DetailView
{
    using System;
    using System.Windows.Forms;

    internal class RescoLicenseMessage
    {
        internal static string MsgEval = "This is the evaluation version of the Resco {0} control.\r\n\r\nUse of this control is limited to evaluation purposes.\r\n\r\nMore info at \r\nhttp://www.resco.net/developer\r\n\r\n{1}\r\nCopyright (c) 2010 Resco";
        internal static string MsgEvalVersion = "Evaluation version";

        internal static void ShowEvaluationMessage(Type type, string text)
        {
            MessageBox.Show(string.Format(MsgEval, type.Name, text), MsgEvalVersion);
        }
    }
}

