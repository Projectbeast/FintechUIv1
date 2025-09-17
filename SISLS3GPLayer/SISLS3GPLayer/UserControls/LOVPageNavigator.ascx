<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LOVPageNavigator.ascx.cs"
    Inherits="UserControls_LOVPageNavigator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="styleMainTable col-md-12">
    <div class="row" visible="false" runat="server" id="trMessage">
        <%--class="styleRecordCount"--%>
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 styleRecordCount">
            <asp:Label runat="server" ID="lblMessage" Text="No Records Found" Font-Size="Medium"
                class="styleMandatoryLabel"></asp:Label>
        </div>
    </div>
    <div class="row m-0" runat="server" id="trControl">

        <div class="col-12 d-flex justify-content-end p-3">
            <asp:Label runat="server" ID="lblTotalRecords" CssClass="mr-3 pagination-info"></asp:Label>
            <div class="d-flex mt-1">

                <asp:LinkButton ID="btnFirst" runat="server" CommandName="First" OnCommand="Navigation_Link" Enabled="true" CausesValidation="false" CssClass="btn btn-outline-success btn-sm grid-pagination-btn mr-2"><i class="fa fa-step-backward" aria-hidden="true" style="font-size :11px"></i> First</asp:LinkButton>
                <asp:LinkButton ID="btnPrevious" runat="server" Enabled="true" CommandName="Prev" OnCommand="Navigation_Link" CausesValidation="false" CssClass="btn btn-outline-success btn-sm grid-pagination-btn"><i class="fa fa-angle-left" aria-hidden="true"></i> Previous</asp:LinkButton>
                <div class="col">
                    <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel d-none" ID="lblCurrentPage"></asp:Label>
                   <asp:TextBox runat="server" ID="txtGotoPage" AutoPostBack="true" CssClass="stylePagingTextBox form-control form-control-sm"
                MaxLength="4" onpaste="return false;" onblur="return fnValidateEmptyGotoPage();"
                OnTextChanged="btnGO_Click" height="27px"></asp:TextBox>
                    <asp:Label runat="server" Text="Page Number" CssClass="navigation_label d-none" ID="lblPageNo"></asp:Label>
                    <span class="stylePagingRecFieldLabel">/ </span>
                    <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel" ID="lblTotalPages"></asp:Label>

                </div>
                <asp:LinkButton ID="btnNext" runat="server" Enabled="true" CommandName="Next" OnCommand="Navigation_Link" CausesValidation="false" CssClass="btn btn-outline-success btn-sm grid-pagination-btn mr-2"> Next <i class="fa fa-angle-right" aria-hidden="true"></i></asp:LinkButton>
                <asp:LinkButton ID="btnLast" runat="server" Enabled="true" CommandName="Last" OnCommand="Navigation_Link" CausesValidation="false" CssClass="btn btn-outline-success btn-sm grid-pagination-btn mr-2"> Last <i class="fa fa-step-forward" aria-hidden="true" style="font-size :11px"></i></asp:LinkButton>
                <asp:TextBox runat="server" CssClass="stylePagingTextBox stylePagingTextBox form-control form-control-sm"  MaxLength="3"
                OnTextChanged="btnSize_Click" onpaste="return false;" AutoPostBack="true" onblur="return fnValidateEmpty();"
                ID="txtPageSize"></asp:TextBox>
                <asp:Label runat="server" Text="Page Size" CssClass="navigation_label" ID="lblPageSize"></asp:Label>
                

            </div>
        </div>
        
    </div>
</div>
<input type="hidden" runat="server" style="width: 1px;" id="hdnTotRec" />
<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtGotoPage"
    FilterType="Numbers">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPageSize"
    FilterType="Numbers">
</cc1:FilteredTextBoxExtender>

<script type="text/javascript">

    function fnValidateGotoPage() {
        var iTotPage;
        var iGotoPage;
        iTotPage = document.getElementById('<%=lblTotalPages.ClientID%>').innerText;
        iGotoPage = document.getElementById('<%=txtGotoPage.ClientID%>').value;
        if (iGotoPage == 0) {
            alert('Go to page cannot be zero');
            document.getElementById('<%=txtGotoPage.ClientID%>').value = 1;
                return false;
            }
            if (parseInt(iTotPage) < parseInt(iGotoPage)) {
                alert('Go to page cannot be greater than total no of pages');
                document.getElementById('<%=txtGotoPage.ClientID%>').value = 1;
                return false;
            }
            return true;
        }

        function fnValidatePage() {
            var iTotRec;
            var iPageSize;
            iTotRec = document.getElementById('<%=hdnTotRec.ClientID%>').value;
            iPageSize = document.getElementById('<%=txtPageSize.ClientID%>').value;

            if (iPageSize == 0) {
                alert('Page size cannot be zero');
                document.getElementById('<%=txtPageSize.ClientID%>').value = 100;
            return false;
        }

            // user can enter only page size of 1 - 100
        if (parseInt(iPageSize) > 100) {
            alert('Page size cannot be greater than 100');
            document.getElementById('<%=txtPageSize.ClientID%>').value = 100;
        return true;
    }
    return true;
}
function fnValidateEmpty() {
    if (document.getElementById('<%=txtPageSize.ClientID%>').value == '' || document.getElementById('<%=txtPageSize.ClientID%>').value == 0) {
            document.getElementById('<%=txtPageSize.ClientID%>').value = 1;
            document.getElementById('<%=txtPageSize.ClientID%>').focus();
            alert('Page size cannot be empty or zero');
            return false;
        }
        return (fnValidatePage());
    }
    function fnValidateEmptyGotoPage() {
        if (document.getElementById('<%=txtGotoPage.ClientID%>').value == '' || document.getElementById('<%=txtGotoPage.ClientID%>').value == 0) {
        document.getElementById('<%=txtGotoPage.ClientID%>').value = 1;
        document.getElementById('<%=txtGotoPage.ClientID%>').focus();
        alert('Go to page cannot be empty or zero');
        return false;
    }
    return (fnValidateGotoPage());
}
</script>
