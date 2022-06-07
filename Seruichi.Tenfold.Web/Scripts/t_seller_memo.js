const _url = {};
$(function () {
    _url.get_t_seller_Info = common.appPath + '/t_seller_memo/get_t_seller_Info';
    _url.get_t_seller_memo_DisplayData = common.appPath + '/t_seller_memo/get_t_seller_memo_DisplayData';
    _url.Insert_L_Log = common.appPath + '/t_seller_memo/Insert_L_Log';
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_purchase').addClass('font-bold text-underline');
});

function addEvents() {
    $('#reale').addClass('d-none');
    $('#submenu_reale').addClass('d-none');
    Bind_Company_Data(this);         //Bind Company Info Data to the title part of the page

    var model = {
        RealECD: common.getUrlParameter('RealECD');
    }
    get_memo_Data(model, this);
}

function get_memo_Data(model, $form) {
    common.callAjaxWithLoadingSync(_url.get_t_seller_memo_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_memo_Data(result);
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