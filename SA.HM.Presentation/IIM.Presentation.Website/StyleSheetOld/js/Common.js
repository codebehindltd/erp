var viewportwidth;
var viewportheight;
var popupDivID;

$(document).ready(function () {
    viewportheight = $(document).height();
    viewportwidth = $(document).width();
    //if ($(hidProcedureId).val() == "")
    $("#popUpDiv").css({ 'display': 'none' });
    //    else {
    //        $(hidProcedureId).val("");
    //        popup(1, 'DivAddProcedure', 'Edit Procedure', 500, 250);
    //    }
});

$(window).resize(function () {

    viewportheight = $(document).height();
    viewportwidth = $(document).width();
});

function popup(flag, divID, title, width, height, msgType, msg) {
    if (!$('#popUpDiv'))
        return false;

    if (flag == 1) {
        $("#popUpDivContainer").width(width);
        popupDivID = divID;
        $('#popUpDiv').css({ height: height + 'px' });
        $('#popUpDiv').css({ width: width + 'px' });

        if (title == "Add Location" || title == "Edit Location")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32house.png'/></div>" + title;
        else if (title == "Add Sub Location" || title == "Edit Sub Location")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32sublocation.png'/></div>" + title;
        else if (title == "Add Temperature" || title == "Edit Temperature")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32temperature.png'/></div>" + title;
        else if (title == "Add General Object" || title == "Edit General Object")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32cubes_yellow.png'/></div>" + title;
        else if (title == "Add Glass Plastic" || title == "Edit Glass Plastic")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32jar.png'/></div>" + title;
        else if (title == "Add Metal Item" || title == "Edit Metal Item")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32metal.png'/></div>" + title;
        else if (title == "Add Probe" || title == "Edit Probe")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/probes32.png'/></div>" + title;
        else if (title == "Add Scale" || title == "Edit Scale")
            title = "<div class='iconDiv'><img style='border-width: 0pt;' alt='Location' src='Images/32Reportweight.png'/></div>" + title;

        $(".popupTitle").html(title);
        $(".popupContentBox").append($("#" + popupDivID));
        $("#" + popupDivID).css({ 'display': 'block' });

        $('#layer1').css({ height: viewportheight + 'px' });
        $('#layer1').css({ width: viewportwidth + 'px' });
        $("#layer1").css({ 'display': 'block' });
        $("#popUpDiv").css({ 'display': 'block' });


        var contentDivHeight = height - 10;

        if ($(".popupHeaderBox").attr('class') == 'popupHeaderBox') {
            contentDivHeight = contentDivHeight - $(".popupHeaderBox").height() - 10;
        }

        if ($(".popupActionPanel").attr('class') == 'popupActionPanel') {
            contentDivHeight = contentDivHeight - $(".popupActionPanel").height() - 3;
        }

        $('.popupContentBox').css({ height: contentDivHeight + 'px' });

        contentDivHeight = contentDivHeight - 20;
        $("#" + popupDivID).css({ height: contentDivHeight + 'px' });

        var popupTopPosition = $(window).height() / 2 - $("#popUpDiv").height() / 2 + $(window).scrollTop();
        var popupLeftPosition = $(window).width() / 2 - $("#popUpDiv").width() / 2;

        $('#popUpDiv').css({ top: popupTopPosition + 'px' });
        $('#popUpDiv').css({ left: popupLeftPosition + 'px' });

        if (msgType) {
            $("#" + popupDivID + " div.msg").attr("class", "msg " + msgType);
            $("#" + popupDivID + " div.msg").html(msg);
            $("#" + popupDivID + " div.msg").show();
        }
    }
    else {

        $("#layer1").css({ 'display': 'none' });
        $("#popUpDiv").css({ 'display': 'none' });
        $("#" + popupDivID).css({ 'display': 'none' });
    }
}


function showHideProperties(ctrl) {
    if (ctrl.value == "Yes/No") {
        if (document.all) {
            document.getElementById("trMandatory").style.display = "block";
            document.getElementById("trDefaultValue").style.display = "block";
            document.getElementById("trAction").style.display = "block";
        }
        else {
            document.getElementById("trMandatory").style.display = "table-row";
            document.getElementById("trDefaultValue").style.display = "table-row";
            document.getElementById("trAction").style.display = "table-row";
        }

    }
    else if (ctrl.value == "Barcode Scan") {
        if (document.all) {
            document.getElementById("trMandatory").style.display = "block";
        }
        else {
            document.getElementById("trMandatory").style.display = "table-row";
        }
        document.getElementById("trDefaultValue").style.display = "none";
        document.getElementById("trAction").style.display = "none";
    }
    else {
        document.getElementById("trMandatory").style.display = "none";
        document.getElementById("trDefaultValue").style.display = "none";
        document.getElementById("trAction").style.display = "none";
    }
}

