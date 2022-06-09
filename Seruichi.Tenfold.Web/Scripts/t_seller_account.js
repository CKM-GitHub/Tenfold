const _url = {};
$(function () {
    _url.GetM_SellerBy_SellerCD = common.appPath + '/t_seller_account/GetM_SellerBy_SellerCD';
    addEvents()
});

function addEvents() {


    $('#reale').addClass('d-none');
    $('#submenu_reale').addClass('d-none');
    debugger;
   
    Bind_Company_Data(this);         //Bind Company Info Data to the title part of the page

    
    //sortTable.getSortingTable("mansiontable");
}




