
$(function () {

    //_url.CheckSellerName = common.appPath + '/t_Seller_list/CheckSellerName';
    //_url.CheckDate = common.appPath + '/t_Seller_list/CheckDate';
    //_url.DateCompare = common.appPath + '/t_Seller_list/DateCompare';

    setValidation();
    addEvents();
});

function setValidation() {

    $('#SellerName')
        .addvalidation_errorElement("#errorSeller")
        .addvalidation_singlebyte_doublebyte()
        .addvalidation_MaxLength(10);

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck()
        .addvalidation_datecompare();

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck()
        .addvalidation_datecompare();

    $('.myvalidcheck')
        .addvalidation_errorElement("#errorValidChkLenght")
        .addvalidation_checkboxlenght();

    $('.mystscheck')
        .addvalidation_errorElement("#errorStatusChkLenght")
        .addvalidation_checkboxlenght();

}
function addEvents() {

    common.bindValidationEvent('#form1', '');

    //const $locationSelect = $("#locationSelect").val(),$SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(),
    //    $startdate = $("#startdate").val(), $enddate = $("#enddate").val(), $ValidCheck = $("#ValidCheck").val(),
    //    $InValidCheck = $("#InValidCheck").val(), $StatusCheck1 = $("#StatusCheck1").val(), $StatusCheck2 = $("#StatusCheck2").val(),
    //    $StatusCheck3 = $("#StatusCheck3").val()

    //$('#SellerName').on('change', function () {
    //    const $this = $(this), $SellerName = $('#SellerName')
    //    let model = {
    //        SellerName: $SellerName.val()
    //    };
    //    common.callAjax(_url.CheckSellerName,model,
    //        function (result) {
    //            if (result && result.isOK) {
    //                $($email).hideError();
    //                const data = result.data;
    //            }
    //            if (result && !result.isOK) {
    //                const message = result.message;
    //                $this.showError(message.MessageText1);
    //            }
    //        });
    //});

   
    $('#btnToday').on('click', function () {
        let today = common.getToday();
        $('#StartDate').val(today);
        $('#EndDate').val(today);
    });

    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);

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

    $('.myvalidcheck').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($(".myvalidcheck:checked").length > 0) {
            $('.myvalidcheck').hideError();
        }
    }).change();

    $('.mystscheck').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.mystscheck').hideError();
        }
    }).change();

}

//function CompareDate() {
//    const $this = $(this), $enddate = $('#enddate'), $startdate = $('#startdate')
//    let model = {
//        startDate: $startdate.val(),
//        endDate: $enddate.val()
//    };

//    //alert(model.startDate);
//    //alert(model.endDate);
//    if (model.startDate > model.endDate) {
//        $this.showError(common.getMessage('E111'));
//        return false;
//    }

//    return true;
//}