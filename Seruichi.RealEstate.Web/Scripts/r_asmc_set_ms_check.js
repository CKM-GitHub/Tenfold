const _url = {};

$(function () {
    _url.saveTemplate = common.appPath + '/r_asmc_set_ms_check/SaveTemplate';
    _url.insertAll = common.appPath + '/r_asmc_set_ms_check/InsertAll';
    _url.gotoBack = common.appPath + '/r_asmc_set_ms_check/GotoBack';
    _url.gotoAsmcTop = common.appPath + '/r_asmc_set_ms_check/GotoAsmcTop';

    setValidation();
    addEvents();
});

function setValidation() {
    //買取利回り設定
    $('#EstimatedRent')
        .addvalidation_errorElement("#errorEstimatedRent")
        .addvalidation_reqired()
        .addvalidation_money(10);

    //テンプレート名
    $('#TemplateName')
        .addvalidation_errorElement("#errorTemplateName")
        .addvalidation_reqired()
        .addvalidation_singlebyte_doublebyte();
}

function addEvents() {
    common.bindValidationEvent('#AssessmentSimulation');
    common.bindValidationEvent('#TemplateModal');

    //推定価格表示
    $('#btnDisplayPrice').on('click', function () {
        const $panel = $('#AssessmentSimulation').hideChildErrors();
        if (!common.checkValidityOnSave('#AssessmentSimulation')) {
            common.setFocusFirstError($panel);
            return false;
        }

        const rent = Number($('#EstimatedRent').val().replaceAll(',',''));
        const rate = Number($('#Rate').text());

        if (!isNaN(rent) && !isNaN(rate) && rate !== 0) {
            let price = rent * 12 / (rate / 100);
            price = Math.floor(price / 1000000) * 1000000;
            $('#EstimatedPrice').text(price.toLocaleString());
        }
    });

    //テンプレート保存
    $('#btnSaveTemplate').on('click', function () {
        const $panel = $('#TemplateModal').hideChildErrors();
        if (!common.checkValidityOnSave('#TemplateModal')) {
            common.setFocusFirstError($panel);
            return false;
        }

        const model = {
            templateName: $('#TemplateName').val(),
        }

        common.callAjaxWithLoading(_url.saveTemplate, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#TemplateModal').modal('hide');
                $('#TemplateCompletedModal').modal('show');
            } else if (result && result.data) {
                //validation error
                common.setValidationErrors(result.data);
                common.setFocusFirstError($panel);
            } else if (result && result.message) {
                //templatename error
                alert(result.message.MessageText1);
            } else {
                alert('テンプレートの保存に失敗しました。');
            }
        });
    });

    //登録
    $('#btnRegister').on('click', function () {
        const model = {
            validFLG: $('#ValidFLGCheck').prop('checked') ? 1 : 0,
        }
        common.callAjaxWithLoading(_url.insertAll, model, this, function (result) {
            if (result && result.isOK) {
                //sucess
                $('#SaveModal').modal('hide');
                $('#SaveCompletedModal').modal('show');
            } else {
                alert('登録に失敗しました。');
            }
        });
    });

    //登録完了
    $('#btnCompleted').on('click', function () {
        const fd = new FormData();
        fd.append('asmcTopPage', $(this).data('asmc-top-page'));

        const form = document.createElement("form");
        document.body.append(form);
        common.callSubmit(form, _url.gotoAsmcTop, fd);
    });

    //戻る
    $('#btnBack').on('click', function () {
        const form = document.createElement("form");
        document.body.append(form);
        common.callSubmit(form, _url.gotoBack);
    });
}
