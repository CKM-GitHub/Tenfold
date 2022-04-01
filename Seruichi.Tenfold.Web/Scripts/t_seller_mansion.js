const _url = {};

$(function () {    
    _url.getM_SellerMansionList = common.appPath + '/t_seller_mansion/GetM_SellerMansionList';  
    addEvents();
});
function addEvents()
{
    const $Chk_Blue = $("#Chk_Blue").val(), $Chk_Sky = $("#Chk_Sky").val(), $Chk_Orange = $("#Chk_Orange").val(),
        $Chk_Green = $("#Chk_Green").val(), $Chk_Brown = $("#Chk_Brown").val(), $Chk_Dark_Sky = $("#Chk_Dark_Sky").val(),
        $Chk_Gray = $("#Chk_Gray").val(), $Chk_Black = $("#Chk_Black").val(), $Chk_Pink = $("#Chk_Pink").val(),
        $MansionName = $("#MansionName").val(), $Range = $("#Range").val(), $StartDate = $("#StartDate").val(),
        $EndDate = $("#EndDate").val()

    let model = {
        Chk_Blue: $Chk_Blue,
        Chk_Sky: $Chk_Sky,
        Chk_Orange: $Chk_Orange,
        Chk_Green: $Chk_Green,
        Chk_Brown: $Chk_Brown,
        Chk_Dark_Sky: $Chk_Dark_Sky,
        Chk_Gray: $Chk_Gray,
        Chk_Black: $Chk_Black,
        Chk_Pink: $Chk_Pink,
        MansionName: $MansionName,
        Range: $Range,
        StartDate: $StartDate,
        EndDate: $EndDate,
    };
    common.callAjax(_url.getM_SellerMansionList, model,
        function (result) {
            if (result && result.isOK) {
                const data = result.data;
            }
            if (result && !result.isOK) {
                const message = result.message;
            }
        });
}