using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Common
{
    /// <summary>
    /// custom dropdown list for support option groups
    /// </summary>
    [DefaultProperty("OptionGroupDataField")]
    [ToolboxData("<{0}:CustomDropDownList runat=server></{0}:CustomDropDownList>")]
    public class CustomDropDownList : DropDownList
    {
        /// <summary>
        /// Gets or sets the data option group field.
        /// </summary>
        /// <value>
        /// The data option group field.
        /// </value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string DataOptionGroupField
        {
            get
            {
                string dataOptionGroupField = (string)ViewState["OptionGroupDataField"];
                return (dataOptionGroupField == null) ? string.Empty : dataOptionGroupField;
            }

            set
            {
                ViewState["OptionGroupDataField"] = value;
            }
        }

        /// <summary>
        /// Renders the items in the <see cref="T:System.Web.UI.WebControls.ListControl"/> control.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream used to write content to a Web page.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Items.Count > 0)
            {
                bool selected = false;
                Dictionary<string, List<ListItem>> optionGroupsCollection = this.OptionGroupsCollection();
                foreach (string optionGroup in optionGroupsCollection.Keys)
                {
                    writer.WriteBeginTag("optgroup");
                    writer.WriteAttribute("label", optionGroup);
                    writer.Write('>');
                    writer.WriteLine();
                    foreach (ListItem item in optionGroupsCollection[optionGroup])
                    {
                        writer.WriteBeginTag("option");
                        if (item.Selected)
                        {
                            if (selected)
                            {
                                this.VerifyMultiSelect();
                            }

                            selected = true;
                            writer.WriteAttribute("selected", "selected");
                        }

                        writer.WriteAttribute("value", item.Value, true);

                        if (item.Attributes.Count > 0)
                        {
                            item.Attributes.Render(writer);
                        }

                        if (this.Page != null)
                        {
                            this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, item.Value);
                        }

                        writer.Write('>');
                        HttpUtility.HtmlEncode(item.Text, writer);
                        writer.WriteEndTag("option");
                        writer.WriteLine();
                    }
                }

                writer.WriteEndTag("optgroup");
            }
        }

        /// <summary>
        /// Option groups collection.
        /// </summary>
        /// <returns>Option groups collection</returns>
        private Dictionary<string, List<ListItem>> OptionGroupsCollection()
        {
            // temporary storage to options based on option group. inside this, 
            // all options will be grouped based on the option group value.
            Dictionary<string, List<ListItem>> optionGroupsCollection = new Dictionary<string, List<ListItem>>();
            string lastOptionGroup = string.Empty;
            int i = 0;
            List<ListItem> optionGroupMembers = null;

            // loop through the data source
            if (DataSource != null)
            {
                foreach (var dataRow in this.DataSource as IEnumerable)
                {
                    PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(dataRow);
                    PropertyDescriptor propertyDescriptor = propertyDescriptorCollection.Find(this.DataOptionGroupField, true);
                    ListItem item = this.Items[i];
                    if (item.Enabled)
                    {
                        if (lastOptionGroup != propertyDescriptor.GetValue(dataRow).ToString())
                        {
                            lastOptionGroup = propertyDescriptor.GetValue(dataRow).ToString();
                            if (optionGroupsCollection.ContainsKey(lastOptionGroup))
                            {
                                optionGroupMembers = (List<ListItem>)optionGroupsCollection[lastOptionGroup];
                            }
                            else
                            {
                                optionGroupMembers = new List<ListItem>();
                            }
                        }

                        optionGroupMembers.Add(item);

                        if (optionGroupsCollection.ContainsKey(lastOptionGroup))
                        {
                            optionGroupsCollection[lastOptionGroup] = optionGroupMembers;
                        }
                        else
                        {
                            optionGroupsCollection.Add(lastOptionGroup, optionGroupMembers);
                        }
                    }

                    i++;
                }
            }
            return optionGroupsCollection;
        }
    }
}