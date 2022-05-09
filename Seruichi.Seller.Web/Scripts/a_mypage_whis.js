const _url = {};

$(function () {
    _url.getSellerList = common.appPath + '/a_mypage_whis/GetD_SellerPossibleData';
    addEvents();
    
});


function addEvents() {
    let model = {
        SellerCD: null
    };
    getSellerList(model, this);
    common.addPager('#getSellerCD', 10);
    $('#mypage_whis').addClass('active');
}
function getSellerList(model, $form) {
    common.callAjaxWithLoadingSync(_url.getSellerList, model, this, function (result) {
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
//function isNullor(item) {
    
//        return item = (item == null) ? '' : item;
//    }

function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            html +=
               
                '<tr>\
            <td class= "text-truncate text-center" > ' + data[i]["No"] + '</td>\
            <td class="text-truncate text-center" > ' + data[i]["PossibleID"] + ' </td >\
            <td class="text-truncate text-center" > ' + data[i]["ChangeDateTime"] + ' </td>\
            <td class="text-end" > ' + data[i]["ChangeCount"] + ' </td >\
             <td class="text-end" > ' + data[i]["ChangeFee"] + ' </td >\
            <td class="text-truncate text-center" > ' + data[i]["PaymentFlg"] + ' </td >\
            </tr>'
        }
    }
    $('#getSellerCD tbody').append(html);

    sortTable.getSortingTable("getSellerCD");
}



