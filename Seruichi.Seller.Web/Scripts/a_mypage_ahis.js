
const _url = {};

$(function () {
    _url.GetAssHistory = common.appPath + '/a_mypage_ahis/GetD_AssReqProgressList';
    _url.InsertL_Log = common.appPath + '/a_mypage_ahis/InsertGetD_AssReqProgress_L_Log';
    let model = {
        SellerCD: null
    };
    
    GetAssHistoryData(model, this)
    common.addPager('#GetAssHistory', 10); 
    $('#mypage_ahis').addClass('active');  
    //if ($('#pager').children('li').eq(1))
    //    $('#pager').children('li').eq(1).addClass('active');

    if ($("tbody tr").length >= 10) {
        $('#pager').children('li').eq(1).addClass('active');

    }

    //class="active"
}); 
function GetAssHistoryData(model, $form) {
    return common.callAjaxWithLoadingSync(_url.GetAssHistory, model, this, function (result) {
        if (result && result.isOK) {
            Bind_tbody(result.data);
        }
        if (result && !result.isOK) {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    });
   
}
function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    //alert(data.length);
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var param = "'"+data[i]["AssReqID"] + "','" + data[i]["MansionName"]+"'";
            html +=  
            '<tr>\
            <td class= "text-truncate text-center"  > ' + data[i]["No"] + '</td>\
            <td class="text-truncate text-center" style="width:200px"> ' + data[i]["ID"] + ' </td >\
            <td class="text-truncate text-center" style="width:200px"> ' + data[i]["Status"] + ' </td>\
            <td class="text-start" style="width:500px">\
            <a onclick="setIdMansion('+ param+')" href="#" data-bs-toggle="modal"\
            data-bs-target="#exampleModal"\
            class="text-decoration-underline"> ' + data[i]["MansionName"] + ' </a>\
            <p class="p-0 m-0"><small class="text-nowrap w-100"  > ' + data[i]["Address"] + ' </small></p>\
            </td>\
            <td class="text-start text-nowrap"> ' + data[i]["REName"] + ' </td>\
            <td class="text-nowrap text-end" style="width:200px"> ' + data[i]["AssessAmount"] + ' </td>\
            <td class="text-nowrap" style="width:200px"> ' + data[i]["DeepAssDateTime"] + ' </td>\
            <td  style="display:none">'+ data[i]["IDT"] +'</td>\
            <td  style="display:none">'+ data[i]["StatusIndex"] + '</td>\
            </tr>'
        }
    }
    $('#GetAssHistory tbody').append(html);   
    sortTable.getSortingTable("GetAssHistory");
}
function a123() {
    
}
function setIdMansion(id,mansion) {
  
    $('#exampleModalLabel').text(mansion);
    $('#assessreqid').text(id);
}

$('#btn_assess').on('click', function () {
    l_logfunction('a_assess_d'); 
});
$('#btn_chat').on('click', function () {
    l_logfunction('a_chat');  
});

function l_logfunction(link) {
    let model = {
        LogDateTime: null,
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        SellerCD: null,
        Link:link
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = window.location.href.replace('a_mypage_ahis', '')  + link + "?AssReqID=" + $('#assessreqid').text();  
            } 
        });
}  