function showProperties(stepId, ctrl) {
    //ctrl.class = "selectedRow";
    var table = document.getElementById("ctl00_cpBody_gvStep");
    var trs = table.getElementsByTagName("tr");
    for (var i = 1; i < trs.length; i++) {
        if (i % 2 == 0)
            trs[i].className = "odd";
        else
            trs[i].className = "even";
    }
    ctrl.setAttribute("class", "selectedRow");
    PageMethods.GetStepProperties(stepId, onSuccessStepProperties, onFailureStepProperties);
}
function onSuccessStepProperties(response) {

    var step = eval(response);

    var htmlBuilder = new Sys.StringBuilder();

    htmlBuilder.append("<table cellpadding='5' border='0'><tr><td colspan='2' class='stepHeader'>STEP PROPERTIES</td></tr>");
    htmlBuilder.append(String.format("<table cellpadding='5' border='0'><tr><td class='tblHeader'>Type</td><td class='odd'>{0}</td></tr>", step.Type));
    htmlBuilder.append(String.format("<tr><td class='tblHeader'>Description</td><td class='even'>{0}</td></tr>", step.Description));
    htmlBuilder.append(String.format("<tr><td class='tblHeader'>PDA Message</td><td class='odd'>{0}</td></tr>", step.PDAMessage));
    htmlBuilder.append(String.format("<tr><td class='tblHeader'>Mandatory</td><td class='even'>{0}</td></tr>", (step.Mandatory) ? "Y" : "N"));
    if (step.Type == "Yes/No") {
        htmlBuilder.append(String.format("<tr><td class='tblHeader'>Default Value</td><td class='odd'>{0}</td></tr>", step.DefaultValue));
        htmlBuilder.append(String.format("<tr><td class='tblHeader'>Action</td><td class='even'>{0}</td></tr>", step.Action));
    }
    htmlBuilder.append("</table>");

    //alert(document.getElementById("stepProperties"))
    document.getElementById("stepProperties").innerHTML = htmlBuilder.toString();

    $(txtNumber).val(step.Number);
    $(ddlType).val(step.Type);
    $(txtStepDescription).val(step.Description);
    $(txtMessage).val(step.PDAMessage);
    $(ddlMandatory).val(step.Mandatory);
    $(ddlDefaultValue).val(step.DefaultValue);
    $(txtAction).val(step.Action);
    $(txtStepId).val(step.StepID)
    $(txtProcedureId).val(step.ProcedureID)
    showHideProperties(document.getElementById(ddlType.replace("#", "")));
}

function onFailureStepProperties() {

}

function deleteProcedure(procedureId, procedureName) {
    var isDelete = confirm("Do you want to delete \"" + procedureName + "\"procedure?");
    if (isDelete) {
        location.href = "AddStep.aspx";
        PageMethods.DeleteProcedure(procedureId, onSuccessDeleteProcedure, onFailureDeleteProcedure);
    }
}

function onSuccessDeleteProcedure(response) {
    location.href = "ProcedureListing.aspx";
}

function onFailureDeleteProcedure(response) {
    location.href = "ProcedureListing.aspx";
}

function deleteStep(stepId, stepName) {
    var isDelete = confirm("Do you want to delete \"" + stepName + "\"step?");
    if (isDelete) {
        PageMethods.DeleteStep(stepId, onSuccessDeleteStep, onFailureDeleteStep);
    }
}

function onSuccessDeleteStep(response) {
    location.href = "ProcedureListing.aspx";
}

function onFailureDeleteStep(response) {
    location.href = "ProcedureListing.aspx";
}

function editProcedure(procedureId) {
    $(hidProcedureId).val(procedureId);
    PageMethods.GetProcedureById(procedureId, onSuccessGetProcedure, onFailureGetProcedure);
}

function onSuccessGetProcedure(response) {

    var procedure = eval(response);

    $(txtName).val(procedure.ProcedureName)
    $(txtDescription).val(procedure.Description)
    $(ddlFrequency).val(procedure.Frequency)
    $(txtProcedureId).val(procedure.ProcedureID)
    popup(1, 'DivAddProcedure', 'Edit Procedure', 500, 250);
}

function onFailureGetProcedure() {
}
function editStep(stepId, stepNo) {
    $(lblStepNo).text(stepNo);
    popup(1, 'DivAddStep', 'Edit Step', 500, 350);
    //PageMethods.GetProcedureById(procedureId, onSuccessGetProcedure, onFailureGetProcedure);
}

function addStep(procedureId, stepNo) {
    clearStepSelection();
    $(lblStepNo).text(stepNo);
    popup(1, 'DivAddStep', 'Add Step', 500, 350);
}

function clearProcedureSelection() {
    $(txtName).val("");
    $(txtDescription).val("");
    $(ddlFrequency).val("");
    $(txtProcedureId).val("");
}

function clearStepSelection() {
    $(txtNumber).val("");
    $(ddlType).val("");
    $(txtStepDescription).val("");
    $(txtMessage).val("");
    $(ddlMandatory).val("");
    $(ddlDefaultValue).val("");
    $(txtAction).val("");
    $(txtStepId).val("");
}
//function onSuccessGetProcedure(response) {

//    var procedure = eval(response);

//    $(txtName).val(procedure.ProcedureName)
//    $(txtDescription).val(procedure.Description)
//    $(ddlFrequency).val(procedure.Frequency)
//    popup(1, 'DivAddProcedure', 'Edit Procedure', 500, 250);
//}

