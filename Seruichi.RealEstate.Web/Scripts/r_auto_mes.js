const _url = {};
$(function () {
    _url.DeleteData = common.appPath + '/r_auto_mes/DeleteData';
    _url.InsertData = common.appPath + '/r_auto_mes/InsertUpdateData';
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

    $('#chk1').on('change', function () {
        this.value = this.checked ? 1 : 0;
    }).change();

    //const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val(), $TemplateName = $("#TemplateName").val(),
    //    $TemplateContent = $("#TemplateContent").val(), $chk = $("#ChkFlg").val()
    var $mode = "0";
    const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val()
   

    $('#btnEdit').on('click', function () {
        debugger;
        $mode = "2";
        $('#TemplateName').val($('#MessageTitle').text());
        $("#TemplateContent").val($("#MessageText").text());
        
    });

    $('#btnNew').on('click', function () {
        $mode = "1";
    });

    $('#btnConfirm').on('click', function () {
        debugger;
        let model = {
            MessageSEQ: $HiddenSEQ,
            ProcessKBN: "Delete",
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        common.callAjax(_url.DeleteData, model,
            function (result) {
                if (result && !result.isOK) {
                    $('#message-delok').modal('hide');
                }
            });

    });

    $('#btnCloseUp').on('click', function () {
        window.location.href = common.appPath + '/r_auto_mes/Index';
    });

    
    $('#btnBack').on('click', function () {
        debugger;
        $("#TemplateName").hideError();
        $("#TemplateContent").hideError();
    });

    $('#btnSend').on('click', function () {

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

       const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val(), $TemplateName = $("#TemplateName").val(),
            $TemplateContent = $("#TemplateContent").val(), $chk = $("#ChkFlg").val()

        let model = {
            MessageSEQ: $HiddenSEQ,
            MessageTitle: $TemplateName,
            MessageTEXT: $TemplateContent,
            ValidFlg: $chk,
            Mode: $mode,
            Remarks: "MessageSEQ：" + $HiddenSEQ
        };

        debugger;
        if (model.MessageTitle == "" && model.MessageTEXT == "") {
            document.getElementById("btnSend").disabled = true;
        }
        else {
            common.callAjaxWithLoading(_url.InsertUpdateData, model, this, function (result) {
                if (result && result.isOK) {
                    $('#message-com').modal('hide');
                    window.location.href = common.appPath + '/r_auto_mes/Index';
                }

            });
        }

        

    });
}
//function refreshPage() {
//    debugger;
//    window.location.reload();
//} 