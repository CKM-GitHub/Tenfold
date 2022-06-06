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
    let model = {
        SellerCD: common.getUrlParameter('SellerCD')
    };

    Bind_Company_Data(model, this);         //Bind Company Info Data to the title part of the page

    //get_purchase_Data(model, this, 'page_load');

}