//const _url = {};
$(function () {

    //_url.CheckSellerName = common.appPath + '/t_Seller_list/CheckSellerName';
    //_url.CheckDate = common.appPath + '/t_Seller_list/CheckDate';
    //_url.DateCompare = common.appPath + '/t_Seller_list/DateCompare';

    setValidation();
    //_url.getM_SellerList = common.appPath + '/t_seller_list/GetM_SellerList';
    addEvents();
});

function setValidation() {

    $('#SellerName')
        .addvalidation_errorElement("#errorSeller")
        .addvalidation_MaxLengthforSellerlist(10);    

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

    const $SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(),
        $startdate = $("#StartDate").val(), $enddate = $("#EndDate").val(), $ValidCheck = $("#ValidCheck").val(),
        $InValidCheck = $("#InValidCheck").val(), $StatusCheck1 = $("#StatusCheck1").val(), $StatusCheck2 = $("#StatusCheck2").val(),
        $StatusCheck3 = $("#StatusCheck3").val()

    //let model = {
    //    SellerName: $SellerName,
    //    RangeSelect: $RangeSelect,
    //    StartDate: $startdate,
    //    EndDate: $enddate,
    //    ValidCheck: $ValidCheck,
    //    InValidCheck: $InValidCheck,
    //    expectedCheck: $StatusCheck1,
    //    negtiatioinsCheck: $StatusCheck2,
    //    endCheck: $StatusCheck3,
    //};
    //common.callAjax(_url.getM_SellerList, model,
    //    function (result) {
    //        if (result && result.isOK) {
    //            const data = result.data;
    //            Bind_tbody(data);
    //        }
    //        if (result && !result.isOK) {
    //            const message = result.message;
    //        }
    //    });


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
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.myvalidcheck').hideError();
        }
    }).change();

    $('.mystscheck').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.mystscheck').hideError();
        }
    }).change();

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();

        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        getM_SellerList(model, this);

    });

}

function getM_SellerList(model, selector) {
    common.callAjaxWithLoading(_url.getM_SellerList, model, selector, function (result) {
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
            <td class="text-center"> ' + data[i]["無効会員"] + ' </td>\
            < td> ' + data[i]["売主CD"] + ' </td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" href="#">' + data[i]["売主名"] + '</a> </td>\
            <td> ' + data[i]["居住地"] + ' </td>\
            <td> ' + data[i]["登録日時"] + ' </td>\
            <td> ' + data[i]["査定依頼日時"] + ' </td >\
            <td> ' + data[i]["買取依頼日時"] + ' </td>\
            <td class="text-end"> '+ data[i]["登録数"] + ' </td>\
            <td class="text-end"> '+ data[i]["成約数"] + ' </td>\
            </tr>'
        )
    }
}