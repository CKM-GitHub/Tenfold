const _url = {};
$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_orderdetails/get_t_seller_Info';
    _url.get_D_SellerPossibleData = common.appPath + '/t_seller_orderdetails/get_D_SellerPossibleData';
    addEvents()
    $('#subMenu_Seller li').children('a').removeClass("active");
    $('#subMenu_Seller li').children('a').eq(3).addClass('active');
});

function addEvents() {
    let model = {
        SellerCD: common.getUrlParameter('SellerCD')
    }
    debugger;
    get_account_Data(model, this);

    getSellerList(model, this);
}

function get_account_Data(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_Info, model, this, function (result) {
        if (result && result.isOK) {
           
        }
        else {
            const errors = result.data;
            for (key in errors) {
                const target = document.getElementById(key);
                $(target).showError(errors[key]);
                $form.getInvalidItems().get(0).focus();
            }
        }
    })
}


function getSellerList(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_D_SellerPossibleData, model, this, function (result) {
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
}
