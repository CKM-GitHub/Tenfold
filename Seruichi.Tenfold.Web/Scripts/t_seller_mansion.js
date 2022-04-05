
const _url = {};

$(function () {
    setValidation();
    _url.getM_SellerMansionList = common.appPath + '/t_seller_mansion/GetM_SellerMansionList';  
    addEvents();
});
function setValidation() {
    $('.form-check-input')
        .addvalidation_errorElement("#errorChkLenght")
        .addvalidation_checkboxlenght();

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck()
        .addvalidation_datecompare();

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck()
        .addvalidation_datecompare();

    $('#btnProcess')
        .addvalidation_errorElement("#errorProcess");

}
function addEvents()
{
    common.bindValidationEvent('#form1', '');

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
    getM_SellerMansionList(model);
    //common.callAjax(_url.getM_SellerMansionList, model,
    //    function (result) {
    //        if (result && result.isOK) {
    //            const data = result.data;
    //            Bind_tbody(data);
    //        }
    //        if (result && !result.isOK) {
    //            const message = result.message;
    //        }
    //    });
   
    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#btnToday').on('click', function () {   
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
    });

    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
    });
    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);

    });

    $('#btnProcess').on('click', function () {
        $form = $('#form1').hideChildErrors();

        //if (!common.checkValidityOnSave('#form1')) {
        //    $form.getInvalidItems().get(0).focus();
        //    return false;
        //}
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerMansionList(model,this);
    });

}


function getM_SellerMansionList(model,selector) {   
    common.callAjaxWithLoading(_url.getM_SellerMansionList, model, selector, function (result) {
        if (result && result.isOK) {
            //sucess
            const data = result.data;
            Bind_tbody(data);
        }
        if (result && !result.isOK) {
            if (result.message != null) {
                const message = result.message;
                $selector.showError(message.MessageText1);
            }
            else {
                const errors = result.data;
                for (key in errors) {
                    const target = document.getElementById(key);
                    $(target).showError(errors[key]);
                    $form.getInvalidItems().get(0).focus();
                }
            }
        }
    });
}
function Bind_tbody(data) {
   
    for (var i = 0; i < data.count; i++) {
        $('.tbody_t_seller_Mansion_List').append(
            '<tr>\
            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
            <td><i class="ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal">未</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td> ' + data[i]["物件CD"] + ' </td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" href="#"> 物件名物件名物件名物件名物件名</a><p> <small class="text-wrap w-100">'+ data[i]["マンション名/住所"] + '</small></p></td>\
            <td> '+data[i]["部屋"]+'</td>\
            <td class="text-end">'+data[i]["階数(階)"]+'</td>\
            <td class="text-end">'+data[i]["面積(㎡)"]+'</td>\
            <th class="text-decoration-underline text-nowrap">'+data[i]["売主名"]+'</th>\
            <td class="text-nowrap"> '+data[i]["登録日時"]+'</td>\
            <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#"> '+ data[i]["詳細査定日時"] +' </a> </td>\
            <td class="text-nowrap"> '+ data[i]["買取依頼日時"] +' </td>\
            <td class="text-nowrap">\
            <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a>\
            <p class="text-end">'+ data[i]["マンション TOP1/金額"] +'</p>\
             </td>\
             <td class="text-nowrap">\
             <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a>\
             <p class="text-end">'+data[i]["エリア TOP1/金額"]+'</p>\
             </td>\
             <td class="text-nowrap"> <a class="text-heading font-semibold text-decoration-underline" href="#"> 不動産会社名不動産会社名 </a><p class="text-end">'+data[i]["買取依頼会社/金額"]+'</p> </td>\
             <td class="text-nowrap"> '+data[i]["送客日時"]+' </td>\
             <td class="text-nowrap"> '+data[i]["成約日時"]+' </td>\
             <td class="text-nowrap"> '+data[i]["辞退日時"]+'</td>\
            </tr>'
        )
    }
}
