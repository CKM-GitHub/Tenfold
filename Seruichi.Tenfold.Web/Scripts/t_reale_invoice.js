const _url = {};
$(function () {
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_invoice/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_invoice/get_t_reale_CompanyCountingInfo';
    _url.get_t_reale_memo_DisplayData = common.appPath + '/t_reale_invoice/get_t_reale_invoice_DisplayData';
    //_url.Insert_L_Log = common.appPath + '/t_reale_memo/Insert_L_Log';
    setValidation();
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_invoice').addClass('font-bold text-underline');
});

function setValidation()
{


}

function addEvents()
{

}