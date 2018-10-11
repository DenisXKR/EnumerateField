using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using EnumerateField;

namespace EnumerateField_Test.Features.ListsDeploy
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("f75d9fd2-4615-4a16-b867-317b8ebefd71")]
    public class ListsDeployEventReceiver : SPFeatureReceiver
    {
        protected const string testListName = "TestList";
        protected const string testListDescription = "Списко для тестирования поля";
        protected const string testFieldName = "enm";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            if (web.Lists.TryGetList(testListName) == null)
            {
                web.AllowUnsafeUpdates = true;

                Guid listGuid = web.Lists.Add(testListName, testListDescription, SPListTemplateType.GenericList);

                SPList list = web.Lists[listGuid];

                if (list.Fields.TryGetFieldByStaticName(testFieldName) == null)
                {
                    string fieldXML = "<Field Type=\"EnumField\" DisplayName=\"enm\" Required=\"FALSE\" EnforceUniqueValues=\"FALSE\" WebSite=\""+ web.ServerRelativeUrl +"\" ListField=\"" + web.Lists["EnumFieldSettingsList"].ID.ToString() + "\" " +
                                      "PrefixField=\"Title\" SerialNumberField=\"SerialNumber\" NumberSymbolCount=\"5\" ID=\"" + Guid.NewGuid().ToString() + "\" "+
                                      "StaticName=\"enm\" Name=\"enm\" ColName=\"nvarchar3\" RowOrdinal=\"0\" Group=\"\" Version=\"4\"><Customization><ArrayOfProperty><Property><Name>WebSite</Name>" +
                                      "</Property><Property><Name>List</Name></Property><Property><Name>PrefixField</Name></Property><Property><Name>SerialNumberField</Name></Property><Property><Name>" +
                                      "NumberSymbolCount</Name></Property></ArrayOfProperty></Customization></Field>";

                    list.Fields.AddFieldAsXml(fieldXML);
                    list.Update();
                    web.Update();

                    SPField field = list.Fields[testFieldName];
                    SPView view = list.DefaultView;
                    view.ViewFields.Add(field);
                    view.Update();
                }                    
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;
            SPList list = web.Lists.TryGetList(testListName);

            if (list != null)
            {
                web.AllowUnsafeUpdates = true;
                web.Lists.Delete(list.ID);
                web.Update();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
