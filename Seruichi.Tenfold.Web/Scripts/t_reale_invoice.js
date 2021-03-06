const _url = {};
$(function () {
    _url.get_t_reale_CompanyInfo = common.appPath + '/t_reale_invoice/get_t_reale_CompanyInfo';
    _url.get_t_reale_CompanyCountingInfo = common.appPath + '/t_reale_invoice/get_t_reale_CompanyCountingInfo';
    _url.get_D_Billing_List = common.appPath + '/t_reale_invoice/get_D_Billing_List';
    //_url.Insert_L_Log = common.appPath + '/t_reale_memo/Insert_L_Log';
    setValidation();
    addEvents();
    $('#navbarDropdownMenuLink').addClass('font-bold active text-underline');
    $('#t_reale_invoice').addClass('font-bold text-underline');
});

function setValidation()
{
    $('#StartMonth')
        .addvalidation_errorElement("#errorStartMonth")
        .addvalidation_datecheck() //E125
        .addvalidation_datecompare(); //E126

    $('#EndMonth')
        .addvalidation_errorElement("#errorEndMonth")
        .addvalidation_datecheck() //E125
        .addvalidation_datecompare(); //E126
}

function Date_Compare() {
    $start = $('#StartMonth').val(), $end = $('#EndMonth').val()
    let model = {
        StartDate: $start,
        EndDate: $end
    };
    if (model.StartDate && model.EndDate) {
        if (model.StartDate <= model.EndDate) {
            $("#StartMonth").hideError();
            $("#EndMonth").hideError();
            return;
        }
        else {
            $("#StartMonth").showError(common.getMessage('E126'));
            $("#EndMonth").showError(common.getMessage('E126'));
            return;
        }
    }
}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#StartMonth, #EndMonth').on('change', function () {
        Date_Compare();
    });
    const $Range = $("#Range").val(), $StartDate = $("#StartMonth").val(), $EndDate = $("#EndMonth").val()
    let model = {
        RealECD: common.getUrlParameter('RealECD'),
        Range: $Range,
        StartDate: $StartDate,
        EndDate: $EndDate,
        Option: 1,
    };
    get_D_Billing_List(model, this);
    sortTable.getSortingTable("billingtable");

    $('#btnDisplay').on('click', function () {
        $('#total_record').text("")
        $('#total_record_up').text("")
        $('#no_record').text("");
        $('#billingtable tbody').empty();
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $Range = $("#Range").val(), $StartDate = $("#StartMonth").val(), $EndDate = $("#EndMonth").val()

        let model = {
            RealECD: common.getUrlParameter('RealECD'),
            Range: $Range,
            StartDate: $StartDate,
            EndDate: $EndDate,
            Option: 0,
        };
        get_D_Billing_List(model, $form);
    });

    $('#btn1').click(function () {

        let model = {
            RealECD: "RE01"
        }

        $.ajax({
            url: _url.GeneratePDF,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(model),
            responseType: 'arraybuffer',
            headers: {
                RequestVerificationToken: $("#_RequestVerificationToken").val(),
            },
            success: function (data) {
                var atobData = atob(data);
                var num = new Array(atobData.length);
                for (var i = 0; i < atobData.length; i++) {
                    num[i] = atobData.charCodeAt(i);
                }
                var pdfData = new Uint8Array(num);

                //var blob = new Blob([pdfData], { type: 'text/plain' });
                blob = new Blob([pdfData], { type: 'application/pdf;base64' });
                var url = URL.createObjectURL(blob);

                var a = document.createElement('a');
                a.href = url;
                a.download = 'File.pdf';
                a.click();

                window.open(url);
            }
        });
    });
}


function get_D_Billing_List(model, $form) {
    common.callAjax(_url.get_D_Billing_List, model, function (result) {
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
                <td class="text-end d-none">' + (i + 1) + '</td>\
                <td>' + data[i]["InvoiceYYYYMM"] + '</td>\
                <td>' + data[i]["InsertDateTime"] + '</td>\
                <td>' + data[i]["TransferDate"] + '</td>\
                <td class="text-end">' + data[i]["AreaPlanFee"] + '</td>\
                <td class="text-end">' + data[i]["ManPlanFee"] + '</td>\
                <td class="text-end">' + data[i]["SendTimes"] + '</td>\
                <td class="text-end">' + data[i]["TransferFee"] + '</td>\
                <td class="text-end">' + data[i]["OtherFee"] + '</td>\
                <td class="text-end">' + data[i]["BaseAmount"] + '</td>\
                <td class="text-end">' + data[i]["TaxAmount"] + '</td>\
                <td class="text-end">' + data[i]["BillingAmount"] + '</td>\
                <td class="text-center"> <button type="button" class="btn btn-primary btn" id="'+ data[i]["InvoiceNo"] + '&' + data[i]["RealECD"] + '&' + data[i]["InvoiceYYYYMM"] + '">出力</button> </td>\
            </tr>'
    }
    //html +='<tr>\
    //     <td class="text-end d-none"> 1 </td>\
    //    <td> 2020/12 </td>\
    //    <td> 2020/12/25</td>\
    //    <td> 2020/12/25</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 99</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-end"> 9,999,999,999</td>\
    //    <td class="text-center"> <button type="button" class="btn btn-primary btn">出力</button> </td>\
    //</tr>\
    //    <tr>\
    //    <td class="text-end d-none"> 2 </td>\
    //    <td> 2020/13 </td>\
    //        <td> 2020/12/30</td>\
    //        <td> 2020/12/30</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-end"> 0</td>\
    //        <td class="text-center"> <button type="button" class="btn btn-primary">出力</button> </td>\
    //                                    </tr>'
    if (data.length > 0) {
        $('#total_record').text("検索結果：" + data.length + "件")
        $('#total_record_up').text("検索結果：" + data.length + "件")
    }
    else {
        $('#total_record').text("検索結果： 0件")
        $('#total_record_up').text("検索結果： 0件")
        $('#no_record').text("表示可能データがありません");
    }
    $('#billingtable tbody').append(html);
}
