namespace Resco.Controls.MaskedTextBox
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    public class MaskedTextProvider : ICloneable
    {
        private string m_format;
        private bool m_includeLiterals;
        private bool m_includePrompt;
        private string m_mask;
        private int m_optCnt;
        private Hashtable m_posNdx;
        private char m_promptChar;
        private static Hashtable m_regexps;
        private int m_reqdCnt;
        private bool m_resetOnPrompt;
        private bool m_resetOnSpace;
        private string m_text;
        protected const char MASK_KEY = '@';

        public event EventHandler TextChanged;

        public MaskedTextProvider()
        {
            this.m_text = "";
            this.m_promptChar = '_';
            this.m_mask = "";
            this.m_format = "";
            this.m_includeLiterals = true;
            this.m_includePrompt = false;
            this.m_resetOnSpace = true;
            this.m_resetOnPrompt = true;
        }

        public MaskedTextProvider(MaskedTextProvider maskedTextProvider)
        {
            this.PromptChar = maskedTextProvider.PromptChar;
            this.m_format = maskedTextProvider.m_format;
            this.IncludeLiterals = maskedTextProvider.IncludeLiterals;
            this.IncludePrompt = maskedTextProvider.IncludePrompt;
            this.ResetOnSpace = maskedTextProvider.ResetOnSpace;
            this.ResetOnPrompt = maskedTextProvider.ResetOnPrompt;
            this.Mask = maskedTextProvider.Mask;
            this.Text = maskedTextProvider.Text;
        }

        public static void AddMaskChar(char mask, string regExp)
        {
            RegExps[mask] = regExp;
            string str = (string) RegExps['@'];
            if (str.IndexOf(mask) < 0)
            {
                RegExps['@'] = str.Insert(str.Length - 1, mask.ToString());
            }
        }

        public virtual void Clear()
        {
            this.BaseText = this.m_format;
        }

        public MaskedTextProvider Clone()
        {
            return new MaskedTextProvider(this);
        }

        private string ExcludeLiterals(string text)
        {
            if (this.m_mask == "")
            {
                return text;
            }
            string str = "";
            string mask = this.Mask;
            string str3 = text;
            int num = 0;
            bool flag = true;
            for (int i = 0; (i < mask.Length) && (num < str3.Length); i++)
            {
                string str4 = (string) RegExps[mask[i]];
                if (IsValidMaskChar(mask[i]) && ((this.VerifyChar(str3[num], i) || (str3[num] == this.m_promptChar)) || (((str4 != null) && (str4.IndexOf(' ') < 0)) && (str4.IndexOf('?') < 0))))
                {
                    if (((flag && (str4 != null)) && ((str4.IndexOf(' ') != -1) || (str4.IndexOf('?') != -1))) && ((str3[num] == ' ') || (str3[num] == this.m_promptChar)))
                    {
                        num++;
                    }
                    else
                    {
                        str = str + str3[num++];
                        flag = false;
                    }
                }
                else if (str3[num] == this.m_format[i])
                {
                    num++;
                }
            }
            return str;
        }

        public virtual int FindEditPositionFrom(int position, bool direction)
        {
            int num = position;
            int num2 = num;
            if (!direction)
            {
                while (num > 0)
                {
                    num--;
                    if (this.m_format[num] == this.m_promptChar)
                    {
                        return num;
                    }
                }
                return num2;
            }
            while (num < (this.m_format.Length - 1))
            {
                num++;
                if (this.m_format[num] == this.m_promptChar)
                {
                    return num;
                }
            }
            return num2;
        }

        public virtual int InsertAt(string input, int position)
        {
            if ((this.Mask != "") && !IsValidMaskChar(this.Mask[(int) this.m_posNdx[position]]))
            {
                position = this.FindEditPositionFrom(position, true);
            }
            string baseText = this.BaseText;
            string str2 = input;
            if ((position >= 0) && (position < baseText.Length))
            {
                int length = str2.Length;
                if ((position + length) >= baseText.Length)
                {
                    length = baseText.Length - position;
                }
                string s = baseText.Substring(0, position) + str2.Substring(0, length) + baseText.Substring(position + length);
                if (this.VerifyString(s, true))
                {
                    this.BaseText = s;
                    return length;
                }
            }
            return 0;
        }

        public static bool IsValidMaskChar(char c)
        {
            return Regex.IsMatch(c.ToString(), (string) RegExps['@']);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (this.TextChanged != null)
            {
                this.TextChanged(this, e);
            }
        }

        public virtual void RemoveAt(int position)
        {
            this.RemoveAt(position, 1);
        }

        public virtual void RemoveAt(int position, int length)
        {
            if ((this.Mask != "") && !IsValidMaskChar(this.Mask[(int) this.m_posNdx[position]]))
            {
                position = this.FindEditPositionFrom(position, true);
            }
            string baseText = this.BaseText;
            this.BaseText = baseText.Substring(0, position) + this.m_format.Substring(position, length) + baseText.Substring(position + length);
        }

        public static void RemoveMaskChar(char mask)
        {
            RegExps.Remove(mask);
            string str = (string) RegExps['@'];
            int index = str.IndexOf(mask);
            if (index != -1)
            {
                RegExps['@'] = str.Remove(index, 1);
            }
        }

        public virtual bool Replace(char input, int position)
        {
            return this.Replace(input, position, 1);
        }

        public virtual bool Replace(char input, int position, int length)
        {
            if (length <= 0)
            {
                length = 1;
            }
            if ((this.Mask != "") && !IsValidMaskChar(this.Mask[(int) this.m_posNdx[position]]))
            {
                position = this.FindEditPositionFrom(position, true);
            }
            bool flag = this.VerifyChar(input, (int) this.m_posNdx[position]);
            if ((!flag && ((this.m_resetOnSpace && (input == ' ')) || (this.m_resetOnPrompt && (input == this.m_promptChar)))) && IsValidMaskChar(this.Mask[position]))
            {
                flag = true;
                input = this.m_promptChar;
            }
            if (flag)
            {
                string str = (this.BaseText.Substring(0, position) + input.ToString()) + this.m_format.Substring(position + 1, length - 1) + this.BaseText.Substring(position + length);
                this.BaseText = str;
                return true;
            }
            return false;
        }

        public bool Set(string input)
        {
            if (this.m_mask == "")
            {
                this.BaseText = input;
            }
            else
            {
                input = this.ExcludeLiterals(input);
                string s = "";
                int num = 0;
                int num2 = input.Length - this.m_reqdCnt;
                if (input == "")
                {
                    this.BaseText = this.m_format;
                }
                else
                {
                    if (input.Length <= (this.m_reqdCnt + this.m_optCnt))
                    {
                        for (int i = 0; i < this.m_format.Length; i++)
                        {
                            if ((num < input.Length) && (this.m_format[i] == this.m_promptChar))
                            {
                                string str2 = (string) RegExps[this.Mask[(int) this.m_posNdx[i]]];
                                if ((str2.IndexOf(' ') != -1) || (str2.IndexOf('?') != -1))
                                {
                                    if (num2 > 0)
                                    {
                                        s = s + input[num++];
                                        num2--;
                                    }
                                    else
                                    {
                                        s = s + this.m_format[i];
                                    }
                                }
                                else if (this.VerifyChar(input[num], (int) this.m_posNdx[i]))
                                {
                                    s = s + input[num++];
                                }
                                else
                                {
                                    s = s + this.m_promptChar;
                                    num++;
                                }
                            }
                            else
                            {
                                s = s + this.m_format[i];
                            }
                        }
                        if (this.VerifyString(s, true))
                        {
                            this.BaseText = s;
                            goto Label_01C4;
                        }
                    }
                    return false;
                }
            }
        Label_01C4:
            return true;
        }

        private void SetupMask()
        {
            string mask = this.Mask;
            this.m_format = "";
            if (this.m_posNdx == null)
            {
                this.m_posNdx = new Hashtable();
            }
            else
            {
                this.m_posNdx.Clear();
            }
            int key = 0;
            this.m_reqdCnt = 0;
            this.m_optCnt = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                if (IsValidMaskChar(mask[i]))
                {
                    this.m_posNdx.Add(key, i);
                    this.m_format = this.m_format + this.m_promptChar;
                    string str2 = (string) RegExps[this.Mask[i]];
                    if ((str2.IndexOf(' ') != -1) || (str2.IndexOf('?') != -1))
                    {
                        this.m_optCnt++;
                    }
                    else
                    {
                        this.m_reqdCnt++;
                    }
                }
                else if (mask[i] == '\\')
                {
                    i++;
                    this.m_format = this.m_format + mask[i].ToString();
                }
                else
                {
                    this.m_format = this.m_format + mask[i].ToString();
                }
                key++;
            }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public virtual string ToDisplayString()
        {
            return this.BaseText;
        }

        public override string ToString()
        {
            return this.ToString(this.IncludePrompt, this.IncludeLiterals);
        }

        public virtual string ToString(bool includePrompt, bool includeLiterals)
        {
            if (this.m_mask == "")
            {
                return this.BaseText;
            }
            string str = "";
            string mask = this.Mask;
            string baseText = this.BaseText;
            if (this.VerifyString(baseText, true))
            {
                int num = 0;
                for (int i = 0; i < mask.Length; i++)
                {
                    if (((IsValidMaskChar(mask[i]) && (baseText[num] != ' ')) || includeLiterals) && ((baseText[num] != this.m_promptChar) || includePrompt))
                    {
                        str = str + baseText[num];
                    }
                    else if (mask[i] == '\\')
                    {
                        i++;
                    }
                    else if (baseText[num] == this.m_promptChar)
                    {
                        str = str + " ";
                    }
                    num++;
                }
            }
            return str;
        }

        public virtual bool VerifyChar(char input, int position)
        {
            return (((position >= 0) && (position < this.Mask.Length)) && Regex.IsMatch(input.ToString(), Convert.ToString(RegExps[this.Mask[position]])));
        }

        public virtual bool VerifyString(string input)
        {
            if ((input != null) && (input.Length != 0))
            {
                return this.VerifyString(input, false);
            }
            return true;
        }

        private bool VerifyString(string s, bool allowInputChar)
        {
            bool flag = true;
            for (int i = 0; flag && (i < this.m_format.Length); i++)
            {
                if (this.m_format[i] == this.m_promptChar)
                {
                    string str = (string) RegExps[this.Mask[(int) this.m_posNdx[i]]];
                    if (i >= s.Length)
                    {
                        flag = (str.IndexOf(' ') != -1) || (str.IndexOf('?') != -1);
                    }
                    else
                    {
                        flag = this.VerifyChar(s[i], (int) this.m_posNdx[i]);
                        if (!flag)
                        {
                            flag |= (((str.IndexOf(' ') != -1) || (str.IndexOf('?') != -1)) || allowInputChar) && ((s[i] == ' ') || (s[i] == this.m_promptChar));
                        }
                    }
                }
                else if (i < s.Length)
                {
                    flag = s[i] == this.m_format[i];
                }
            }
            return flag;
        }

        protected virtual string BaseText
        {
            get
            {
                return this.m_text;
            }
            set
            {
                if (this.m_text != value)
                {
                    this.m_text = value;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
        }

        public virtual string Format
        {
            get
            {
                return this.m_format;
            }
        }

        public bool IncludeLiterals
        {
            get
            {
                return this.m_includeLiterals;
            }
            set
            {
                this.m_includeLiterals = value;
            }
        }

        public bool IncludePrompt
        {
            get
            {
                return this.m_includePrompt;
            }
            set
            {
                this.m_includePrompt = value;
            }
        }

        public virtual string Mask
        {
            get
            {
                return this.m_mask;
            }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                this.m_mask = value;
                this.SetupMask();
                if (this.m_mask != "")
                {
                    if ((this.BaseText.Length == 0) || !this.VerifyString(this.BaseText, false))
                    {
                        this.BaseText = this.m_format;
                    }
                    else
                    {
                        this.Text = this.Text;
                    }
                }
            }
        }

        public virtual bool MaskCompleted
        {
            get
            {
                return this.VerifyString(this.BaseText, false);
            }
        }

        public virtual char PromptChar
        {
            get
            {
                return this.m_promptChar;
            }
            set
            {
                this.m_promptChar = value;
                this.Mask = this.m_mask;
            }
        }

        protected static Hashtable RegExps
        {
            get
            {
                if (m_regexps == null)
                {
                    m_regexps = new Hashtable();
                    m_regexps.Add('0', "[0-9]");
                    m_regexps.Add('9', "[0-9 ]");
                    m_regexps.Add('#', @"[\+\-0-9 ]");
                    m_regexps.Add('L', "[a-z]");
                    m_regexps.Add('l', "[a-z ]");
                    m_regexps.Add('U', "[A-Z]");
                    m_regexps.Add('u', "[A-Z ]");
                    m_regexps.Add('A', "[a-zA-Z]");
                    m_regexps.Add('a', "[a-zA-Z ]");
                    m_regexps.Add('D', "[a-zA-Z0-9]");
                    m_regexps.Add('d', "[a-zA-Z0-9 ]");
                    m_regexps.Add('C', ".");
                    m_regexps.Add('@', "[09#LlUuAaDdC]");
                }
                return m_regexps;
            }
        }

        public bool ResetOnPrompt
        {
            get
            {
                return this.m_resetOnPrompt;
            }
            set
            {
                this.m_resetOnPrompt = value;
            }
        }

        public bool ResetOnSpace
        {
            get
            {
                return this.m_resetOnSpace;
            }
            set
            {
                this.m_resetOnSpace = value;
            }
        }

        public virtual string Text
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this.Set(value);
            }
        }
    }
}

