
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
            html +=  
            '<tr>\
            <td class= "text-truncate text-center" > ' + data[i]["No"] + '</td>\
            <td class="text-truncate text-center" style="width:200px"> ' + data[i]["ID"] + ' </td >\
            <td class="text-truncate text-center" style="width:200px"> ' + data[i]["Status"] + ' </td>\
            <td class="text-start" style="width:500px">\
            <a onclick="setId('+ data[i]["AssReqID"] +')" href="#" data-bs-toggle="modal"\
            data-bs-target="#exampleModal"\
            class="text-decoration-underline"> ' + data[i]["Status"] + ' </a>\
            <p class="p-0 m-0"><small class="text-nowrap w-100"> ' + data[i]["Address"] + ' </small></p>\
            </td>\
            <td class="text-start text-nowrap"> ' + data[i]["REName"] + ' </td>\
            <td class="text-nowrap text-end" style="width:200px"> ' + data[i]["AssessAmount"] + ' </td>\
            <td class="text-nowrap" style="width:200px"> ' + data[i]["DeepAssDateTime"] + ' </td>\
            </tr>'
        }
    }
    $('#GetAssHistory tbody').append(html);   
} 
$('#btn_assess').on('click', function () {
    l_logfunction('a_assess_d'); 
});
$('#btn_chat').on('click', function () {
    l_logfunction('a_chat');  
});
function setId(id) { 
    $('#assessreqid').text(id)
}
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
                window.location.href = window.location.protocol + "//" + window.location.host + "/" + link + "?AssReqID=" + $('#assessreqid').text();  
            } 
        });
}  
