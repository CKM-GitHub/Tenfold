const _url = {};
$(function () {
    _url.DeleteData = common.appPath + '/r_auto_mes/DeleteData';
    _url.InsertUpdateData = common.appPath + '/r_auto_mes/InsertUpdateData';
    setValidation();
    addEvents();
});

function setValidation() {
    $('#TemplateName')
        .addvalidation_errorElement("#errorTemplateName")
        .addvalidation_reqired()//E101
        .addvalidation_singlebyte_doublebyte(); //E105
        

    $('#TemplateContent')
        .addvalidation_errorElement("#errorTemplateContent")
        .addvalidation_reqired()//E101
        .addvalidation_singlebyte_doublebyte(); //E105
}

function addEvents() {
    common.bindValidationEvent('#form1', '');


    //checkbox check or not
    $('#ChkFlg').on('change', function () {
        this.value = this.checked ? 1 : 0;
    }).change();


    //btnDel
    $('#btnConfirm').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val()

        let model = {
            MessageSEQ: $HiddenSEQ,
            ProcessKBN: "Delete",
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        common.callAjaxWithLoading(_url.DeleteData, model, this, function (result) {
            if (result && result.isOK) {
                window.location.href = common.appPath + '/r_auto_mes/Index';
            }
            else {
                alert("Processing UnSuccessfull!!");
            }
        });

    });


    //btnDelClose
    $('#btnCloseUp').on('click', function () {
        window.location.href = common.appPath + '/r_auto_mes/Index';
    });


    //btnInsertUpdate
    $('#btnSend').on('click', function () {
        $form = $('#form1').hideChildErrors();
        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

       const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val(), $TemplateName = $("#TemplateName").val(),
            $TemplateContent = $("#TemplateContent").val(), $chk = $("#ChkFlg").val()
        var $mode = "0";
        if ($HiddenSEQ == "") {
            $mode = "1";
        }
        else {
            $mode = "2";
        }

        let model = {
            MessageSEQ: $HiddenSEQ,
            MessageTitle: $TemplateName,
            MessageTEXT: $TemplateContent,
            ValidFlg: $chk,
            Mode: $mode,
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        common.callAjaxWithLoading(_url.InsertUpdateData, model, this, function (result) {
            if (result && result.isOK) {
                $('#message-com').modal('hide');
                window.location.href = common.appPath + '/r_auto_mes/Index';
            }
            else {
                alert("Processing UnSuccessfull!!");
            }

        });

    });


    //btnNew
    $('#btnNew').on('click', function () {
        $('#message-com').modal('show');
        $('#TemplateName').hideError();
        $('#TemplateContent').hideError();
    });
}
function Get_MsgSEQ(id) {
    $('#MessageTitle').hideError();
    $('#MessageText').hideError();
    $('#TemplateName').hideError();
    $('#TemplateContent').hideError();

    $('#MsgSEQ').val(id.split('&')[0]);
    $('#TemplateName').val(id.split('&')[1]);
    $('#TemplateContent').val(id.split('&')[2]);
    var data = id.split('&')[3];
    if (data == "適用中") {
        $("#ChkFlg").attr("checked", true);
    }
    else {
        $("#ChkFlg").attr("checked", false);
    }
}