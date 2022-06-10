const _url = {};
$(function () {
    _url.DeleteData = common.appPath + '/r_auto_mes/DeleteData';
    _url.InsertData = common.appPath + '/r_auto_mes/InsertUpdateData';
    setValidation();
    addEvents();
});

function setValidation() {

}

function addEvents() {
    common.bindValidationEvent('#form1', '');

    $('#chk1').on('change', function () {
        this.value = this.checked ? 1 : 0;
    }).change();

    //const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val(), $TemplateName = $("#TemplateName").val(),
    //    $TemplateContent = $("#TemplateContent").val(), $chk = $("#ChkFlg").val()
    const $mode = "0";
    const $this = $(this), $HiddenSEQ = $("#MsgSEQ").val()

    $('#btnEdit').on('click', function () {
         $mode = "2";
    });

    $('#btnNew').on('click', function () {
        $mode = "1";
    });

    //$('#btnDel').on('click', function () {
    //    const $this = $(this), $chk = $("#chk1").val()

    //    let model = {
    //        MessageSEQ =$HiddenSEQ
    //    };

    //});

    $('#btnConfirm').on('click', function () {
        
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

    $('#btnSend').on('click', function () {
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

        common.callAjaxWithLoading(_url.InsertUpdateData, model, this, function (result) {
            if (result && result.isOK) {
                $('#message-com').modal('hide');
                window.location.href = common.appPath + '/r_auto_mes/Index';
            }
            
        });

    });
}