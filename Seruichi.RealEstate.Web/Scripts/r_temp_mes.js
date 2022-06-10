const _url = {};
$(function () {

    _url.insert_M_REMessage = common.appPath + '/r_temp_mes/Insert_M_REMessage';
    _url.update_M_REMessage = common.appPath + '/r_temp_mes/Update_M_REMessage';
    _url.delete_M_REMessage = common.appPath + '/r_temp_mes/Delete_M_REMessage';
    setValidation();
    addEvents();
});

function setValidation()
{
    $('#txtMsgTitle')
        .addvalidation_errorElement("#error_txtMsgTitle")
        .addvalidation_reqired()  //E101
        .addvalidation_singlebyte_doublebyte();

    $('#txtMsgContent')
        .addvalidation_errorElement("#error_txtMsgContent")
        .addvalidation_reqired()  //E101
        .addvalidation_singlebyte_doublebyte();

}
function addEvents()
{
    //$('#btnInsert').on('click', function () {
    //    $form = $('#form1').hideChildErrors();

    //    if (!common.checkValidityOnSave('#form1')) {
    //        $form.getInvalidItems().get(0).focus();
    //        return false;
    //    }
    //    let model = {
    //        MessageTitle: $('#txtMsgTitle').val(),
    //        MessageTEXT: $('#txtMsgContent').val(),
    //    };

    //    common.callAjax(_url.insert_M_REMessage, model, function (result) {
    //        if (result && result.isOK) {
    //            window.location.reload();
    //        }
    //        if (result && !result.isOK) {
    //            alert("UnInsertSuccessfull!!")
    //        }
    //    });
    //});
    common.bindValidationEvent('#form1', '');
    $('#btnUpdate').on('click', function ()
    {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }

        if ($('#txtMsgSEQ').val() != "")
        {//For update
            let model = {
                MessageSEQ: $('#txtMsgSEQ').val(),
                MessageTitle: $('#txtMsgTitle').val(),
                MessageTEXT: $('#txtMsgContent').val(),
            };

            common.callAjax(_url.update_M_REMessage, model, function (result) {
                if (result && result.isOK) {
                    window.location.reload();
                }
                if (result && !result.isOK) {
                    alert("UnUpdate Successfull!!")
                }
            });
        }
        else
        {// For insert
            let model = {
                MessageTitle: $('#txtMsgTitle').val(),
                MessageTEXT: $('#txtMsgContent').val(),
            };

            common.callAjax(_url.insert_M_REMessage, model, function (result) {
                if (result && result.isOK) {
                    window.location.reload();
                }
                if (result && !result.isOK) {
                    alert("UnInsertSuccessfull!!")
                }
            });

        }
    });

    $('#btnDelete').on('click', function () {
        let model = {
            MessageSEQ: $('#txtMsgSEQ').val()
        };
        common.callAjax(_url.delete_M_REMessage, model, function (result) {
            if (result && result.isOK) {
                $('#message-delok').modal('show');
            }
            if (result && !result.isOK) {
                alert("UnDelete Successfull!!")
            }
        });
    });

    $('#btnDeleteClose').on('click', function ()
    {
        window.location.reload();
    });

    $('#btnNew').on('click', function ()
    {
        $('#message-com').modal('show');
        $('#txtMsgTitle').hideError();
        $('#txtMsgContent').hideError();
    });
}

function Get_MsgSEQ(id)
{
    $('#txtMsgTitle').hideError();
    $('#txtMsgContent').hideError();
    $('#txtMsgSEQ').val(id);
}

