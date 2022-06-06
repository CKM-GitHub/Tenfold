const _url = {};
$(function () {
    setValidation();
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_purchase/get_t_reale_CompanyCountingInfo';
    _url.get_t_reale_purchase_DisplayData = common.appPath + '/t_reale_purchase/get_t_reale_purchase_DisplayData';
    _url.get_t_reale_purchase_CSVData = common.appPath + '/t_reale_purchase/get_t_reale_purchase_CSVData';
    _url.Insert_L_Log = common.appPath + '/t_reale_purchase/Insert_L_Log';
    _url.get_Modal_HomeData = common.appPath + '/t_reale_purchase/get_Modal_HomeData';
    _url.get_Modal_ProfileData = common.appPath + '/t_reale_purchase/get_Modal_ProfileData';
    _url.get_Modal_ContactData = common.appPath + '/t_reale_purchase/get_Modal_ContactData';
    _url.get_Modal_DetailData = common.appPath + '/t_reale_purchase/get_Modal_DetailData';
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_purchase').addClass('font-bold text-underline');
});

function setValidation() {
    $('.form-check-input')
        .addvalidation_errorElement("#CheckBoxError")
        .addvalidation_checkboxlenght(); //E112

    $('#StartDate')
        .addvalidation_errorElement("#errorStartDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111

    $('#EndDate')
        .addvalidation_errorElement("#errorEndDate")
        .addvalidation_datecheck() //E108
        .addvalidation_datecompare(); //E111
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('.form-check-input').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.form-check-input').hideError();
        }
    }).change();

    $('#StartDate, #EndDate').on('change', function () {
        const $this = $(this), $start = $('#StartDate').val(), $end = $('#EndDate').val();
        if (!common.checkValidityInput($this)) {
            return false;
        }
        let model = {
            StartDate: $start,
            EndDate: $end
        };
        if (model.StartDate && model.EndDate) {
            if (model.StartDate < model.EndDate) {
                $("#StartDate").hideError();
                $("#EndDate").hideError();
                $("#EndDate").focus();
                return;
            }
        }
    });

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

    let model = {
        RealECD: common.getUrlParameter('RealECD'),
        chk_Purchase: $("#chk_Purchase").val(),
        chk_Checking: $("#chk_Checking").val(),
        chk_Nego: $("#chk_Nego").val(),
        chk_Contract: $("#chk_Contract").val(),
        chk_SellerDeclined: $("#chk_SellerDeclined").val(),
        chk_BuyerDeclined: $("#chk_BuyerDeclined").val(),
        Range: $("#Range").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val()
    };
    $('#RealECD').val(model.RealECD);
    Bind_Company_Data(model, this);         //Bind Company Info Data to the title part of the page

    get_purchase_Data(model, this, 'page_load');

    sortTable.getSortingTable("tblPurchaseDetails");

    $('#btnDisplay').on('click', function () {
        $('#RealEName').val($('#r_REName').text());
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_purchase_Data(model, $form, 'Display');
    });

    $('#btnCSV').on('click', function () {
        $('#RealEName').val($('#r_REName').text());
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        $('#tblPurchaseDetails tbody').empty();
        const fd = new FormData(document.forms.form1);
        const model = Object.fromEntries(fd);
        get_purchase_Data(model, $form, 'csv');

        common.callAjax(_url.get_t_reale_purchase_CSVData, model,
            function (result) {
                //sucess
                var table_data = result.data;

                var csv = common.getJSONtoCSV(table_data);
                if (!(csv == "ERROR")) {
                    l_logfunction(model.RealECD + ' ' + model.RealEName, 'csv', '');

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
                    downloadLink.download = "案件一覧_" + dateString + ".csv";

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
}

function get_purchase_Data(model, $form, state) {
    common.callAjaxWithLoading(_url.get_t_reale_purchase_DisplayData, model, this, function (result) {
        if (result && result.isOK) {
            Bind_DisplayData(result.data);
            if (state == 'Display')
                l_logfunction(model.RealECD + ' ' + model.RealEName, 'display', '');
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

function Bind_DisplayData(result) {
    let _letter = "", _class = "", _status = "";
    let data = JSON.parse(result);
    let html = "";
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (isEmptyOrSpaces(data[i]["ステータス"])) {
                _letter = "";
                _status = "0";
                _class = "ms-1 ps-1 pe-1 rounded-circle";
            }
            else {
                if (data[i]["ステータス"] == "買取依頼") {
                    _letter = "買";
                    _status = "1";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-success text-white";
                }
                else if (data[i]["ステータス"] == "確認中") {
                    _letter = "確";
                    _status = "2";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-warning ext-dark";
                }
                else if (data[i]["ステータス"] == "交渉中") {
                    _letter = "交";
                    _status = "3";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-info txt-dark";
                }
                else if (data[i]["ステータス"] == "成約") {
                    _letter = "成";
                    _status = "4";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-secondary";
                }
                else if (data[i]["ステータス"] == "売主辞退") {
                    _letter = "辞";
                    _status = "5";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-light text-danger";
                }
                else if (data[i]["ステータス"] == "買主辞退") {
                    _letter = "辞";
                    _status = "6";
                    _class = "ms-1 ps-1 pe-1 rounded-circle bg-dark text-white";
                }
            }

            var DeepDatetime = data[i]["詳細査定日時"].replace(" ", "&");
            html += '<tr>\
            <td class= "text-end"> ' + (i + 1) + '</td>\
            <td><span class="' + _class + '">' + _letter + '</span><span class="font-semibold"> ' + data[i]["ステータス"] + '</span></td>\
            <td class="d-none">'+ _status + '</td>\
            <td class="text-nowrap">'+ data[i]["査定依頼ID"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id='+ data[i]["SellerMansionID"] + '&' + data[i]["SellerCD"] + '&' + data[i]["査定依頼ID"] + '&' + DeepDatetime + ' data-bs-toggle="modal" data-bs-target="#t_reale" href="#" onclick="Bind_ModalDetails(this.id)"><span>' + data[i]["物件名"] + '</span></a></td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id="' + data[i]["SellerCD"] + '" href="#" onclick="l_logfunction(' + '\'t_seller_assessment ' + data[i]["SellerCD"] + '\', ' + '\'link\'' + ', this.id)">' + data[i]["売主名"] + '</a></td>\
            <td class="text-nowrap">'+ data[i]["登録日時"] + '</td>\
            <td class="text-nowrap">'+ data[i]["簡易査定日時"] + '</td>\
            <td><a class="text-heading font-semibold text-decoration-underline text-nowrap" id="' + data[i]["査定依頼ID"] + '" href="#" onclick="l_logfunction(' + '\'t_seller_assessment_detail ' + data[i]["査定依頼ID"] + '\', ' + '\'link\'' + ', this.id)">' + data[i]["詳細査定日時"] + '</a></td>\
            <td class="text-nowrap">'+ data[i]["買取依頼日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["送客日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["成約日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["売主辞退日時"] + '</td>\
            <td class="text-nowrap"> '+ data[i]["買主辞退日時"] + '</td>\
            </tr>'
        }

        $('#total_record').text("検索結果： " + data.length + "件");
        $('#total_record_up').text("検索結果： " + data.length + "件");
        $('#no_record').text("");
    }
    else {
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件 ");
        $('#no_record').text("表示可能データがありません");
    }
    $('#tblPurchaseDetails tbody').append(html);
}

function l_logfunction(remark, Process, id) {
    let model = {
        LoginKBN: '3',
        LoginID: null,
        RealECD: null,
        LoginName: null,
        IPAddress: null,
        Page: 't_reale_purchase',
        Processing: Process,
        Remarks: remark,
    };
    common.callAjax(_url.Insert_L_Log, model, function (result) {
        if (result && result.isOK) {
            if (remark == 't_seller_assessment ' + id)
                window.location.href = common.appPath + '/t_seller_assessment/Index?SellerCD=' + id;
            else if (remark == 't_seller_assessment_detail ' + id)
                alert(common.appPath + '/t_seller_assessment_detail/Index?AssReqID=' + id);
                //window.location.href = common.appPath + '/t_seller_assessment_detail/Index?AssReqID=' + id;
        }
    });
}
