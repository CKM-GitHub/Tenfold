const _url = {};

$(function () {
    _url.GetAssHistory = common.appPath + '/a_mypage_ahis/GetD_AssReqProgressList';
    let model = {
        SellerCD: null
    };
    GetAssHistoryData(model, this);
});

function GetAssHistoryData(model, $form) {
    common.callAjaxWithLoading(_url.GetAssHistory, model, this, function (result) {
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
    let _letter = "";
    let _class = "";
    let _sort_checkbox = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            html += '<tr>\
            <td class= "text-truncate text-center" > ' + data[i]["No"] + '</td>\
            <td class="text-truncate text-center"> ' + data[i]["ID"] + ' </td >\
            <td class="text-truncate text-center"> ' + data[i]["Status"] + ' </td>\
            <td class="text-start">\
            <a href="#" data-bs-toggle="modal"\
            data-bs-target="#exampleModal"\
            class="text-decoration-underline"> ' + data[i]["Status"] + ' </a>\
            <p class="p-0 m-0"><small class="text-wrap w-100"> ' + data[i]["Address"] + ' </small></p>\
            </td>\
            <td class="text-start"> ' + data[i]["REName"] + ' </td>\
            <td class="text-nowrap"> ' + data[i]["AssessAmount"] + ' </td>\
            <td class="text-nowrap"> ' + data[i]["DeepAssDateTime"] + ' </td>\
            </tr>'
        }
    }
    $('#GetAssHistory tbody').append(html);
}

