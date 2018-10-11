<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnumFieldPropertyEditor.ascx.cs" Inherits="EnumerateField.ControlTemplates.EnumerateField.EnumFieldPropertyEditor" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>

<wssuc:InputFormControl LabelText="Веб-узел списка серий номеров" runat="server">
    <Template_Control>
	    <asp:DropDownList id="webColumnField" runat="server" OnSelectedIndexChanged="webColumnField_SelectedIndexChanged" AutoPostBack="true" />
    </Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl LabelText="Список серий номеров" runat="server">
    <Template_Control>
	    <asp:DropDownList id="listColumnField" runat="server" OnSelectedIndexChanged="listColumnField_SelectedIndexChanged" AutoPostBack="true" />
    </Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl LabelText="Поле наименования префикса серии номеров" runat="server">
    <Template_Control>
	    <asp:DropDownList id="prefixColumnField" runat="server" />
    </Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl LabelText="Поле порядкового номера серии номеров" runat="server">
    <Template_Control>
	    <asp:DropDownList id="serialNumberColumnField" runat="server" />
    </Template_Control>
</wssuc:InputFormControl>

<wssuc:InputFormControl LabelText="Количество символов в номере" runat="server">
    <Template_Control>
        <asp:TextBox id="numberCountColumnField" runat="server" />
    </Template_Control>
</wssuc:InputFormControl>