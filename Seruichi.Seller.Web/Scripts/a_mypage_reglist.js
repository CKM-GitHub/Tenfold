const _url = {};
$(function () {
    _url.get_displaydata = common.appPath + '/a_mypage_reglist/get_displaydata';
    addEvents();
});

function addEvents() {
    let model = {
        SellerID: null
    }
    getDisplayData(model, this);

}

function getDisplayData(model, $form) {
    common.callAjaxWithLoading(_url.get_displaydata, model, this, function (result) {

        if (result & result.isOk) {

            Bind_tbody(result.data);
        }
    }
    );
}
function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            html +=
            '<tr>\
             < td class="text-truncate text-center" >'+ data[i]["No"] + '</td >\
             <td class="text-end"> '+ data[i]["ステータス"]+ '</td>\
             < td class="text-start" >' + data[i]["査定ボタン"] + '</td >\
             <td class="text-end"> '+ data[i]["マンション名＆部屋番号"] + '</td>\
             < td class="text-start" >' + data[i]["登録日時"] + '</td >\
             <td class="text-end"> '+ data[i]["最終査定日時"] + '</td>\
             < td class="text-start" >' + data[i]["査定額"] + '</td >\
             < td class="text-start" >' + data[i]["不動産会社"] + '</td >\
            < /tr>'
        }
    }
    $("sellermansiontb tbody").append(html);
}