//function onFailureGetProcedure() {
//}

function editObject() {
    var objectId = $(txtLocationId).val();
    alert(objectId)
    PageMethods.GetObjectById(objectId, onSuccessGetObject, onFailureGetObject);
    return false;
}
function onSuccessGetObject(response) {

    var obj = eval(response);

    var htmlBuilder = new Sys.StringBuilder();
    alert(obj)
    //    htmlBuilder.append("<table cellpadding='5' border='0'><tr><td colspan='2' class='stepHeader'>STEP PROPERTIES</td></tr>");
    //    htmlBuilder.append(String.format("<table cellpadding='5' border='0'><tr><td class='tblHeader'>Type</td><td class='odd'>{0}</td></tr>", step.Type));
    //    htmlBuilder.append(String.format("<tr><td class='tblHeader'>Description</td><td class='even'>{0}</td></tr>", step.Description));
    //    htmlBuilder.append(String.format("<tr><td class='tblHeader'>PDA Message</td><td class='odd'>{0}</td></tr>", step.PDAMessage));
    //    htmlBuilder.append(String.format("<tr><td class='tblHeader'>Mandatory</td><td class='even'>{0}</td></tr>", (step.Mandatory) ? "Y" : "N"));
    //    if (step.Type == "Yes/No") {
    //        htmlBuilder.append(String.format("<tr><td class='tblHeader'>Default Value</td><td class='odd'>{0}</td></tr>", step.DefaultValue));
    //        htmlBuilder.append(String.format("<tr><td class='tblHeader'>Action</td><td class='even'>{0}</td></tr>", step.Action));
    //    }
    //    htmlBuilder.append("</table>");

    //    //alert(document.getElementById("stepProperties"))
    //    document.getElementById("stepProperties").innerHTML = htmlBuilder.toString();

    //    $(txtNumber).val(step.Number);
    //    $(ddlType).val(step.Type);
    //    $(txtStepDescription).val(step.Description);
    //    $(txtMessage).val(step.PDAMessage);
    //    $(ddlMandatory).val(step.Mandatory);
    //    $(ddlDefaultValue).val(step.DefaultValue);
    //    $(txtAction).val(step.Action);
    //    $(txtStepId).val(step.StepID)
    //    $(txtProcedureId).val(step.ProcedureID)
    //    showHideProperties(document.getElementById(ddlType.replace("#", "")));
}

function onFailureGetObject() {

}

//function ClearAllChildren(element) {
//    for (var i = 0; i < element.childNodes.length; i++) {
//        var e = element.childNodes[i];
//        if (e.tagName) switch (e.tagName.toLowerCase()) {
//    case 'input':
//        switch (e.type) {
//            case "radio": e.checked = false; break;
//            case "checkbox": e.checked = false; break;
//            case "button":
//            case "submit":
//            case "image": break;
//            default: e.value = ''; break;
//        }
//        break;
//            case 'select': e.selectedIndex = 0; break;
//            case 'textarea': e.innerHTML = ''; break;
//            default: clearChildren(e);
//        }
//    }
//}

function ClearAllChildren(element) {
    $("#" + element + " input[type=text]").val('');
    $("#" + element + " input[type=hidden]").val('');
    $("#" + element + " textarea").val('');
    $("#" + element + " select").val('');
    $("#" + element + " input[type=radio]").each(function () {
        $(this).attr('checked', false);
    });
    $("#" + element + " input[type=checkbox]").each(function () {
        $(this).attr('checked', false);
    });
}

function HtmlEncode(value) {
    return $('<div/>').text(value).html();
}

function HtmlDecode(value) {
    return $('<div/>').html(value).text();
}

function PrintOptionForReportViewerInMozillaChrome(reportControlId) {
   
    if (($.browser.mozilla || $.browser.chrome) && !$("#ff_print").length) {

        var ControlName = "ctl00_ContentPlaceHolder1_rvTransaction_ctl06";
        var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=10.0.30319.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
        var innerTable = '<table title="Print" onclick="PrintFunc(\'' + ControlName + '\'); return false;" id="ff_print" style="cursor: default;">' + innerTbody + '</table>'
        var outerDiv = '<div style="display: inline; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

        $("#" + ControlName + " > div").append(outerDiv);
    }
}

function PrintFunc(ControlName) {

    var strFrameName = ("printer-" + (new Date()).getTime());
    var jFrame = $("<iframe name='" + strFrameName + "'>");
    jFrame
        .css("width", "1px")
        .css("height", "1px")
        .css("position", "absolute")
        .css("left", "-2000px")
        .appendTo($("body:first"));

    var objFrame = window.frames[strFrameName];
    var objDoc = objFrame.document;
    var jStyleDiv = $("<div>").append($("style").clone());

    objDoc.open();
    objDoc.write($("head").html());
    objDoc.write($("#ctl00_ContentPlaceHolder1_rvTransaction_fixedTable tbody > tr:nth-child(4) td:eq(2)").html());
    objDoc.close();
    objFrame.print();

    setTimeout(function () { jFrame.remove(); }, (60 * 1000));

}