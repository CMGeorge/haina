namespace Resco.Controls.AdvancedList
{
    using Resco.Controls.AdvancedList.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public sealed class HeaderRow : Row
    {
        public HeaderRow()
        {
        }

        public HeaderRow(int templateIndex, IList dataList) : base(templateIndex, templateIndex, dataList, Resco.Controls.AdvancedList.Mapping.Empty)
        {
        }

        public HeaderRow(int templateIndex, string tag, IList dataList) : base(templateIndex, templateIndex, dataList, Resco.Controls.AdvancedList.Mapping.Empty)
        {
            base.Tag = tag;
        }

        public override void Delete()
        {
        }

        public override int ActiveTemplateIndex
        {
            get
            {
                return base.ActiveTemplateIndex;
            }
        }

        public override int AlternateTemplateIndex
        {
            get
            {
                return base.AlternateTemplateIndex;
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public override int CurrentTemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
        }

        public override int Index
        {
            get
            {
                return base.Index;
            }
        }

        [DefaultValue(false), Resco.Controls.AdvancedList.Design.Browsable(false), Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden)]
        public override bool Selected
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility(Resco.Controls.AdvancedList.Design.DesignerSerializationVisibility.Hidden), Resco.Controls.AdvancedList.Design.Browsable(false)]
        public override int SelectedTemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
            set
            {
            }
        }

        [DefaultValue(0)]
        public override int TemplateIndex
        {
            get
            {
                return base.m_iTemplate;
            }
            set
            {
                if (base.m_iTemplate != value)
                {
                    base.m_iTemplate = value;
                    this.OnChanged();
                }
            }
        }
    }
}

