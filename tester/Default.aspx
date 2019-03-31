<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="testsite.Default" %>
<%@ Register tagPrefix="usr" tagName="Foo" src="FooControl.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div>
        <usr:Foo runat="server"/>
    </div>
</form>
</body>
</html>