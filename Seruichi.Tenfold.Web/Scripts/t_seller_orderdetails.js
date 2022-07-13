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
    getSellerList(model, this);
    sortTable.getSortingTable("sellertable");

    get_account_Data(model, this);

    
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
function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}
function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {


            if (isEmptyOrSpaces(data[i]["PaymentFlg"])) {
                _letter = "";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
                _sort_checkbox = "";
            }
            else {
                _letter = data[i]["PaymentFlg"].charAt(0);
                if (_letter == "完") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info text-white fst-normal";
                    _sort_checkbox = "1";
                }
                else if (_letter == "失") {
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-danger text-white fst-normal";
                    _sort_checkbox = "2";
                }
               

            }
            
            html +=
                '<tr>\
            <td class="text-end" > ' + data[i]["No"] + '</td>\
            <td > ' + data[i]["PossibleID"] + ' </td >\
            <td  > ' + data[i]["ChangeDateTime"] + ' </td>\
            <td class="text-end" > ' + data[i]["ChangeCount"] + ' </td >\
            <td class="text-end" > ' + data[i]["ChangeFee"] + ' </td >\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold"> ' + data[i]["PaymentFlg"] + '</span></td>\
            <td class="d-none">'+ _sort_checkbox + '</td>\
            </tr>'
        }
    }
    $('#sellertable tbody').append(html);
}
