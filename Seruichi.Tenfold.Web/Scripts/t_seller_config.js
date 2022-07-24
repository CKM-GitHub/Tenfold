const _url = {};
//const SlotsNum = null, Additionslots = null, AmountBilled = null, Reassessment = null;
const _session = {};

$(function () {
    _url.SelectFromMultipurpose = common.appPath + '/t_seller_config/SelectFromMultipurpose';
    _url.InsertUpdateToMultipurpose = common.appPath + '/t_seller_config/InsertUpdateToMultipurpose';
    _url.InsertL_Log = common.appPath + '/t_seller_config/InsertL_Log';
    setValidation();
    addEvents();
});

function setValidation() {

    $('#slotsNum')
        .addvalidation_errorElement("#errorslots")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);

    $('#additionslotsNum')
        .addvalidation_errorElement("#erroradditions")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);


    $('#Amountbilled')
        .addvalidation_errorElement("#errorBilled")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);

    $('#reassessment')
        .addvalidation_errorElement("#errorReassessment")
        .addvalidation_reqired()
        .addvalidation_onebyte_character()
        .addvalidation_maxlengthCheck(2);
}

$(document).ready(function () {
    let model1 = {
        DataID: 112
    };
    common.callAjaxWithLoading(_url.SelectFromMultipurpose, model1, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;
            if (length > 0) {
                let data = dataArray[0];
                $('#slotsNum').val(data.Num1);
                SlotsNum = data.Num1;
            }
            else {
                $('#slotsNum').val(0);
                SlotsNum = "0";
            }
        }
    )

    let model2 = {
        DataID: 108
    };
    common.callAjaxWithLoading(_url.SelectFromMultipurpose, model2, this,
        function (result) {
            
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;
            if (length > 0) {
                let data = dataArray[0];
                $('#additionslotsNum').val(data.Num1);
                Additionslots = data.Num1;
            }
            else {
                $('#additionslotsNum').val(0);
                Additionslots = "0";
            }
        }
    )

    let model3 = {
        DataID: 107
    };
    common.callAjaxWithLoading(_url.SelectFromMultipurpose, model3, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#Amountbilled').val(data.Num1);
                AmountBilled = data.Num1;
            }
            else {
                $('#Amountbilled').val(0);
                AmountBilled = "0";
            }
        }
    )

    let model4 = {
        DataID: 114
    };
    common.callAjaxWithLoading(_url.SelectFromMultipurpose, model4, this,
        function (result) {
            const dataArray = JSON.parse(result.data);
            const length = dataArray.length;

            if (length > 0) {
                let data = dataArray[0];
                $('#reassessment').val(data.Num1);
                Reassessment = data.Num1;
            }
            else {
                $('#reassessment').val(0);
                Reassessment = "0";
            }
        }
    )
   
});

function addEvents() {
    $('#btnSave').on('click', function () {

        const $this = $(this), $slots = $('#slotsNum').val(), $additionslots = $('#additionslotsNum').val(),
            $billed = $('#Amountbilled').val(), $reassessment = $('#reassessment').val()

        if (SlotsNum != $slots) {
            let mode = 0;
            if (SlotsNum == 0) {
                mode = 1;
            }
            else {
                mode = 2;
            }
            let model1 = {
                DataID: 112,
                Num: $slots,
                Mode: mode
            };
            common.callAjaxWithLoading(_url.InsertUpdateToMultipurpose, model1, this,
                function (result) {
                    if (result && result.isOK) {
                       
                    }
                    
                }
            )

        }

        if (Additionslots != $additionslots) {
            let mode = 0;
            if (Additionslots == 0) {
                mode = 1;
            }
            else {
                mode = 2;
            }

            let model1 = {
                DataID: 108,
                Num: $additionslots,
                Mode: mode,
            };
            common.callAjaxWithLoading(_url.InsertUpdateToMultipurpose, model1, this,
                function (result) {
                    if (result && result.isOK) {

                    }
                }
            )
        }

        if (AmountBilled != $billed) {
            let mode = 0;
            if (AmountBilled == 0) {
                mode = 1;
            }
            else {
                mode = 2;
            }
            
            let model1 = {
                DataID: 107,
                Num: $billed,
                Mode: mode
            };
            common.callAjaxWithLoading(_url.InsertUpdateToMultipurpose, model1, this,
                function (result) {
                    if (result && result.isOK) {

                    }
                }
            )
        }

        if (Reassessment != $reassessment) {
            let mode = 0;
            if (Reassessment == 0) {
                mode = 1;
            }
            else {
                mode = 2;
            }

            let model1 = {
                DataID: 114,
                Num: $reassessment,
                Mode: mode
            };
            common.callAjaxWithLoading(_url.InsertUpdateToMultipurpose, model1, this,
                function (result) {
                    if (result && result.isOK) {

                    }
                    
                }
            )
        }

        let model = {
            LoginKBN: null,
            LoginID: null,
            RealECD: null,
            LoginName: null,
            IPAddress: null,
            Page: null,
            Processing: null,
            Remarks: null
        }
        common.callAjax(_url.InsertL_Log, model,
            function (result) {
                if (result && result.isOK) {
                    alert("Insert/Update Successfully!!")
                }
            });

    });
}