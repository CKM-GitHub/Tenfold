const _url = {};

$(function () {
    setValidation();
    _url.getM_RealList = common.appPath + '/t_reale_list/getM_RealList';
    _url.generate_CSV = common.appPath + '/t_reale_list/Generate_CSV';
    _url.InsertL_Log = common.appPath + '/t_reale_list/InsertM_Reale_L_Log';
    addEvents();
});

function setValidation() {

    $('#txtrealEstateCompany')
        .addvalidation_errorElement("#errorRealEstate")
        .addvalidation_maxlengthCheck(50)

    $('.StatusChk')
        .addvalidation_errorElement("#errorStatusChkLenght")
        .addvalidation_checkboxlenght();
}
function addEvents() {

    common.bindValidationEvent('#form1', '');

    $('.StatusChk').on('change', function () {
        this.value = this.checked ? 1 : 0;
        if ($("input[type=checkbox]:checked").length > 0) {
            $('.StatusChk').hideError();
        }
    }).change();

    const $RealEstateCom = $("#txtrealEstateCompany").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
        $EffectiveChk = $("#chkEffective").val(),
        $InValidCheck = $("#chkInvalid").val()

        let model = {
            RealEStateCom: $RealEstateCom,
            PrefNameSelect: $PrefNameSelect,
            EffectiveChk: $EffectiveChk,
            InValidCheck: $InValidCheck
        };
    getM_RealList(model, this);

    $('#btnDisplay').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#no_record').text("");
        $('#Datatable tbody').empty();

        const $RealEstateCom = $("#txtrealEstateCompany").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
            $EffectiveChk = $("#chkEffective").val(),
            $InValidCheck = $("#chkInvalid").val()

            let model = {
                RealEStateCom: $RealEstateCom,
                PrefNameSelect: $PrefNameSelect,
                EffectiveChk: $EffectiveChk,
                InValidCheck: $InValidCheck
            };
        getM_RealList(model, $form);

    });

    $('#btnCSV').on('click', function () {
        $('#total_record').text("検索結果： 0件");
        $('#total_record_up').text("検索結果： 0件");
        $('#no_record').text("");
        $('#Datatable tbody').empty();

        const $RealEstateCom = $("#txtrealEstateCompany").val(), $PrefNameSelect = $('#PrefNameSelect option:selected').text(),
            $EffectiveChk = $("#chkEffective").val(),
            $InValidCheck = $("#chkInvalid").val()

        let model = {
            RealEStateCom: $RealEstateCom,
            PrefNameSelect: $PrefNameSelect,
            EffectiveChk: $EffectiveChk,
            InValidCheck: $InValidCheck
        };
        getM_RealList(model, this);

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
                    downloadLink.download = "不動産会社リスト_" + dateString + ".csv";

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
        
    });

}
function getM_RealList(model, $form) {
    common.callAjaxWithLoading(_url.getM_RealList, model, this, function (result) {
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
        RealeCD: id
    }
    common.callAjax(_url.InsertL_Log, model,
        function (result) {
            if (result && result.isOK) {
                window.location.href = common.appPath + '/t_reale_purchase/Index?RealeCD=' + model.RealeCD;
                //window.location.href = '/t_seller_assessment/Index';
                alert("https://www.seruichi.com/t_reale_purchase?RealeCD" + model.RealeCD);
            }
            if (result && !result.isOK) {

            }
        });
}