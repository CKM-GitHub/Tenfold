
const _url = {};

$(function () {

    setValidation();
    _url.getM_SellerList = common.appPath + '/t_seller_list/GetM_SellerList';
    _url.generate_CSV = common.appPath + '/t_seller_list/Generate_CSV';
    _url.InsertL_Log = common.appPath + '/t_seller_list/InsertM_Seller_L_Log';
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

    const $SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
        $startdate = $("#StartDate").val(), $enddate = $("#EndDate").val(), $ValidCheck = $("#ValidCheck").val(),
        $InValidCheck = $("#InValidCheck").val(), $expectedCheck = $("#expectedCheck").val(), $negtiatioinsCheck = $("#negtiatioinsCheck").val(),
        $endCheck = $("#endCheck").val()

    let model = {
        PrefNameSelect: $PrefNameSelect,
        SellerName: $SellerName,
        RangeSelect: $RangeSelect,
        StartDate: $startdate,
        EndDate: $enddate,
        ValidCheck: $ValidCheck,
        InValidCheck: $InValidCheck,
        expectedCheck: $expectedCheck,
        negtiatioinsCheck: $negtiatioinsCheck,
        endCheck: $endCheck,
    };
    getM_SellerList(model, this);

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

        const $SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
            $startdate = $("#StartDate").val(), $enddate = $("#EndDate").val(), $ValidCheck = $("#ValidCheck").val(),
            $InValidCheck = $("#InValidCheck").val(), $expectedCheck = $("#expectedCheck").val(), $negtiatioinsCheck = $("#negtiatioinsCheck").val(),
            $endCheck = $("#endCheck").val()

        let model = {
            PrefNameSelect: $PrefNameSelect,
            SellerName: $SellerName,
            RangeSelect: $RangeSelect,
            StartDate: $startdate,
            EndDate: $enddate,
            ValidCheck: $ValidCheck,
            InValidCheck: $InValidCheck,
            expectedCheck: $expectedCheck,
            negtiatioinsCheck: $negtiatioinsCheck,
            endCheck: $endCheck,
        };
        getM_SellerList(model, $form);

    });

    $('#btnCSV').on('click', function () {
        const $SellerName = $("#SellerName").val(), $RangeSelect = $("#RangeSelect").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
            $startdate = $("#StartDate").val(), $enddate = $("#EndDate").val(), $ValidCheck = $("#ValidCheck").val(),
            $InValidCheck = $("#InValidCheck").val(), $expectedCheck = $("#expectedCheck").val(), $negtiatioinsCheck = $("#negtiatioinsCheck").val(),
            $endCheck = $("#endCheck").val()

        let model = {
            PrefNameSelect: $PrefNameSelect,
            SellerName: $SellerName,
            RangeSelect: $RangeSelect,
            StartDate: $startdate,
            EndDate: $enddate,
            ValidCheck: $ValidCheck,
            InValidCheck: $InValidCheck,
            expectedCheck: $expectedCheck,
            negtiatioinsCheck: $negtiatioinsCheck,
            endCheck: $endCheck,
        };
        common.callAjax(_url.generate_CSV, model,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    var downloadLink = document.createElement("a");
                    var blob = new Blob(["\ufeff", csv]);
                    var url = URL.createObjectURL(blob);
                    downloadLink.href = url;
                    let m = new Date();
                    var dateString =
                        m.getUTCFullYear() + "" +
                        ("0" + (m.getUTCMonth() + 1)).slice(-2) + "" +
                        ("0" + m.getUTCDate()).slice(-2) + "_" +
                        ("0" + m.getUTCHours()).slice(-2) + "" +
                        ("0" + m.getUTCMinutes()).slice(-2) + "" +
                        ("0" + m.getUTCSeconds()).slice(-2);
                    downloadLink.download = "売主リスト" + dateString+".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    alert("There is no data!");
                }
            }
        )

    });
    
    $('#btnSignUp').on('click', function () {
        $form = $('#form1').hideChildErrors();

        //window.location.href = '/t_seller_new/Index';
    });

    $('#btnSetting').on('click', function () {
        $form = $('#form1').hideChildErrors();

        //window.location.href = '/t_seller_billing/Index';
    });

}
function getM_SellerList(model, $form) {
    common.callAjaxWithLoading(_url.getM_SellerList, model, this, function (result) {
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
    for (var i = 0; i < data.length; i++) {
        html += '<tr>\
            <td class= "text-end" > ' + data[i]["NO"] + '</td>\
            <td><i class="ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal fst-normal">未</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td class="text-center"> ' + data[i]["無効会員"] + ' </td>\
            <td> ' + data[i]["売主CD"] + ' </td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" id='+ data[i]["売主CD"] + ' href="#" onclick="l_logfunction(this.id)">売主名売主名売主名売主名売主名</a><p> <small class="text-wrap w-100">' + data[i]["売主名"] + '</small></p></td>\
            <td> ' + data[i]["居住地"] + ' </td>\
            <td> ' + data[i]["登録日時"] + ' </td>\
            <td> ' + data[i]["査定依頼日時"] + ' </td >\
            <td> ' + data[i]["買取依頼日時"] + ' </td>\
            <td class="text-end"> '+ data[i]["登録数"] + ' </td>\
            <td class="text-end"> '+ data[i]["成約数"] + ' </td>\
            </tr>'
    }
    $('#mansiontable tbody').append(html);
}

function l_logfunction(id) {
    let model = {
        LoginKBN: null,
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: null,
        Processing: null,
        Remarks: null,
        SellerCD: id
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                //window.location.href = '/t_seller_assessment/Index';
            }
            if (result && !result.isOK) {

            }
        });
}