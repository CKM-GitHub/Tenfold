
const _url = {};

$(function () {

    setValidation();
    _url.getM_SellerList = common.appPath + '/t_seller_list/GetM_SellerList';
    _url.generate_CSV = common.appPath + '/t_seller_list/Generate_CSV';
    _url.InsertL_Log = common.appPath + '/t_seller_list/InsertM_Seller_L_Log';
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_seller_list').addClass('font-bold text-underline');
});

function setValidation() {

    $('#SellerName')
        .addvalidation_errorElement("#errorSeller")
        .addvalidation_maxlengthCheck(10);

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck()
       // .addvalidation_datecompare();


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

    $('#StartDate,#EndDate').on('change', function () {
        Date_Compare();
    });
    
    
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
        Date_Compare();
    });

    $('#btnThisWeek').on('click', function () {
        let start = common.getDayaweekbeforetoday();
        let end = common.getToday();
        $('#StartDate').val(start);
        $('#EndDate').val(end);
        Date_Compare();
    });

    $('#btnThisMonth').on('click', function () {
        let today = common.getToday();
        let firstDay = common.getFirstDayofMonth();
        $('#StartDate').val(firstDay);
        $('#EndDate').val(today);
        Date_Compare();
    });

    $('#btnLastMonth').on('click', function () {
        let firstdaypremonth = common.getFirstDayofPreviousMonth();
        let lastdaypremonth = common.getLastDayofPreviousMonth();
        $('#StartDate').val(firstdaypremonth);
        $('#EndDate').val(lastdaypremonth);
        Date_Compare();

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
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#no_record').text("");
        $('#mansiontable tbody').empty();

        const $SellerName = $("#SellerName").val().trim(), $RangeSelect = $("#RangeSelect").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
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
        $form = $('#form1').hideChildErrors();

        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#no_record').text("");
        $('#mansiontable tbody').empty();

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
        $('#btnDisplay').hideError();
        getM_SellerList(model, $form);

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
                        ("0" + m.getHours()).slice(-2) + "" +
                        ("0" + m.getMinutes()).slice(-2) + "" +
                        ("0" + m.getSeconds()).slice(-2);
                    downloadLink.download = "売主リスト_" + dateString + ".csv";

                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
                else {
                    $('#site-error-modal').modal('show');
                }
            }
        )

    });

    $('#btnSignUp').on('click', function () {
        $form = $('#form1').hideChildErrors();

        //window.location.href = '/t_seller_new/Index';
        alert("Go to t_seller_new Form")
    });

    $('#btnSetting').on('click', function () {
        $form = $('#form1').hideChildErrors();

        //window.location.href = '/t_seller_billing/Index';
        alert("Go to t_seller_billing Form")
    });

}
function Date_Compare() {
        $start = $('#StartDate').val(), $end = $('#EndDate').val()
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate <= model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                return;
            }
            else {
                $("#StartDate").showError(common.getMessage('E111'));
                $("#EndDate").showError(common.getMessage('E111'));
                return;
            }
        }
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
function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}
function Bind_tbody(result) {
    let data = JSON.parse(result);
    let html = "";
    let _letter = "";
    let _class = "";
    let _class1 = "";
    let _sort_checkbox = "";
    let status = "";
    if (data.length > 0) {
    for (var i = 0; i < data.length; i++) {
        if (isEmptyOrSpaces(data[i]["ステータス"])) {
            _letter = "";
            _class = "ms-1 ps-1 pe-1 rounded-circle";
            _sort_checkbox = "";
            status = "";
        }
        else {
            _letter = data[i]["ステータス"].charAt(0);
            if (_letter == "見") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-primary text-white fst-normal";
                _sort_checkbox = "t_seller_list_one";
                status = "1";
            }
            else if (_letter == "交") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark fst-normal";
                _sort_checkbox = "t_seller_list_two";
                status = "2";
            }
            else if (_letter == "終") {
                _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary fst-normal";
                _sort_checkbox = "t_seller_list_three";
                status = "3";
            }

        }

        if (isEmptyOrSpaces(data[i]["無効会員"])) {
            _class1 = "";
        }
        else {
            _class1 = "fa fa-check";
        }

        html += '<tr>\
            <td class= "text-end" > ' + (i + 1) + '</td>\
            <td class="'+ _sort_checkbox + '"><i class="' + _class + '">' + _letter + '</i><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td >\
            <td class="d-none"> ' + status + ' </td>\
            <td class="text-center"><i class="' + _class1 + '">' + '</i> </td>\
            <td> ' + data[i]["売主CD"] + ' </td>\
            <td> <a class="text-heading font-semibold text-decoration-underline" id='+ data[i]["売主CD"] + ' href="#" onclick="l_logfunction(this.id)">' + data[i]["売主名"] + '</a></td>\
            <td> ' + data[i]["居住地"] + ' </td>\
            <td> ' + data[i]["登録日時"] + ' </td>\
            <td> ' + data[i]["査定依頼日時"] + ' </td >\
            <td> ' + data[i]["買取依頼日時"] + ' </td>\
            <td class="text-end"> '+ data[i]["登録数"] + ' </td>\
            <td class="text-end"> '+ data[i]["成約数"] + ' </td>\
            <td class="text-end d-none"> '+ data[i]["SellerKana"] + ' </td>\
            <td class="text-end d-none"> '+ data[i]["PrefCD"] + ' </td>\
            </tr>'
    }
    
        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
        $('#no_record').text("");
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
        $('#no_record').text("表示可能データがありません");
    }
    $('#mansiontable tbody').append(html);

    sortTable.getSortingTable("mansiontable");
}

function l_logfunction(id) {
    let model = {
        LogDateTime: null,
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
                window.location.href = common.appPath +'/t_seller_assessment/Index?SellerCD=' + model.SellerCD;
                //window.location.href = '/t_seller_assessment/Index';
                //alert("https://www.seruichi.com/t_seller_assessment?seller" + model.SellerCD);
            }
            if (result && !result.isOK) {

            }
        });
}