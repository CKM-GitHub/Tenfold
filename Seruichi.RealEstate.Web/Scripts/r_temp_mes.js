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
        .addvalidation_singlebyte_doublebyte(); //E105

    $('#txtMsgContent')
        .addvalidation_errorElement("#error_txtMsgContent")
        .addvalidation_reqired()  //E101
        .addvalidation_singlebyte_doublebyte();//E105


    $('#txtMsgTitle_New')
        .addvalidation_errorElement("#error_txtMsgTitle_New")
        .addvalidation_reqired()  //E101
        .addvalidation_singlebyte_doublebyte(); //E105

    $('#txtMsgContent_New')
        .addvalidation_errorElement("#error_txtMsgContent_New")
        .addvalidation_reqired()  //E101
        .addvalidation_singlebyte_doublebyte();//E105


}
function addEvents()
{
    common.bindValidationEvent('#form1', '');
    $('#btnInsert').on('click', function () {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
        //const $this = $(this)
        //if (!common.checkValidityInput($this)) {
        //    return false;
        //}
        let model = {
            MessageTitle: $('#txtMsgTitle_New').val(),
            MessageTEXT: $('#txtMsgContent_New').val(),
        };

        common.callAjax(_url.insert_M_REMessage, model, function (result) {
            if (result && result.isOK) {
                window.location.reload();
            }
            if (result && !result.isOK) {
                alert("UnInsertSuccessfull!!")
            }
        });
    });
   
    $('#btnUpdate').on('click', function ()
    {
        $form = $('#form1').hideChildErrors();

        if (!common.checkValidityOnSave('#form1')) {
            $form.getInvalidItems().get(0).focus();
            return false;
        }
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
        $('#message-com-ins').modal('show');
        $('#txtMsgTitle_New').hideError();
        $('#txtMsgContent_New').hideError();
    });
}

function Get_MsgSEQ(id)
{

    $('#txtMsgTitle').hideError();
    $('#txtMsgContent').hideError();

    $('#txtMsgSEQ').val(id.split('&')[0]);
    $('#txtMsgTitle').val(id.split('&')[1]);
    $('#txtMsgContent').val(id.split('&')[2]);

}

