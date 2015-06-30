<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="EnquiryForm.aspx.cs" Inherits="WebEnquiryForm.EnquiryForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <link type="text/css" href="Theme/jquery-ui-1.10.4.custom.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Theme/style.css" />
    <script>
        $(function () {
            $("#<%= txtDate.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
              
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>New Enquiry</h1>
            <label>Event Type</label>
            <asp:DropDownList ID="EventTypesDropDown"
                Width="200px"
                runat="server" Height="20px">
            </asp:DropDownList>
            <p>
                <label>Date</label>
                <asp:TextBox ID="txtDate" runat="server" OnTextChanged="txtDate_OnTextChanged" AutoPostBack="True"></asp:TextBox>
               <%-- <input type="date"/>--%>
                <asp:CheckBox ID="DateCheckBox" Text="No Specific Date" runat="server"
                    ViewStateMode="Enabled"
                    AutoPostBack="True" Checked="True" OnCheckedChanged="CheckBox1_OnCheckedChanged" />
            </p>
            <label>Event Name</label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <p>
                <label>Number of People</label>
                <asp:TextBox ID="txtNum" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server"
                    CssClass="validate"
                    Operator="DataTypeCheck" Type="Integer"
                    ControlToValidate="txtNum" ErrorMessage="Value must be a whole number" />
            </p>
            <p>
                <label>First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                &nbsp; 
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server"
                ErrorMessage="Required!"
                CssClass="validate"
                ControlToValidate="txtFirstName">
            </asp:RequiredFieldValidator>
            </p>
            <p>
                <label>Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                &nbsp; 
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server"
                ErrorMessage="Required!"
                CssClass="validate"
                ControlToValidate="txtLastName">
            </asp:RequiredFieldValidator>
            </p>
            <p>
                <label>Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                &nbsp; 
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator3" runat="server"
                ErrorMessage="Required!"
                CssClass="validate"
                ControlToValidate="txtEmail">
            </asp:RequiredFieldValidator>
            </p>
            <p>
                <label>Mobile Phone</label>
                <asp:TextBox Width="200px" ID="txtMobilePhone" runat="server"></asp:TextBox>
                &nbsp; 
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator4" runat="server"
                ErrorMessage="Required!"
                CssClass="validate"
                ControlToValidate="txtMobilePhone">
            </asp:RequiredFieldValidator>
            </p>
            <p>
                <label>Event Details</label>
                <asp:TextBox ID="txtEventDetails" Height="100" runat="server" TextMode="MultiLine" CssClass="textArea"></asp:TextBox>
            </p>
            <p>
                <asp:Button runat="server" Text="Submit" OnClick="OnClick" />
            </p>
        </div>
    </form>
</body>
</html>
